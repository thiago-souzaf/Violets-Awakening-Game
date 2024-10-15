using System;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject widgetPrefab;
    [SerializeField] private Vector3 widgetOffset;

    private bool m_isActive;
    public bool isAvailable;

    private GameObject go_widget;
    private InteractionWidget m_interactionWidget;

    public float interactionRadius = 5.0f;
    public float fadeDuration = 1.0f;

    public event EventHandler<InteractionEventArgs> OnInteract;

    private void Start()
    {
        go_widget = Instantiate(widgetPrefab, transform.position + widgetOffset, Quaternion.identity);
        go_widget.transform.SetParent(this.transform);
        m_interactionWidget = go_widget.GetComponent<InteractionWidget>();
        m_interactionWidget.fadeDuration = fadeDuration;
        isAvailable = true;

    }
    private void OnEnable()
    {
        GameManager.Instance.interactableObjects?.Add(this);
    }

    public void SetActive(bool active)
    {
        m_isActive = active;

        if (m_isActive)
        {
            m_interactionWidget.Show();
        }
        else
        {
            m_interactionWidget.Hide();
        }
    }

    public void Interact()
    {
        OnInteract?.Invoke(this, new InteractionEventArgs());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
