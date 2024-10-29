using UnityEngine;

public class Disabled : State
{
    public Disabled() : base("Disabled")
    {
    }

    public override void Enter()
    {
        base.Enter();

        GameManager gameManager = GameManager.Instance;
        gameManager.boss.SetActive(false);

        // UI

        gameManager.gameplayUI.ToggleBossBar(false);

    }

    public override void Exit()
    {
        base.Exit();
        GameManager.Instance.boss.SetActive(true);

    }
}
