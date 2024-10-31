using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSoundOnEnable : MonoBehaviour
{
	[SerializeField] List<AudioClip> _clipList;
	private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        if (_clipList.Count > 0)
        {
            AudioClip randomClip = _clipList[Random.Range(0, _clipList.Count)];
            _audioSource.PlayOneShot(randomClip);
        }
    }
}
