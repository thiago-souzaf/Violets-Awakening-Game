using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class InteractionWidget : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI m_inputText;
	[SerializeField] private TextMeshProUGUI m_actionText;
    private CanvasGroup m_canvasGroup;

    // Text
    private string m_inputString = "E";
    private string m_actionString = "";

    // Visibility
    public float fadeDuration = 1.0f;
    private bool m_isVisible = false;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        m_inputText.text = m_inputString;
        m_actionText.text = m_actionString;
        transform.rotation = Camera.main.transform.rotation;


        m_canvasGroup.alpha = 0;
    }

    public void SetActionText(string text)
    {
        m_actionString = text;
        m_actionText.text = m_actionString;
    }

    public void SetInputText(string text)
    {
        m_inputString = text;
        m_inputText.text = m_inputString;
    }

    [ContextMenu("Show")]
    public void Show()
    {
        m_isVisible = true;
        StartCoroutine(Fade());
    }

    [ContextMenu("Hide")]
    public void Hide()
    {
        m_isVisible = false;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        float currentAlpha = m_canvasGroup.alpha;
        float targetAlpha = m_isVisible ? 1 : 0;
        float startTime = Time.time;
        float endTime = Time.time + fadeDuration;

        while (Time.time < endTime)
        {
            float timeProgressed = (Time.time - startTime) / fadeDuration;
            m_canvasGroup.alpha = Mathf.Lerp(currentAlpha, targetAlpha, timeProgressed);
            yield return null;
        }
        m_canvasGroup.alpha = targetAlpha;
    }
}
