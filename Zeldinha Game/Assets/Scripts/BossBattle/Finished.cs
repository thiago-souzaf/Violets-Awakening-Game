using UnityEngine;

public class Finished : State
{
    public Finished() : base("Finished")
    {
    }

    public override void Enter()
    {
        base.Enter();
        // Stop Music
        GameManager gameManager = GameManager.Instance;
        gameManager.StartCoroutine(FadeAudioSource.StartFade(gameManager.bossBattleAudioSource, 0.0f, 1.0f));
    }

    public override void Exit()
    {
        base.Exit();
    }
}
