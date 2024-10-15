using UnityEngine;

[RequireComponent(typeof(Interaction))]
public class ChestScript : MonoBehaviour
{
    private Interaction interaction;
    private Animator anim;

    public Transform itemHolder;

    public ItemObject so_itemInside;

    private void Awake()
    {
        interaction = GetComponent<Interaction>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        interaction.OnInteract += OpenChest;
        
    }

    private void OpenChest(object sender, InteractionEventArgs args)
    {
        interaction.isAvailable = false;
        anim.SetTrigger("tOpen");
        GameObject objectInside = Instantiate(so_itemInside.objectPrefab, itemHolder.position, so_itemInside.objectPrefab.transform.rotation);
        objectInside.transform.SetParent(itemHolder);
        objectInside.transform.localScale = so_itemInside.objectScale * Vector3.one;
        Debug.Log("Player opened a chest containing a " + so_itemInside.displayName);
    }
}
