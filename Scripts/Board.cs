using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    public GameControl controller;
    private Matrix matrix;
    private List<Tile> tiles;
    public bool gameOnHold;
    public bool autoEnabled;
    public int greatestTileValue;
    [SerializeField] private AutoRun autoRun;
    [SerializeField] private Tile tile; 
    [SerializeField] private TileState[] tileStates; 

    private void Awake() {
        matrix = GetComponentInChildren<Matrix>();
        tiles = new List<Tile>(16); 
        gameOnHold = false;
        autoEnabled = false;
        greatestTileValue = 0;
    }

    private void Update() {
        if (!gameOnHold) {
            if (!autoEnabled) {
                if (Input.GetKeyDown(KeyCode.UpArrow)) {
                    MoveTiles(Vector2Int.up);
                } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                    MoveTiles(Vector2Int.down);
                } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                    MoveTiles(Vector2Int.left);
                } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                    MoveTiles(Vector2Int.right);
                }
            } else {
                MoveTiles(autoRun.GetOptimalMove());
            }
        }
    }

    private IEnumerator PostMoveAction() {
        gameOnHold = true;
        yield return new WaitForSeconds(0.1f);
        if (tiles.Count < 16) {
            CreateNewTile();
        }
        if (GameOver()) {
            controller.GameOver();
        }
        foreach (Tile tile in tiles) {
            tile.canMerge = true;
        }
        gameOnHold = false;
    }

    public void CreateNewTile() {
        Tile tile = Instantiate(this.tile, matrix.transform);
        int random = Random.Range(0, 2);
        tile.SetState(tileStates[random]);
        tile.SpawnAt(matrix.GetEmptyCell());
        tiles.Add(tile);
        if (tile.state.number > greatestTileValue) {
            greatestTileValue = tile.state.number;
        }
    }

    private void MoveTiles(Vector2Int direction) {
        int startX = 0;
        int incrementX = 1;
        int startY = 1;
        int incrementY = 1;
        if (direction == Vector2Int.down) {
            startY = matrix.numRows - 2;
            incrementY = -1;
        } else if (direction == Vector2Int.left) {
            startX = 1;
            startY = 0;
        } else if (direction == Vector2Int.right) {
            startX = matrix.rows[0].numCells - 2;
            incrementX = -1;
            startY = 0;
        }
        bool moved = false;
        for (int i = startX; i >= 0 && i < matrix.rows[0].numCells; i += incrementX) {
            for (int j = startY; j >= 0 && j < matrix.numRows; j += incrementY) {
                Cell cell = matrix.GetCellAt(i, j);
                if (cell.IsOccupied()) {
                    moved = MoveTile(cell.tile, direction) || moved;
                }
            }
        }
        if (moved) {
            StartCoroutine(PostMoveAction());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction) {
        Cell targetCell = null;
        Cell nextCell = matrix.GetAdjacentCell(tile.cell, direction);
        while (nextCell != null) {
            if (nextCell.IsOccupied()) {
                if (CanMerge(tile, nextCell.tile)) {
                    MergeTiles(tile, nextCell.tile);
                    return true;
                }
                break;
            }
            targetCell = nextCell;
            nextCell = matrix.GetAdjacentCell(nextCell, direction);
        }
        if (targetCell != null) {
            tile.MoveTo(targetCell);
            return true;
        }
        return false;
    }

    private bool CanMerge(Tile t1, Tile t2) {
        if (t1.state.number == t2.state.number && t2.canMerge) {
            return true;
        }
        return false;
    }

    private void MergeTiles(Tile t1, Tile t2) {
        tiles.Remove(t1);
        t1.MergeIntoAndDestroy(t2.cell);
        t2.canMerge = false;
        TileState nextTileState = GetNextTileState(t2.state);
        controller.UpdateScore(nextTileState.number);
        t2.SetState(nextTileState);
        if (nextTileState.number > greatestTileValue) {
            greatestTileValue = nextTileState.number;
        }
    }

    private TileState GetNextTileState(TileState state) {
        int number = state.number;
        int index = (int)Mathf.Log(number, 2);
        if (index > tileStates.Length - 1) {
            index = tileStates.Length - 1;
        }
        return tileStates[index];
    }

    public int NumEmptyCells() {
        return 16 - tiles.Count;
    }

    public int MaxValue() {
        return greatestTileValue;
    }

    public float Monotonicity() {
        float left = 0;
        float right = 0;
        for (int row = 0; row < matrix.numRows; row++) {
            int currentColumn = 0;
            Cell currentCell = matrix.GetCellAt(row, currentColumn);
            int nextColumn = 1;
            Cell nextCell = matrix.GetCellAt(row, nextColumn);
            while (nextColumn < 4) {
                while (nextColumn < 4 && !nextCell.IsOccupied()) {
                    nextColumn++;
                    nextCell = matrix.GetCellAt(row, nextColumn);
                }
                if (nextColumn == 4) { 
                    nextColumn--; 
                    nextCell = matrix.GetCellAt(row, nextColumn);
                }
                float currentValue = 0;
                if (currentCell.IsOccupied()) {
                    currentValue = Mathf.Log(currentCell.tile.state.number, 2);
                }
                float nextValue = 0;
                if (nextCell.IsOccupied()) {
                    nextValue = Mathf.Log(nextCell.tile.state.number, 2);
                }
                if (currentValue > nextValue) {
                    left += nextValue - currentValue;
                } else if (nextValue > currentValue) {
                    right += currentValue - nextValue;
                }
                currentColumn = nextColumn;
                currentCell = matrix.GetCellAt(row, currentColumn);
                nextColumn++;
                nextCell = matrix.GetCellAt(row, nextColumn);
            }
        }
        float top = 0;
        float bottom = 0;
        for (int column = 0; column < matrix.rows[0].numCells; column++) {
            int currentRow = 0;
            Cell currentCell = matrix.GetCellAt(currentRow, column);
            int nextRow = 1;
            Cell nextCell = matrix.GetCellAt(nextRow, column);
            while (nextRow < 4) {
                while (nextRow < 4 && !nextCell.IsOccupied()) {
                    nextRow++;
                    nextCell = matrix.GetCellAt(nextRow, column);
                }
                if (nextRow == 4) { 
                    nextRow--; 
                    nextCell = matrix.GetCellAt(nextRow, column);
                }
                float currentValue = 0;
                if (currentCell.IsOccupied()) {
                    currentValue = Mathf.Log(currentCell.tile.state.number, 2);
                }
                float nextValue = 0;
                if (nextCell.IsOccupied()) {
                    nextValue = Mathf.Log(nextCell.tile.state.number, 2);
                }
                if (currentValue > nextValue) {
                    top += nextValue - currentValue;
                } else if (nextValue > currentValue) {
                    bottom += currentValue - nextValue;
                }
                currentRow = nextRow;
                currentCell = matrix.GetCellAt(currentRow, column);
                nextRow++;
                nextCell = matrix.GetCellAt(nextRow, column);
            }
        }
        float monotonicity = Mathf.Max(left, right) + Mathf.Max(top, bottom);
        return monotonicity;
    }

    private Cell GetFurthestPosition(Tile tile, Vector2Int direction) {
        Cell targetCell = null;
        Cell nextCell = matrix.GetAdjacentCell(tile.cell, direction);
        while (nextCell != null) {
            if (nextCell.IsOccupied()) {
                return nextCell;
            }
            targetCell = nextCell;
            nextCell = matrix.GetAdjacentCell(nextCell, direction);
        }
        return targetCell;
    }

    public float Smoothness() {
        float smoothness = 0;
        for (int i = 0; i < matrix.numRows; i += 1) {
            for (int j = 0; j < matrix.rows[0].numCells; j += 1) {
                Cell cell = matrix.GetCellAt(i, j);
                if (cell.IsOccupied()) {
                    float value = Mathf.Log(cell.tile.state.number, 2);
                    Cell rightCell = GetFurthestPosition(cell.tile, Vector2Int.right);
                    if (rightCell != null && rightCell.IsOccupied()) {
                        float rightValue = Mathf.Log(rightCell.tile.state.number, 2);
                        smoothness -= Mathf.Abs(value - rightValue); // # of merges before equal state
                    }
                    Cell bottomCell = GetFurthestPosition(cell.tile, Vector2Int.down);
                    if (bottomCell != null && bottomCell.IsOccupied()) {
                        float bottomValue = Mathf.Log(bottomCell.tile.state.number, 2);
                        smoothness -= Mathf.Abs(value - bottomValue);
                    }
                }
            }
        }
        return smoothness;
    }

    private bool GameOver() {
        if (tiles.Count < 16) {
            return false;
        }
        foreach (Tile tile in tiles) {
            Cell up = matrix.GetAdjacentCell(tile.cell, Vector2Int.up);
            Cell down = matrix.GetAdjacentCell(tile.cell, Vector2Int.down);
            Cell left = matrix.GetAdjacentCell(tile.cell, Vector2Int.left);
            Cell right = matrix.GetAdjacentCell(tile.cell, Vector2Int.right);
            if (up != null && CanMerge(tile, up.tile) ||
                down != null && CanMerge(tile, down.tile) ||
                left != null && CanMerge(tile, left.tile) ||
                right != null && CanMerge(tile, right.tile)) {
                return false;
            }
        }
        return true;
    }

    public void Clear() {
        foreach (Tile tile in tiles) {
            tile.cell.tile = null;
            tile.cell = null;
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }
}
