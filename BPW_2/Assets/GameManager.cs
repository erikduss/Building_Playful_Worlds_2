using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject Player;

    // Start is called before the first frame update
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void LevelGenerationComplete(Vector3 spawnRoomPos)
    {
        Vector3 spawnRoomPosition = spawnRoomPos;
        Player.transform.position = new Vector3(spawnRoomPosition.x + 10, spawnRoomPosition.y - 8, spawnRoomPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
