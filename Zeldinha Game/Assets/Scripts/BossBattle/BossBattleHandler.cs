using UnityEngine;

public class BossBattleHandler
{
	public StateMachine stateMachine;

	public Disabled disabledState;
	public Waiting waitingState;
	public Intro introState;
    public Battle battleState;
	public Finished finishedState;

	public BossBattleHandler()
    {
        // Create state machine and states
        stateMachine = new StateMachine();
        disabledState = new Disabled();
        waitingState = new Waiting();
        introState = new Intro();
        battleState = new Battle();
        finishedState = new Finished();
        stateMachine.ChangeState(disabledState);

        // Disable hidden parts
        GameManager.Instance.hiddenWalls.SetActive(false);

        // Register listeners    
        GlobalEvents.Instance.OnBossDoorOpen += (_, _) => stateMachine.ChangeState(waitingState);
        GlobalEvents.Instance.OnBossRoomEnter += (_, _) => stateMachine.ChangeState(introState);
        GlobalEvents.Instance.OnGameOver += (_, _) => stateMachine.ChangeState(finishedState);
        GlobalEvents.Instance.OnGameWon += (_, _) => stateMachine.ChangeState(finishedState);

    }

    public void Update()
    {
        stateMachine.Update();
    }

    public bool IsBossBattling()
    {
        return stateMachine.CurrentStateName == battleState.name;
    }

    public bool IsInCutscene()
    {
        return stateMachine.CurrentStateName == introState.name;
    }
}
