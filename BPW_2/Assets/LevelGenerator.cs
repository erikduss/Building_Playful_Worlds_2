using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> topWallPrefabs = new List<GameObject>();
    public List<GameObject> bottomWallPrefabs = new List<GameObject>();
    public List<GameObject> rightWallPrefabs = new List<GameObject>();
    public List<GameObject> cornerWallPrefabs = new List<GameObject>();

    public List<GameObject> leftWallPrefabs = new List<GameObject>();

    public List<GameObject> doorwayPrefabs = new List<GameObject>();


    public List<GameObject> floorTiles = new List<GameObject>();
    
    private float tileSize = 1.28f;
    private int roomLength = 13;
    private int wallLength = 4;

    private int levelWidth = 150;
    private int levelHeight = 150;

    private List<float> tilePositionsX = new List<float>();
    private List<float> tilePositionsY = new List<float>();

    private List<float> roomPositionsX = new List<float>();
    private List<float> roomPositionsY = new List<float>();

    private List<Vector3> roomPositions = new List<Vector3>();

    private float roomLenth = 19.2f;
    private float roomHeight = 16.64f;

    public GameObject levelParentObject;

    private int amountOfGeneratedRooms = 0;

    private int lastRoomPosition = 0;
    private int roomsSpawned = 0;

    private List<GameObject> generatedRooms = new List<GameObject>();
    private List<Room> roomsList = new List<Room>();

    private List<int> requiredAdditionalDoorways = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        levelParentObject = GameObject.FindGameObjectWithTag("Level");

        //tile positions
        float tilePosX = 0;
        for (int i = 0; i < levelWidth; i++)
        {
            tilePositionsX.Add(tilePosX);
            tilePosX += tileSize;
        }
        float tilePosY = 0;
        for (int i = 0; i < levelHeight; i++)
        {
            tilePositionsY.Add(tilePosY);
            tilePosY -= tileSize;
        }

        //Room positions
        float roomPosX = 0;
        for (int i = 0; i < 10; i++)
        {
            roomPositionsX.Add(roomPosX);
            roomPosX += roomLenth;
        }
        float roomPosY = 0;
        for (int i = 0; i < 10; i++)
        {
            roomPositionsY.Add(roomPosY);
            roomPosY -= roomHeight;
        }
        for (int i = 0; i < 6; i++)
        {
            for (int t = 0; t < 6; t++)
            {
                Vector3 roomPos = new Vector3(roomPositionsX[t], roomPositionsY[i], 0);
                roomPositions.Add(roomPos);
            }
        }

        GenerateDungeonFloor(3);
        GenerateRooms(36); //36!!!
    }

    private void GenerateRooms(int amount)
    {
        int currentRow = 0;
        int currentRoom = 0;

        int spawnRoom = 0;

        int bossRoomDoorwayPos = Random.Range(1, 9);

        CreateBossRoom(1, bossRoomDoorwayPos);
        generatedRooms[0].transform.position = roomPositions[14];
        roomsList[0].roomPosition = 14;

        CreateBossRoom(2, bossRoomDoorwayPos);
        generatedRooms[1].transform.position = roomPositions[15];
        roomsList[1].roomPosition = 15;

        CreateBossRoom(3, bossRoomDoorwayPos);
        generatedRooms[2].transform.position = roomPositions[20];
        roomsList[2].roomPosition = 20;

        CreateBossRoom(4, bossRoomDoorwayPos);
        generatedRooms[3].transform.position = roomPositions[21];
        roomsList[3].roomPosition = 21;

        switch (bossRoomDoorwayPos)
        {
            case 1:
                spawnRoom = 8;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 2:
                spawnRoom = 9;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 3:
                spawnRoom = 16;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 4:
                spawnRoom = 22;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 5:
                spawnRoom = 26;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 6:
                spawnRoom = 27;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 7:
                spawnRoom = 19;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 8:
                spawnRoom = 13;
                GenerateBasicRoom(bossRoomDoorwayPos);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
        }

        //Debug.Log(lastRoomPosition);

        for (int i = 5; i < amount; i++)
        {
            //Vector3 roomPosition = new Vector3(roomPositionsX[currentRoom], roomPositionsY[currentRow], 0);

            List<bool> possibleSpawns = possibleRooms();
            int impossibleSpawns = 0;
            List<int> spawnDirections = new List<int>();

            foreach (bool possibility in possibleSpawns)
            {
                if (!possibility)
                {
                    impossibleSpawns++;
                }
            }

            //Debug.Log("Spawns: " + possibleSpawns[0] + possibleSpawns[1] + possibleSpawns[2] + possibleSpawns[3]);
            
            if (possibleSpawns[0] == true && roomsList[i - 1].doorPositions.Contains(1))//up
            {
                spawnDirections.Add(1);
            }
            if (possibleSpawns[1] == true && roomsList[i - 1].doorPositions.Contains(3))//down
            {
                spawnDirections.Add(3);
            }
            if (possibleSpawns[2] == true && roomsList[i - 1].doorPositions.Contains(2))//right
            {
                spawnDirections.Add(2);
            }
            if (possibleSpawns[3] == true && roomsList[i-1].doorPositions.Contains(4))//left
            {
                spawnDirections.Add(4);
            }

            int roomPos = 0;
            int roomSpawnDirection = 0;

            if (spawnDirections.Count > 0)
            {
                roomSpawnDirection = spawnDirections[Random.Range(0, spawnDirections.Count)];
            }

            if(roomSpawnDirection == 1)
            {
                roomPos = lastRoomPosition - 6;
                GenerateBasicRoom(1);
            }
            else if(roomSpawnDirection == 2)
            {
                roomPos = lastRoomPosition + 1;
                GenerateBasicRoom(3);
            }
            else if (roomSpawnDirection == 3)
            {
                roomPos = lastRoomPosition + 6;
                GenerateBasicRoom(5);
            }
            else if (roomSpawnDirection == 4)
            {
                roomPos = lastRoomPosition - 1;
                GenerateBasicRoom(7);
            }
            else
            {
                roomPos = 0;
                GenerateBasicRoom(0);
                /*
                if(CheckForRoomsWithOpenDoorways() > 0)
                {
                    lastRoomPosition = GetAvailableRoom();
                }

                possibleSpawns = possibleRooms();
                impossibleSpawns = 0;
                spawnDirections = new List<int>();

                foreach (bool possibility in possibleSpawns)
                {
                    if (!possibility)
                    {
                        impossibleSpawns++;
                    }
                }

                if (possibleSpawns[0] && roomsList[i-1].doorPositions.Contains(1))//up
                {
                    spawnDirections.Add(1);
                }
                if (possibleSpawns[1] && roomsList[i - 1].doorPositions.Contains(3))//down
                {
                    spawnDirections.Add(3);
                }
                if (possibleSpawns[2] && roomsList[i - 1].doorPositions.Contains(2))//right
                {
                    spawnDirections.Add(2);
                }
                if (possibleSpawns[3] && roomsList[i - 1].doorPositions.Contains(4))//left
                {
                    spawnDirections.Add(4);
                }
                roomPos = 0;
                roomSpawnDirection = 0;

                if (spawnDirections.Count > 0)
                {
                    roomSpawnDirection = spawnDirections[Random.Range(0, spawnDirections.Count)];
                }

                if (roomSpawnDirection == 1)
                {
                    roomPos = lastRoomPosition - 6;
                    GenerateBasicRoom(1);
                }
                else if (roomSpawnDirection == 2)
                {
                    roomPos = lastRoomPosition + 1;
                    GenerateBasicRoom(3);
                }
                else if (roomSpawnDirection == 3)
                {
                    roomPos = lastRoomPosition + 6;
                    GenerateBasicRoom(5);
                }
                else if (roomSpawnDirection == 4)
                {
                    roomPos = lastRoomPosition - 1;
                    GenerateBasicRoom(7);
                }
                else
                {
                    //Debug.Log("CANT SPAWN ROOM_ Doors last: " + roomsList[lastRoomPosition].doorPositions.Count + "Position: " + roomsList[lastRoomPosition].roomPosition);
                    roomPos = 0;
                    GenerateBasicRoom(0);
                }*/
            }

            generatedRooms[i].transform.position = roomPositions[roomPos];

            roomsList[i].roomPosition = roomPos;

            lastRoomPosition = roomPos;

            roomsSpawned++;

            currentRoom++;
            if (currentRoom == 6)
            {
                currentRoom = 0;
                currentRow++;
            }
        }
    }

    public void GenerateDungeonFloor(int randomness)
    {
        List<List<GameObject>> floorTilesList = new List<List<GameObject>>();
        List<GameObject> floorTiledRow = new List<GameObject>();
        int tileNumber = 0;
        int rowNumber = 0;

        for (int t = 0; t < levelHeight; t++)
        {
            floorTiledRow = new List<GameObject>();
            for (int i = 0; i < levelWidth; i++)
            {
                int floorRandomness = Random.Range(0, randomness);
                if (floorRandomness == 0)
                {
                    floorTiledRow.Add(floorTiles[1]);
                }
                else
                {
                    floorTiledRow.Add(floorTiles[0]);
                }
            }
            floorTilesList.Add(floorTiledRow);
        }

        GameObject Floor_Tiles = new GameObject();
        Floor_Tiles.transform.parent = levelParentObject.transform;
        Floor_Tiles.name = "Floor_Tiles";

        foreach (List<GameObject> tileList in floorTilesList)
        {
            tileNumber = 0;
            foreach (GameObject tile in tileList)
            {
                Vector3 tilePos = new Vector3(tilePositionsX[tileNumber], tilePositionsY[rowNumber], 0);
                var createdTile = Instantiate(tile, tilePos, Quaternion.identity);
                createdTile.transform.parent = Floor_Tiles.transform;
                tileNumber++;
            }
            rowNumber++;
        }
    }

    private List<GameObject> createHorizontalWall(string wallPlacement, List<GameObject> wallPrefabToAdd, int doorwayPlacement, bool bossRoom, int bossPart, List<int> doorSpawns)
    {
        List<GameObject> returnList = new List<GameObject>();

        if(bossPart == 1 || bossPart == 3 || !bossRoom)
        {
            if (wallPlacement == "Top")
            {
                returnList.Add(cornerWallPrefabs[2]); //add corner top left
            }
            else if (wallPlacement == "Bottom")
            {
                returnList.Add(cornerWallPrefabs[0]); //add corner bottom left
            }
        }
        else
        {
            returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
            returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
        }
        

        //add first bit of walls
        for (int i = 0; i < wallLength; i++)
        {
            returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
        }

        //Add doorway
        if (!bossRoom)
        {
            if (doorwayPlacement == 1 && wallPlacement == "Top" || doorSpawns.Contains(1) && wallPlacement == "Top")
            {
                returnList.Add(doorwayPrefabs[2]);
            }
            else if (doorwayPlacement == 3 && wallPlacement == "Bottom" || doorSpawns.Contains(3) && wallPlacement == "Bottom")
            {
                returnList.Add(doorwayPrefabs[0]);
            }
            else //add replacement walls
            {
                for (int i = 0; i < 3; i++)
                {
                    returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
                }
            }
        }
        else
        {
            if (doorwayPlacement == 1 && wallPlacement == "Top" && bossPart == 1 || doorwayPlacement == 2 && wallPlacement == "Top" && bossPart == 2)
            {
                returnList.Add(doorwayPrefabs[2]);
            }
            else if (doorwayPlacement == 5 && wallPlacement == "Bottom" && bossPart == 3 || doorwayPlacement == 6 && wallPlacement == "Bottom" && bossPart == 4)
            {
                returnList.Add(doorwayPrefabs[0]);
            }
            else //add replacement walls
            {
                for (int i = 0; i < 3; i++)
                {
                    returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
                }
            }
        }
        

        //add second bit of walls
        for (int i = 0; i < wallLength; i++)
        {
            returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
        }

        if (bossPart == 2 || bossPart == 4 || !bossRoom)
        {
            if (wallPlacement == "Top")
            {
                returnList.Add(cornerWallPrefabs[3]); //add corner
            }
            else if (wallPlacement == "Bottom")
            {
                returnList.Add(cornerWallPrefabs[1]); //add corner
            }
        }
        else
        {
            returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
            returnList.Add(wallPrefabToAdd[Random.Range(0, wallPrefabToAdd.Count - 1)]);
        }

        return returnList;
    }

    private List<GameObject> createSideWallBossRow(bool spawnDoor, int doorwayPlacement, bool spawnedDoor, int bossPart)
    {
        List<GameObject> returnList = new List<GameObject>();

        if (!spawnDoor)
        {
            if (!spawnedDoor)
            {
                if (bossPart == 1 || bossPart == 3)
                {
                    returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);
                }
                else if (bossPart == 2 || bossPart == 4)
                {
                    returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
                }           
            }
            else if (spawnedDoor && doorwayPlacement == 4)
            {
                if (bossPart == 2 || bossPart == 4)
                {
                    returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
                }
            }
            else if (spawnedDoor && doorwayPlacement == 2)
            {
                if(bossPart == 1 || bossPart == 3)
                {
                    returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);
                }
            }
        }
        else
        {
            if (doorwayPlacement == 7 && bossPart == 3)
            {
                    returnList.Add(doorwayPrefabs[1]);
            }
            else if(doorwayPlacement == 8 && bossPart == 1)
            {
                    returnList.Add(doorwayPrefabs[1]);
            }
            else
            {
                if (bossPart == 1 || bossPart == 3)
                {
                    returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);
                }
            }

            if (doorwayPlacement == 3 && bossPart == 2)
            {
                 returnList.Add(doorwayPrefabs[3]);
            }
            else if (doorwayPlacement == 4 && bossPart == 4)
            {
                returnList.Add(doorwayPrefabs[3]);
            }
            else
            {
                if (bossPart == 2 || bossPart == 4)
                {
                    returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
                }
            }
        }
        return returnList;
    }

    private List<GameObject> createSideWallsRow(bool spawnDoor, int doorwayPlacement, bool spawnedDoor, List<int> doorSpawns)
    {
        List<GameObject> returnList = new List<GameObject>();

        if (!spawnDoor)
        {
            if (!spawnedDoor)
            {
                if(doorwayPlacement != 4 && doorwayPlacement != 2 && !doorSpawns.Contains(4) && !doorSpawns.Contains(2))
                {
                    returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);

                    returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
                }
                else
                {
                    returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);

                    returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
                }
            }
            else
            {
                if (doorwayPlacement != 4 && !doorSpawns.Contains(4))
                {
                    returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);
                }
                if (doorwayPlacement != 2 && !doorSpawns.Contains(2))
                {
                    returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
                }
            }
        }
        else
        {
            if (doorwayPlacement == 4 || doorSpawns.Contains(4))
            {
                returnList.Add(doorwayPrefabs[1]);
            }
            else if(doorwayPlacement != 4 && !doorSpawns.Contains(4))
            {
                returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);
            }

            if (doorwayPlacement == 2 || doorSpawns.Contains(2))
            {
                returnList.Add(doorwayPrefabs[3]);
            }
            else if(doorwayPlacement != 2 && !doorSpawns.Contains(2))
            {
                returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
            }
        }
        return returnList;
    }

    public void CreateBossRoom(int part, int doorwaySide) 
    {
        Room generatedRoom = new Room();
        generatedRoom.roomID = amountOfGeneratedRooms;
        roomsList.Add(generatedRoom);

        List<List<GameObject>> roomTiles = new List<List<GameObject>>();
        bool spawnedDoor = false;

        #region top Wall spawning
        if (part == 1 || part == 2)
        {
            roomTiles.Add(createHorizontalWall("Top", topWallPrefabs, doorwaySide, true, part, null));
        }
        else
        {
            roomTiles.Add(createSideWallBossRow(false, doorwaySide, false, part)); //first row cant have a door
        }
        #endregion

        int tileNumber = 0;

        //add more rows before the door
        for (int i = 0; i < 3; i++)
        {
            roomTiles.Add(createSideWallBossRow(false, doorwaySide, false, part));
        }

        //add a door or fill the walls up
        for (int i = 0; i < 3; i++)
        {
            if (doorwaySide == 3 && part == 2 || doorwaySide == 4 && part == 4|| doorwaySide == 7 && part == 3|| doorwaySide == 8 && part == 1)
            {
                if (!spawnedDoor)
                {
                    roomTiles.Add(createSideWallBossRow(true, doorwaySide, false, part));
                    spawnedDoor = true;
                }
                else
                {
                    roomTiles.Add(createSideWallBossRow(false, doorwaySide, true, part));
                }
            }
            else
            {
                roomTiles.Add(createSideWallBossRow(false, doorwaySide, false, part));
            }
        }

        for (int i = 0; i < 3; i++)
        {
            roomTiles.Add(createSideWallBossRow(false, doorwaySide, false, part));
        }

        if (part == 3 || part == 4)
        {
            roomTiles.Add(createHorizontalWall("Bottom", bottomWallPrefabs, doorwaySide, true, part, null));
        }
        else
        {
            roomTiles.Add(createSideWallBossRow(false, doorwaySide, false, part));
            roomTiles.Add(createSideWallBossRow(false, doorwaySide, false, part));
            roomTiles.Add(createSideWallBossRow(false, doorwaySide, false, part));
        }
        

        int rowNumber = 0;

        GameObject Room_gameobject = new GameObject();
        Room_gameobject.transform.parent = levelParentObject.transform;
        Room_gameobject.name = "Room_" + amountOfGeneratedRooms;

            for (int i = 0; i < 14; i++)
            {
                for (int t = 0; t < 15; t++)
                {
                    int tileRandomNumber = Random.Range(0, 5);
                    int tileVariant = 0;
                    if (tileRandomNumber == 0)
                    {
                        tileVariant = 3;
                    }
                    else
                    {
                        tileVariant = 2;
                    }

                    Vector3 tilePos = new Vector3(tilePositionsX[t], tilePositionsY[i], 0);
                    var roomTile = Instantiate(floorTiles[tileVariant], tilePos, Quaternion.identity);
                    roomTile.transform.parent = Room_gameobject.transform;
                }
            }

        foreach (List<GameObject> tileList in roomTiles)
        {
            if (rowNumber == 0 || rowNumber == 10)
            {
                tileNumber = 0;
            }
            else
            {
                tileNumber = 0;
            }

            foreach (GameObject tile in tileList)
            {

                if(tileList.Count == 1)
                {
                    if(part == 2 || part == 4)
                    {
                        tileNumber += 13;
                    }
                    else
                    {
                        tileNumber = 0;
                    }
                }

                Vector3 tilePos = new Vector3(tilePositionsX[tileNumber], tilePositionsY[rowNumber], 0);
                var roomTile = Instantiate(tile, tilePos, Quaternion.identity);
                roomTile.transform.parent = Room_gameobject.transform;

                if (tileNumber == 0) //corner top and bottom
                {
                    if (rowNumber == 0 || rowNumber == 10)
                    {
                        if(part == 2 || part == 4)
                        {
                            tileNumber++;
                        }
                        else
                        {
                            tileNumber += 2;
                        }
                    }
                }
                else if (tileNumber == 6 && doorwaySide == 1 && rowNumber == 0 && part == 1) //top door
                {
                    tileNumber += 3;
                }
                else if (tileNumber == 6 && doorwaySide == 2 && rowNumber == 0 && part == 2) //top door
                {
                    tileNumber += 3;
                }
                else if (tileNumber == 6 && doorwaySide == 5 && rowNumber == 10 && part == 3) // bottom door
                {
                    tileNumber += 3;
                }
                else if (tileNumber == 6 && doorwaySide == 6 && rowNumber == 10 && part == 4) // bottom door
                {
                    tileNumber += 3;
                }
                else if (rowNumber == 0 || rowNumber == 10)
                {
                    tileNumber++;
                }
                else
                {
                    tileNumber += 13;
                }
            }
            rowNumber++;
        }
        amountOfGeneratedRooms++;
        generatedRooms.Add(Room_gameobject);
    }

    private List<bool> possibleRooms()
    {
        List<bool> possibleSpawns = new List<bool>();
        int roomPos = 0;
        bool isOccupied = false;

        bool canSpawnUp = false;
        bool canSpawnDown = false;
        bool canSpawnRight = false;
        bool canSpawnLeft = false;

        roomPos = lastRoomPosition - 6; //up

        if (lastRoomPosition == 0 || lastRoomPosition == 1 || lastRoomPosition == 2 || lastRoomPosition == 3 || lastRoomPosition == 4 || lastRoomPosition == 5)
        {
            canSpawnUp = false;
        }
        else if (roomPos >= 0)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    isOccupied = true;
                    canSpawnUp = false;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnUp = true;
            }
        }
        else
        {
            canSpawnUp = false;
        }

        isOccupied = false;
        roomPos = lastRoomPosition + 6; //down

        if (lastRoomPosition == 30 || lastRoomPosition == 31 || lastRoomPosition == 32 || lastRoomPosition == 33 || lastRoomPosition == 34 || lastRoomPosition == 35)
        {
            canSpawnDown = false;
        }
        else if (roomPos < 36)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    isOccupied = true;
                    canSpawnDown = false;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnDown = true;
            }
        }
        else
        {
            canSpawnDown = false;
        }

        isOccupied = false;
        roomPos = lastRoomPosition + 1; //right

        if (lastRoomPosition == 5 || lastRoomPosition == 11 || lastRoomPosition == 17 || lastRoomPosition == 23 || lastRoomPosition == 29 || lastRoomPosition == 35)
        {
            canSpawnRight = false;
        }
        else if (roomPos < 36)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    canSpawnRight = false;
                    isOccupied = true;
                    break;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnRight = true;
            }
        }
        else
        {
            canSpawnRight = false;
        }

        isOccupied = false;
        roomPos = lastRoomPosition - 1; //left

        if (lastRoomPosition == 0 || lastRoomPosition == 6 || lastRoomPosition == 12 || lastRoomPosition == 18 || lastRoomPosition == 24 || lastRoomPosition == 30 || lastRoomPosition == 36)
        {
            canSpawnLeft = false;
        }
        else if (roomPos >= 0)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    canSpawnLeft = false;
                    isOccupied = true;
                    break;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnLeft = true;
            }
        }
        else
        {
            canSpawnLeft = false;
        }

        possibleSpawns.Add(canSpawnUp);
        possibleSpawns.Add(canSpawnDown);
        possibleSpawns.Add(canSpawnRight);
        possibleSpawns.Add(canSpawnLeft);

        return possibleSpawns;
    }

    private List<bool> possibleDoorways()
    {
        List<bool> possibleSpawns = new List<bool>();
        int roomPos = 0;
        bool isOccupied = false;

        bool canSpawnUp = false;
        bool canSpawnDown = false;
        bool canSpawnRight = false;
        bool canSpawnLeft = false;

        roomPos = lastRoomPosition - 6; //up

        if (roomPos == 0 || roomPos == 1 || roomPos == 2 || roomPos == 3 || roomPos == 4 || roomPos == 5)
        {
            canSpawnUp = false;
        }
        else if (roomPos > 0)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    isOccupied = true;
                    if (checkRoom.hasUnconnectedDoors)
                    {
                        if (checkRoom.doorPositions.Contains(3))
                        {
                            requiredAdditionalDoorways.Add(1);
                            canSpawnUp = false;
                            checkRoom.amountOfUnconnectedDoors--;
                            if(checkRoom.amountOfUnconnectedDoors <= 0)
                            {
                                checkRoom.hasUnconnectedDoors = false;
                            }
                        }
                        else
                        {
                            canSpawnUp = false;
                        }
                    }
                    else
                    {
                        canSpawnUp = false;
                    }
                    break;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnUp = true;
            }
        }
        else
        {
            canSpawnUp = false;
        }

        isOccupied = false;
        roomPos = lastRoomPosition + 6; //down

        if (roomPos == 30 || roomPos == 31 || roomPos == 32 || roomPos == 33 || roomPos == 34 || roomPos == 35)
        {
            canSpawnDown = false;
        }
        else if (roomPos < 36)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    isOccupied = true;
                    if (checkRoom.hasUnconnectedDoors)
                    {
                        if (checkRoom.doorPositions.Contains(1))
                        {
                            requiredAdditionalDoorways.Add(3);
                            canSpawnDown = false;
                            checkRoom.amountOfUnconnectedDoors--;
                            if (checkRoom.amountOfUnconnectedDoors <= 0)
                            {
                                checkRoom.hasUnconnectedDoors = false;
                            }
                        }
                        else
                        {
                            canSpawnDown = false;
                        }
                    }
                    else
                    {
                        canSpawnDown = false;
                    }
                    break;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnDown = true;
            }
        }
        else
        {
            canSpawnDown = false;
        }

        isOccupied = false;
        roomPos = lastRoomPosition + 1; //right

        if(roomPos == 5 || roomPos == 11 || roomPos == 17 || roomPos == 23 || roomPos == 29 || roomPos == 35)
        {
            canSpawnRight = false;
        }
        else if (roomPos < 36)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    isOccupied = true;
                    if (checkRoom.hasUnconnectedDoors)
                    {
                        if (checkRoom.doorPositions.Contains(2))
                        {
                            requiredAdditionalDoorways.Add(4);
                            canSpawnRight = false;
                            checkRoom.amountOfUnconnectedDoors--;
                            if (checkRoom.amountOfUnconnectedDoors <= 0)
                            {
                                checkRoom.hasUnconnectedDoors = false;
                            }
                        }
                        else
                        {
                            canSpawnRight = false;
                        }
                    }
                    canSpawnRight = false;
                    break;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnRight = true;
            }
        }
        else
        {
            canSpawnRight = false;
        }

        isOccupied = false;
        roomPos = lastRoomPosition - 1; //left

        if (roomPos == 0 || roomPos == 6 || roomPos == 12 || roomPos == 18 || roomPos == 24 || roomPos == 30 || roomPos == 36)
        {
            canSpawnLeft = false;
        }
        else if (roomPos > 0)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPos)
                {
                    isOccupied = true;
                    if (checkRoom.hasUnconnectedDoors)
                    {
                        if (checkRoom.doorPositions.Contains(4))
                        {
                            requiredAdditionalDoorways.Add(2);
                            canSpawnLeft = false;
                            checkRoom.amountOfUnconnectedDoors--;
                            if (checkRoom.amountOfUnconnectedDoors <= 0)
                            {
                                checkRoom.hasUnconnectedDoors = false;
                            }
                        }
                        else
                        {
                            canSpawnLeft = false;
                        }
                    }
                    canSpawnLeft = false;
                    break;
                }
            }
            if (!isOccupied) // if it didnt find a room on the position
            {
                canSpawnLeft = true;
            }
        }
        else
        {
            canSpawnLeft = false;
        }

        possibleSpawns.Add(canSpawnUp);
        possibleSpawns.Add(canSpawnDown);
        possibleSpawns.Add(canSpawnRight);
        possibleSpawns.Add(canSpawnLeft);

        return possibleSpawns;
    }

    private int CheckForRoomsWithOpenDoorways()
    {
        int amountOfUnconnected = 0;
        foreach(Room tempRoom in roomsList)
        {
            if (tempRoom.hasUnconnectedDoors)
            {
                amountOfUnconnected++;
            }
        }
        return amountOfUnconnected;
    }

    private int GetAvailableRoom()
    {
        Room tempRoom = roomsList.Find(x => x.hasUnconnectedDoors == true);
        return tempRoom.roomID;
    }

    public void GenerateBasicRoom(int doorwayPosition)
    {
        Room generatedRoom = new Room();
        generatedRoom.roomID = amountOfGeneratedRooms;
        roomsList.Add(generatedRoom);

        requiredAdditionalDoorways.Clear();
        List<int> possibleDoorSpawns = new List<int>();
        List<bool> doorCheck = possibleDoorways();
        Debug.Log("Door: " + amountOfGeneratedRooms + doorCheck[0] + "_" + doorCheck[1] + "_" + doorCheck[2] + "_" + doorCheck[3]);

        if(doorCheck[0] == true)
        {
            possibleDoorSpawns.Add(1);
        }
        if (doorCheck[1] == true)
        {
            possibleDoorSpawns.Add(3);
        }
        if (doorCheck[2] == true)
        {
            possibleDoorSpawns.Add(2);
        }
        if (doorCheck[3] == true)
        {
            possibleDoorSpawns.Add(4);
        }
        

        int amountOfDoors = Random.Range(1, possibleDoorSpawns.Count);

        List<int> doorsToSpawn = new List<int>();

        for(int t =0; t< amountOfDoors; t++)
        {
            doorsToSpawn.Add(possibleDoorSpawns[Random.Range(0, possibleDoorSpawns.Count)]);
        }

        if(doorsToSpawn.Count == 0)
        {
            Debug.Log("NO DOORS?!?!?!");
        }

        if (CheckForRoomsWithOpenDoorways() == 0 && doorsToSpawn.Count == 0)
        {
            doorsToSpawn.Add(possibleDoorSpawns[Random.Range(1, possibleDoorSpawns.Count)]);
        }

        doorsToSpawn.AddRange(requiredAdditionalDoorways);

        int requiredDoorwaySide = 0;

        // if the room has to flip the door position to fit onto another door
        if(doorwayPosition != 0) 
        {
            if(doorwayPosition == 1 || doorwayPosition == 2)
            {
                requiredDoorwaySide = 3;
            }
            else if (doorwayPosition == 3 || doorwayPosition == 4)
            {
                requiredDoorwaySide = 4;
            }
            else if(doorwayPosition == 5 || doorwayPosition == 6)
            {
                requiredDoorwaySide = 1;
            }
            else if(doorwayPosition == 7 || doorwayPosition == 8)
            {
                requiredDoorwaySide = 2;
            }
        }
        roomsList[amountOfGeneratedRooms].amountOfUnconnectedDoors = doorsToSpawn.Count + 1;
        roomsList[amountOfGeneratedRooms].doorPositions.AddRange(doorsToSpawn);
        roomsList[amountOfGeneratedRooms].doorPositions.Add(requiredDoorwaySide);
        roomsList[amountOfGeneratedRooms].hasUnconnectedDoors = true;

        List<List<GameObject>> roomTiles = new List<List<GameObject>>();

        #region top Wall spawning
        roomTiles.Add(createHorizontalWall("Top", topWallPrefabs, requiredDoorwaySide, false, 0, doorsToSpawn));
        #endregion

        int tileNumber = 0;

        for(int i = 0; i < 3; i++)
        {
            roomTiles.Add(createSideWallsRow(false, requiredDoorwaySide, false, doorsToSpawn));
        }

        bool spawnedDoor = false;

        for (int i = 0; i < 3; i++)
        {
            if(doorsToSpawn.Contains(4) || doorsToSpawn.Contains(2) || requiredDoorwaySide == 2 || requiredDoorwaySide == 4)
            {
                if (!spawnedDoor)
                {
                    roomTiles.Add(createSideWallsRow(true, requiredDoorwaySide, false, doorsToSpawn));
                    spawnedDoor = true;
                }
                else
                {
                    roomTiles.Add(createSideWallsRow(false, requiredDoorwaySide, true, doorsToSpawn));
                }
            }
            else
            {
                roomTiles.Add(createSideWallsRow(false, requiredDoorwaySide, false, doorsToSpawn));
            }
        }

        for (int i = 0; i < 3; i++)
        {
            roomTiles.Add(createSideWallsRow(false, requiredDoorwaySide, false, doorsToSpawn));
        }

        roomTiles.Add(createHorizontalWall("Bottom", bottomWallPrefabs, requiredDoorwaySide, false, 0, doorsToSpawn));

        int rowNumber = 0;

        GameObject Room_gameobject = new GameObject();
        Room_gameobject.transform.parent = levelParentObject.transform;
        if(doorwayPosition != 0)
        {
            Room_gameobject.name = "Room_" + amountOfGeneratedRooms + "_MODIFIED" + "__" + (doorsToSpawn.Count + 1);
        }
        else
        {
            Room_gameobject.name = "Room_" + amountOfGeneratedRooms;
        }
        

        foreach (List<GameObject> tileList in roomTiles)
        {
            if(rowNumber == 0 || rowNumber == 10)
            {
                tileNumber = 0;
            }
            else
            {
                tileNumber = 0;
            }
            
            foreach (GameObject tile in tileList)
            {
                if (tileNumber == 0)
                {
                    if (rowNumber == 5 || rowNumber == 6)
                    {
                        if (!doorsToSpawn.Contains(2) && requiredDoorwaySide != 2)
                        {
                            if (requiredDoorwaySide == 4 || doorsToSpawn.Contains(4))
                            {
                                tileNumber += 13;
                                //Debug.Log("Doors: " + requiredDoorwaySide + "_" + doorsToSpawn.Contains(2) + "_" + doorsToSpawn.Contains(4));
                            }
                        }
                    }
                }

                Vector3 tilePos = new Vector3(tilePositionsX[tileNumber], tilePositionsY[rowNumber], 0);
                var roomTile = Instantiate(tile, tilePos, Quaternion.identity);
                roomTile.transform.parent = Room_gameobject.transform;

                if (tileNumber == 0) 
                {
                    if (rowNumber == 0 || rowNumber == 10) //corner top and bottom
                    {
                        tileNumber += 2;
                    }
                    else
                    {
                        tileNumber += 13;
                    }
                    
                }
                else if (tileNumber == 6 && requiredDoorwaySide == 1 && rowNumber == 0 || tileNumber == 6 && doorsToSpawn.Contains(1) && rowNumber == 0) //top door
                {
                    tileNumber += 3;
                }
                else if(tileNumber == 6 && requiredDoorwaySide == 3 && rowNumber == 10 || tileNumber == 6 && doorsToSpawn.Contains(3) && rowNumber == 10) // bottom door
                {
                    tileNumber += 3;
                }
                else if (rowNumber == 0 || rowNumber == 10)
                {
                    tileNumber ++;
                }
                else
                {
                    //tileNumber += 13;
                }
            }
            rowNumber++;
        }
        amountOfGeneratedRooms++;
        generatedRooms.Add(Room_gameobject);
    }
}
