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
    private void Start()
    {
    }

    private void Update()
    {
		
    } 

    
}
