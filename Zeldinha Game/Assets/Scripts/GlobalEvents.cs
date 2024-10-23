using System;
using UnityEngine;

public class GlobalEvents : Singleton<GlobalEvents>
{
	public event EventHandler<BossDoorOpenEventArgs> OnBossDoorOpen;
    public event EventHandler<BossRoomEnterEventArgs> OnBossRoomEnter;
    public event EventHandler<GameOverEventArgs> OnGameOver;
    public event EventHandler<GameWonEventArgs> OnGameWon;

    public void BossDoorOpen()
    {
        OnBossDoorOpen?.Invoke(this, new BossDoorOpenEventArgs());
        Debug.Log("BossDoorOpen invoked");
    }
    public void BossRoomEnter()
    {
        OnBossRoomEnter?.Invoke(this, new BossRoomEnterEventArgs());

        Debug.Log("BossRoomEnter invoked");
    }

    public void GameOver()
    {
        OnGameOver?.Invoke(this, new GameOverEventArgs());
        Debug.Log("GameOver invoked");
    }

    public void GameWon()
    {
        OnGameWon?.Invoke(this, new GameWonEventArgs());
        Debug.Log("GameWon invoked");
    }
}
