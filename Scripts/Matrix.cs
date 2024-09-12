using UnityEngine;

public class Matrix: MonoBehaviour {
    public int numRows => rows.Length;
    public Row[] rows { get; private set; }

    private void Awake() {
        rows = GetComponentsInChildren<Row>();
    }

    private void Start() {
        for (int i = 0; i < numRows; i++) {
            for (int j = 0; j < rows[i].numCells; j++) {
                rows[i].cells[j].coords = new Vector2Int(j, i);
            }
        }
    }

    public Cell GetEmptyCell() {
        int random = Random.Range(0, numRows);
        int index = random;
        while (rows[index].IsFull()) {
            index++;
            if (index == random) {
                return null;
            } else if (index > numRows - 1) {
                index = 0;
            }
        }
        return rows[index].GetEmptyCell();
    }

    public Cell GetCellAt(int x, int y) {
        if (y >= 0 && y < numRows) {
            return rows[y].GetCellAt(x);
        } 
        return null;
    }

    public Cell GetAdjacentCell(Cell cell, Vector2Int direction) {
        int x = cell.coords.x + direction.x;
        int y = cell.coords.y - direction.y;
        return GetCellAt(x, y);
    }
}
