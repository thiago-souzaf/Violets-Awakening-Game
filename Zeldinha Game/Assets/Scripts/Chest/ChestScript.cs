using UnityEngine;

[RequireComponent(typeof(Interaction))]
public class ChestScript : MonoBehaviour
{
    private Interaction interaction;

    private void Awake()
    {
        interaction = GetComponent<Interaction>();
    }
    private void Start()
    {
        interaction.OnInteract += OpenChest;
    }

    private void OpenChest(object sender, InteractionEventArgs args)
    {
        interaction.isAvailable = false;
    }
}