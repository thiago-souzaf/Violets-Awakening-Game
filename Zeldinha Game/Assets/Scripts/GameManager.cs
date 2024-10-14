using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Physics")]
    public LayerMask groundLayer;
    public GameObject player;

    public List<Interaction> interactableObjects;
    private void Start()
    {
    }

    private void Update()
    {
		
    } 

    
}
