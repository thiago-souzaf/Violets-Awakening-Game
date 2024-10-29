using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    public bool IsGameOver { get; private set; }

    [Header("Physics")]
    public LayerMask groundLayer;
    public GameObject player;

    // Inventory
    [Header("Inventory")]
    public int keys;
    public bool hasBossKey;

    public List<Interaction> interactableObjects;

    // Boss
    [Header("Boss")]
    public GameObject boss;
    public GameObject hiddenWalls;
    public BossBattleHandler bossBattleHandler;

    // Audio
    [Header("Audio")]
    public AudioSource musicAudioSource;
    public AudioSource ambienceAudioSource;
    public AudioSource bossBattleAudioSource;

    // UI
    [Header("UI")]
    public GameplayUI gameplayUI;
    [SerializeField] private float timeToEnableGameplayUI;

    private void Start()
    {
        bossBattleHandler = new();


        // Play audio sources
        float musicTargetVolume = musicAudioSource.volume;
        musicAudioSource.volume = 0.0f;
        StartCoroutine(FadeAudioSource.StartFade(musicAudioSource, musicTargetVolume, 1.0f));
        musicAudioSource.Play();

        float ambienceTargetVolume = ambienceAudioSource.volume;
        ambienceAudioSource.volume = 0.0f;
        StartCoroutine(FadeAudioSource.StartFade(ambienceAudioSource, ambienceTargetVolume, 1.0f));
        ambienceAudioSource.Play();


        gameplayUI.gameObject.SetActive(false);
        Invoke(nameof(EnableGameplayUI), timeToEnableGameplayUI);

        GlobalEvents.Instance.OnGameOver += (_, _) => { IsGameOver = true; };
    }

    private void Update()
    {
        bossBattleHandler.Update();
    }

    private void EnableGameplayUI()
    {
        gameplayUI.gameObject.SetActive(true);
    }
}
