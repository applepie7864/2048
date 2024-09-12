using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {
    public Cell cell { get; set; }
    public bool canMerge { get; set; }
    public TileState state { get; private set; }
    private Image background;
    private TextMeshProUGUI text;

    private void Awake() {
        canMerge = true;
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private IEnumerator Animate(Vector3 to, bool isMerge) {
        float elapsed = 0f;
        float duration = 0.12f;
        Vector3 from = transform.position;
        while (elapsed < duration) {
            transform.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = to;
        if (isMerge) {
            Destroy(gameObject);
        }
    }

    public void SetState(TileState state) {
        this.state = state;
        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = state.number.ToString();
    }

    public void SpawnAt(Cell cell) {
        this.cell = cell;
        cell.tile = this;
        transform.position = cell.transform.position;
    }

    public void MoveTo(Cell cell) {
        this.cell.tile = null;
        this.cell = cell;
        cell.tile = this;
        StartCoroutine(Animate(cell.transform.position, false));
    }

    public void MergeIntoAndDestroy(Cell cell) {
        this.cell.tile = null;
        this.cell = null;
        StartCoroutine(Animate(cell.transform.position, true));
    }
}
