using TMPro;
using UnityEngine;
using System.Collections;

public class DamagePopup : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    public CanvasGroup canvasGroup;

    public float moveY = 1.0f;
    public float duration = 0.9f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 startPos;
    private float elapsed;
    private int poolId;         
    private PoolManager poolManager;

    private void Awake()
    {
        if (tmp == null)
        {
            tmp = GetComponentInChildren<TextMeshProUGUI>();
        }

        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void Play(string text, Color color, Vector3 worldPos, PoolManager pool, int id)
    {
        tmp.text = text;
        tmp.color = color;
        poolManager = pool;
        poolId = id;

        Vector3 canvasPos = Camera.main.WorldToScreenPoint(worldPos);

        startPos = canvasPos;
        transform.position = startPos;
        canvasGroup.alpha = 1f;
        gameObject.SetActive(true);
        elapsed = 0f;

        StopAllCoroutines();
        StartCoroutine(Animate());
    }


    private IEnumerator Animate()
    {
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = ease.Evaluate(t);

            transform.position = startPos + Vector3.up * (eased * moveY);

            canvasGroup.alpha = 1f - Mathf.Clamp01((t - 0.4f) / 0.6f);

            yield return null;
        }

        poolManager.Release(gameObject);
    }
}
