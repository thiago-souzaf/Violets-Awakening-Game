using UnityEngine;

public class Intro : State
{
    private readonly float duration = 3f;
    private float timeElapsed;
    public Intro() : base("Intro")
    {
    }

    public override void Enter()
    {
        base.Enter();

        // Enable hidden parts
        GameManager gameManager = GameManager.Instance;

        gameManager.hiddenWalls.SetActive(true);


        // Music
        gameManager.StartCoroutine(FadeAudioSource.StartFade(gameManager.musicAudioSource, 0.0f, 1.0f));
        float bossBattleTargetVolume = gameManager.bossBattleAudioSource.volume;
        gameManager.bossBattleAudioSource.volume = 0.0f;
        gameManager.StartCoroutine(FadeAudioSource.StartFade(gameManager.bossBattleAudioSource, bossBattleTargetVolume, 1.0f));
        gameManager.bossBattleAudioSource.Play();

        // UI
        gameManager.gameplayUI.ToggleBossBar(true);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= duration)
        {
            var bossBattleHandler = GameManager.Instance.bossBattleHandler;
            bossBattleHandler.stateMachine.ChangeState(bossBattleHandler.battleState);
        }
    }
}
