using System.Collections;
using TMPro;
using UnityEngine;

public class GameControl : MonoBehaviour {
    public Board board;
    public CanvasGroup gameOver;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestText;
    private int score;
    private int best;


    private void Start() {
        NewGame();
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float targetAlpha, float delay) {
        yield return new WaitForSeconds(delay);
        float elapsed = 0f;
        float duration = 0.5f;
        float currentAlpha = canvasGroup.alpha;
        while (elapsed < duration) {
            canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = targetAlpha;
    }

    public void NewGame() {
        SetScore(0);
        board.greatestTileValue = 0;
        gameOver.alpha = 0f;
        gameOver.interactable = false;
        board.Clear();
        board.CreateNewTile();
        board.CreateNewTile();
        board.gameOnHold = false;
        board.autoEnabled = false;
        board.enabled = true;
    }

    public void GameOver() {
        board.enabled = false;
        gameOver.interactable = true;
        StartCoroutine(Fade(gameOver, 1f, 1f));
    }

    public void UpdateScore(int additionalPoints) {
        int newScore = score + additionalPoints;
        SetScore(newScore);
    }

    private void SetScore(int score) {
        this.score = score;
        scoreText.text = score.ToString();
        UpdateBest();
    }

    private void UpdateBest() {
        if (score > best) {
            best = score;
            bestText.text = best.ToString();
        }
    }
}
