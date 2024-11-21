using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _paths;
    [SerializeField] private Transform _player;
    
    private void Update()
    {
        GetInput();
    }
    
    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TeleportPlayer(1); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TeleportPlayer(2); 
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TeleportPlayer(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(_paths.Count < 4) return;
            TeleportPlayer(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if(_paths.Count < 5) return;
            TeleportPlayer(5);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }
    
    private void TeleportPlayer(int position)
    {
        _player.position = _paths[position - 1].position;
    }
}
