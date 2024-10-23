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
        GameManager.Instance.hiddenWalls.SetActive(true);
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
