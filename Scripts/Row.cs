using UnityEngine;

public class Row : MonoBehaviour {
    public int numCells => cells.Length;
    public Cell[] cells { get; private set; }

    private void Awake() {
        cells = GetComponentsInChildren<Cell>();
    }

    public bool IsFull() {
        foreach (Cell cell in cells) {
            if (!cell.IsOccupied()) {
                return false;
            }
        }
        return true;
    }

    public Cell GetEmptyCell() {
        int random = Random.Range(0, numCells);
        int index = random;
        while (cells[index].IsOccupied()) {
            index++;
            if (index == random) {
                return null;
            } else if (index > numCells - 1) {
                index = 0;
            }
        }
        return cells[index];
    }

    public Cell GetCellAt(int x) {
        if (x >= 0 && x < numCells) {
            return cells[x];
        } 
        return null;
    }
}
