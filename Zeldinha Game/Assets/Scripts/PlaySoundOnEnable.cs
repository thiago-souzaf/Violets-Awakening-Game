using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnEnable : MonoBehaviour
{
	[SerializeField] private AudioClip audioClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        audioSource.PlayOneShot(audioClip);
    }

    public void SetAudioClip(AudioClip clip)
    {
        if (audioClip != clip)
        {
            audioClip = clip;
        }
    }
}
