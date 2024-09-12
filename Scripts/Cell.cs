using UnityEngine;

public class Cell : MonoBehaviour {
    public Vector2Int coords { get; set; }
    public Tile tile { get; set; }

    public bool IsOccupied() {
        return tile != null;
    }
}
