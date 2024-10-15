using UnityEngine;

[RequireComponent(typeof(Interaction))]
public class DoorScript : MonoBehaviour
{
	private Interaction m_interaction;
    private bool m_isOpen = false;
    private Animator anim;

    public ItemObject requiredKey;

    private void Awake()
    {
        m_interaction = GetComponent<Interaction>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        m_interaction.OnInteract += OnInteract;
        m_interaction.SetActionText("Open Door");

    }

    private void Update()
    {
        if (!m_isOpen)
        {
            bool hasKey = false;

            if (requiredKey == null)
            {
                hasKey = true;
            }
            else if (requiredKey.itemType == ItemType.Key)
            {
                hasKey = GameManager.Instance.keys > 0;
            }
            else if (requiredKey.itemType == ItemType.BossKey)
            {
                hasKey = GameManager.Instance.hasBossKey;
            }

            m_interaction.isAvailable = hasKey;
        }
    }

    private void OnInteract(object sender, InteractionEventArgs args)
    {

        if (!m_isOpen)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Debug.Log("Door Opened");
        m_isOpen = true;
        anim.SetTrigger("tOpen");

        if (requiredKey.itemType == ItemType.Key)
        {
            GameManager.Instance.keys--;
        }
        else if (requiredKey.itemType == ItemType.BossKey)
        {
            GameManager.Instance.hasBossKey = false;
        }

        // Disable interaction
        m_interaction.isAvailable = false;
    }

    private void CloseDoor()
    {
        Debug.Log("Door Closed");
        m_isOpen = false;
        anim.SetTrigger("tClose");
    }
}
