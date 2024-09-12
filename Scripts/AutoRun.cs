using UnityEngine;

public class AutoRun : MonoBehaviour {
    [SerializeField] private Board board;

    public Vector2Int GetOptimalMove() {
        return Vector2Int.up; // to be implemented
    }

    private float StaticEvaluationFunction(Board board) {
        float empty = (float)board.NumEmptyCells();
        float emptyWeight = 2.7f;
        float smooth = board.Smoothness();
        float smoothWeight = 0.1f;
        float monotonic = board.Monotonicity();
        float monotonicWeight = 1.0f;
        float max = (float)board.MaxValue();
        float maxWeight = 1.0f;
        float staticEvaluation = Mathf.Log(empty, 2.0f) * emptyWeight + smooth * smoothWeight + monotonic * monotonicWeight + max * maxWeight;
        return staticEvaluation;
    }
}
