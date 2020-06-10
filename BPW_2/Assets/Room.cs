using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int roomID = 0;
    public List<int> doorPositions = new List<int>();
    public bool isBossRoom = false;
    public bool isTreasureRoom = false;
    public bool isPuzzleRoom = false;
    public bool isResourceRoom = false;

    public bool hasUnconnectedDoors = false;
    public int amountOfUnconnectedDoors = 0;

    public int roomPosition = 0;
}
