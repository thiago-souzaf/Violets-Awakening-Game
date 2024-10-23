using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

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

    private void Start()
    {
        bossBattleHandler = new();
    }

    private void Update()
    {
        bossBattleHandler.Update();
    }
}
