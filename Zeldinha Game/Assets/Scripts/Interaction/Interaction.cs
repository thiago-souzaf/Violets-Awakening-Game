using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject widgetPrefab;
    [SerializeField] private Vector3 widgetOffset;

    private GameObject widget;

    private void Start()
    {
        widget = Instantiate(widgetPrefab, transform.position + widgetOffset, Quaternion.identity);
        widget.transform.SetParent(this.transform);
    }
    private void OnEnable()
    {
        GameManager.Instance.interatbleObjects.Add(this);
    }

    private void OnDisable()
    {
        GameManager.Instance.interatbleObjects.Remove(this);
    }
}
