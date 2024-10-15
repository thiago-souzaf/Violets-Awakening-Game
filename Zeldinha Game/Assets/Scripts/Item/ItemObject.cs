using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class ItemObject : ScriptableObject
{
    public ItemType itemType;
    public GameObject objectPrefab;
    public string displayName;
    public float objectScale = 1;
}