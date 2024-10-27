using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interaction))]
public class ChestScript : MonoBehaviour
{
    private Interaction interaction;
    private Animator anim;

    public Transform itemHolder;

    public ItemObject itemInside;

    public UnityEvent OnOpen;

    private void Awake()
    {
        interaction = GetComponent<Interaction>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        interaction.OnInteract += OpenChest;
        interaction.SetActionText("Open Chest");
    }

    private void OpenChest(object sender, InteractionEventArgs args)
    {
        interaction.isAvailable = false;
        anim.SetTrigger("tOpen");
        GameObject objectInside = Instantiate(itemInside.objectPrefab, itemHolder.position, itemInside.objectPrefab.transform.rotation);
        objectInside.transform.SetParent(itemHolder);
        objectInside.transform.localScale = itemInside.objectScale * Vector3.one;
        Debug.Log("Player opened a chest containing a " + itemInside.displayName);

        ItemType itemType = itemInside.itemType;
        if (itemType == ItemType.Key)
        {
            GameManager.Instance.keys++;
        }
        else if (itemType == ItemType.BossKey)
        {
            GameManager.Instance.hasBossKey = true;
        }
        else if (itemType == ItemType.HealthPotion)
        {
            GameManager.Instance.player.GetComponent<Life>().Heal();
        }

        OnOpen?.Invoke();
    }
}
