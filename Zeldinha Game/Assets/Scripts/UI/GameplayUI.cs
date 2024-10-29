using System.Collections.Generic;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
	// Health bars
	[Header("Player")]
	public HealthBar playerHealthBar;

    [Header("Boss")]
    public HealthBar bossHealthBar;
	[SerializeField] private GameObject bossLifeContainer;
	private bool isBossLifeVisible = false;

    // Items
    [Header("Items")]
	[SerializeField] private GameObject objectsContainer;
	[SerializeField] private GameObject normalKeyTemplate;
	[SerializeField] private GameObject bossKeyTemplate;
	private List<ObjectEntry> entries = new List<ObjectEntry>();

	public void ToggleBossBar(bool enable)
	{
		bossLifeContainer.SetActive(enable);
        isBossLifeVisible = enable;
	}

	public void AddItem(ItemType type)
	{
		if (type != ItemType.Key && type != ItemType.BossKey)
        {
            Debug.LogError("Invalid item type");
            return;
        }

        GameObject template = type == ItemType.Key ? normalKeyTemplate : bossKeyTemplate;
        GameObject item = Instantiate(template, objectsContainer.transform);
		item.SetActive(true);
		// Create entry
		var entry = new ObjectEntry
		{
			type = type,
			widget = item
		};

        entries.Add(entry);
    }

	public void RemoveItem(ItemType type)
	{
		foreach (ObjectEntry entry in entries)
		{
            if (entry.type == type)
            {
                Destroy(entry.widget);
                entries.Remove(entry);
                break;
            }
        }
	}
	private struct ObjectEntry
	{
		public ItemType type;
        public GameObject widget;
    }


}
