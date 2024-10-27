using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundOnEnable : MonoBehaviour
{
	[SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];
        audioSource.PlayOneShot(randomClip);
    }
}
