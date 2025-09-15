using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class UpgradeNotifier : MonoBehaviour
{
    public TextMeshProUGUI upgradeText;
    public float slideDuration = 0.5f;
    public float displayTime = 2f;
    public Vector2 hiddenPosition; // posição fora da tela
    public Vector2 visiblePosition; // posição visível

    private RectTransform rectTransform;
    private Coroutine currentRoutine;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = hiddenPosition;
    }

    public void ShowUpgrade(string message)
    {
        // Se já está rodando uma animação, interrompe
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message));
    }

    IEnumerator ShowRoutine(string message)
    {
        // Atualiza o texto
        upgradeText.text = message;

        // Sobe o painel
        yield return StartCoroutine(MovePanel(rectTransform.anchoredPosition, visiblePosition, slideDuration));

        // Espera um pouco
        yield return new WaitForSeconds(displayTime);

        // Desce o painel
        yield return StartCoroutine(MovePanel(rectTransform.anchoredPosition, hiddenPosition, slideDuration));

        currentRoutine = null;
    }

    IEnumerator MovePanel(Vector2 start, Vector2 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }
    }
}
