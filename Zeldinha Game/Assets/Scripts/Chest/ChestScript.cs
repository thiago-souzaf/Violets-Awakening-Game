using UnityEngine;

[RequireComponent(typeof(Interaction))]
public class ChestScript : MonoBehaviour
{
    private Interaction interaction;
    private Animator anim;

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
    }
}
