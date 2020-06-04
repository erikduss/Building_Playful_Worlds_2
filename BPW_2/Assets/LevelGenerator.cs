using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int wallLength = 4;

    private float maxWidth = 115.2f;
    private float minHeight = -98.56f;
    private float minWidth = -1.28f;
    private float maxHeight = 2.56f;

    public int spawnRoom;

    private int darknessHeight = 10;
    private int darknessWidth = 10;

    private int levelWidth = 120;
    private int levelHeight = 100;

    private List<float> tilePositionsX = new List<float>();
    private List<float> tilePositionsY = new List<float>();

    private List<float> roomPositionsX = new List<float>();
    private List<float> roomPositionsY = new List<float>();

    public List<Vector3> roomPositions = new List<Vector3>();

    private float roomLenth = 19.2f;
    private float roomHeight = 16.64f;

    public GameObject levelParentObject;
    public GameObject darknessParentObject;

    private int amountOfGeneratedRooms = 0;

    private int lastRoomPosition = 0;
    private int roomsSpawned = 0;

    public bool fullyGenerated = false;

    public GameObject darknessTile;

    private List<GameObject> generatedRooms = new List<GameObject>();
    private List<Room> roomsList = new List<Room>();

    private List<int> requiredAdditionalDoorways = new List<int>();

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
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

        CreateDarkSurrounding();
        GenerateDungeonFloor(3);
        GenerateRooms(36); //36!!!
    }

    void CreateDarkSurrounding()
    {
        // top layer of darkness
        for(int t = 0; t < darknessHeight; t++)
        {
            float yPos = maxHeight + (t * tileSize);
            for (float i = (minWidth - 10); i < (maxWidth + 10); i+=tileSize)
            {
                Vector3 tilePos = new Vector3(i, yPos, 0);
                var roomTile = Instantiate(darknessTile, tilePos, Quaternion.identity);
                roomTile.transform.parent = darknessParentObject.transform;
            }
        }

        // bottom layer of darkness
        for (int t = 0; t < darknessHeight; t++)
        {
            float yPos = minHeight - (t * tileSize);
            for (float i = (minWidth - 10); i < (maxWidth + 10); i += tileSize)
            {
                Vector3 tilePos = new Vector3(i, yPos, 0);
                var roomTile = Instantiate(darknessTile, tilePos, Quaternion.identity);
                roomTile.transform.parent = darknessParentObject.transform;
            }
        }

        // Left layer of darkness
        for (int t = 0; t < darknessWidth; t++)
        {
            float xPos = minWidth - (t * tileSize);
            for (float i = (minHeight - 10); i < (maxHeight + 10); i += tileSize)
            {
                Vector3 tilePos = new Vector3(xPos, i, 0);
                var roomTile = Instantiate(darknessTile, tilePos, Quaternion.identity);
                roomTile.transform.parent = darknessParentObject.transform;
            }
        }

        // Right layer of darkness
        for (int t = 0; t < darknessWidth; t++)
        {
            float xPos = maxWidth + (t * tileSize);
            for (float i = (minHeight - 10); i < (maxHeight + 10); i += tileSize)
            {
                Vector3 tilePos = new Vector3(xPos, i, 0);
                var roomTile = Instantiate(darknessTile, tilePos, Quaternion.identity);
                roomTile.transform.parent = darknessParentObject.transform;
            }
        }
    }

    private void CheckDungeon()
    {
        if (!fullyGenerated)
        {
            roomsList.Clear();
            generatedRooms.Clear();
            roomsSpawned = 0;
            lastRoomPosition = 0;
            amountOfGeneratedRooms = 0;

            foreach (Transform child in levelParentObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            GenerateDungeonFloor(3);
            GenerateRooms(36);
        }
        else
        {
            gameManager.LevelGenerationComplete(roomPositions[spawnRoom]);
        }
    }

    private void GenerateRooms(int amount)
    {
        int currentRow = 0;
        int currentRoom = 0;

        spawnRoom = 0;

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
                GenerateBasicRoom(bossRoomDoorwayPos,8, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 2:
                spawnRoom = 9;
                GenerateBasicRoom(bossRoomDoorwayPos,9, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 3:
                spawnRoom = 16;
                GenerateBasicRoom(bossRoomDoorwayPos,16, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 4:
                spawnRoom = 22;
                GenerateBasicRoom(bossRoomDoorwayPos,22, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 5:
                spawnRoom = 26;
                GenerateBasicRoom(bossRoomDoorwayPos,26, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 6:
                spawnRoom = 27;
                GenerateBasicRoom(bossRoomDoorwayPos,27, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 7:
                spawnRoom = 19;
                GenerateBasicRoom(bossRoomDoorwayPos,19, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
            case 8:
                spawnRoom = 13;
                GenerateBasicRoom(bossRoomDoorwayPos,13, true);
                generatedRooms[4].transform.position = roomPositions[spawnRoom];
                lastRoomPosition = spawnRoom;
                roomsList[4].roomPosition = spawnRoom;
                break;
        }

        for (int i = 5; i < amount; i++)
        {

            List<bool> possibleSpawns = possibleRooms(100);
            List<int> spawnDirections = new List<int>();
            

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
            else
            {
                if(CheckForRoomsWithOpenDoorways() > 0)
                {
                    int newRoomID = GetAvailableRoom();
                    lastRoomPosition = roomsList[newRoomID].roomPosition;

                    possibleSpawns = possibleRooms(100);

                    if (possibleSpawns[0] == true && roomsList[newRoomID].doorPositions.Contains(1))//up
                    {
                        if (lastRoomPosition != 26 && lastRoomPosition != 27)
                        {
                            spawnDirections.Add(1);
                        }
                    }
                    if (possibleSpawns[1] == true && roomsList[newRoomID].doorPositions.Contains(3))//down
                    {
                        if(lastRoomPosition != 8 && lastRoomPosition != 9)
                        {
                            spawnDirections.Add(3);
                        }
                    }
                    if (possibleSpawns[2] == true && roomsList[newRoomID].doorPositions.Contains(2))//right
                    {
                        if (lastRoomPosition != 13 && lastRoomPosition != 19)
                        {
                            spawnDirections.Add(2);
                        }
                    }
                    if (possibleSpawns[3] == true && roomsList[newRoomID].doorPositions.Contains(4))//left
                    {
                        if (lastRoomPosition != 16 && lastRoomPosition != 22)
                        {
                            spawnDirections.Add(4);
                        }
                    }
                    if(spawnDirections.Count > 0)
                    {
                        roomSpawnDirection = spawnDirections[Random.Range(0, spawnDirections.Count)];
                    }
                }
            }

            if(roomSpawnDirection == 1)
            {
                roomPos = lastRoomPosition - 6;
                GenerateBasicRoom(1,roomPos, false);
            }
            else if(roomSpawnDirection == 2)
            {
                roomPos = lastRoomPosition + 1;
                GenerateBasicRoom(3, roomPos,false);
            }
            else if (roomSpawnDirection == 3)
            {
                roomPos = lastRoomPosition + 6;
                GenerateBasicRoom(5, roomPos, false);
            }
            else if (roomSpawnDirection == 4)
            {
                roomPos = lastRoomPosition - 1;
                GenerateBasicRoom(7, roomPos, false);
            }
            else
            {
                break;
            }

            if(roomsList[i - 1].amountOfUnconnectedDoors <= 0)
            {
                roomsList[i - 1].hasUnconnectedDoors = false;
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
        if(roomsList.Count == 36)
        {
            fullyGenerated = true;
        }
        CheckDungeon();
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
        generatedRoom.isBossRoom = true;
        generatedRoom.hasUnconnectedDoors = false;
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

            for (int i = 0; i < 13; i++)
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

    private List<bool> possibleRooms(int roomPosition)
    {
        List<bool> possibleSpawns = new List<bool>();
        int roomPos = 0;
        bool isOccupied = false;

        bool canSpawnUp = false;
        bool canSpawnDown = false;
        bool canSpawnRight = false;
        bool canSpawnLeft = false;

        int checkFromPos = 0;

        if(roomPosition != 100)
        {
            roomPos = roomPosition - 6;
            checkFromPos = roomPosition;
        }
        else
        {
            roomPos = lastRoomPosition - 6; //up
            checkFromPos = lastRoomPosition;
        }

        if (checkFromPos == 0 || checkFromPos == 1 || checkFromPos == 2 || checkFromPos == 3 || checkFromPos == 4 || checkFromPos == 5 || checkFromPos == 26 || checkFromPos == 27)
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

        if (roomPosition != 100)
        {
            roomPos = roomPosition + 6;
        }
        else
        {
            roomPos = lastRoomPosition + 6; //down
        }
       

        if (checkFromPos == 30 || checkFromPos == 31 || checkFromPos == 32 || checkFromPos == 33 || checkFromPos == 34 || checkFromPos == 35 || checkFromPos == 8 || checkFromPos == 9)
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

        if (roomPosition != 100)
        {
            roomPos = roomPosition + 1;
        }
        else
        {
            roomPos = lastRoomPosition + 1; //right
        }
       

        if (checkFromPos == 5 || checkFromPos == 11 || checkFromPos == 17 || checkFromPos == 23 || checkFromPos == 29 || checkFromPos == 35 || checkFromPos == 13 || checkFromPos == 19)
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

        if (roomPosition != 100)
        {
            roomPos = roomPosition - 1;
        }
        else
        {
            roomPos = lastRoomPosition - 1; //left
        }
        

        if (checkFromPos == 0 || checkFromPos == 6 || checkFromPos == 12 || checkFromPos == 18 || checkFromPos == 24 || checkFromPos == 30 || checkFromPos == 36 || checkFromPos == 16 || checkFromPos == 22)
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

    private bool doorLeftPossible(int pos)
    {
        int roomPosToCheck = 0;
        bool isOccupied = false;

        bool canSpawnLeft = false;

        if(pos == 1)
        {

        }

        isOccupied = false;
        roomPosToCheck = pos - 1; //left

        if (pos == 0 || pos == 6 || pos == 12 || pos == 18 || pos == 24 || pos == 30 || pos == 36)
        {
            canSpawnLeft = false;
        }
        else
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPosToCheck)
                {
                    isOccupied = true;
                    if (checkRoom.hasUnconnectedDoors)
                    {
                        if (checkRoom.doorPositions.Contains(2))
                        {
                            requiredAdditionalDoorways.Add(4);
                            canSpawnLeft = false;
                            checkRoom.amountOfUnconnectedDoors--;
                            if (checkRoom.amountOfUnconnectedDoors <= 0)
                            {
                                checkRoom.amountOfUnconnectedDoors = 0;
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
        return canSpawnLeft;
    }

    private bool doorRightPossible(int pos)
    {
        int roomPosToCheck = 0;
        bool isOccupied = false;
        
        bool canSpawnRight = false;

        isOccupied = false;
        roomPosToCheck = pos + 1; //right

        if (pos == 5 || pos == 11 || pos == 17 || pos == 23 || pos == 29 || pos == 35)
        {
            canSpawnRight = false;
        }
        else if (roomPosToCheck < 36)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPosToCheck)
                {
                    isOccupied = true;
                    if (checkRoom.hasUnconnectedDoors)
                    {
                        if (checkRoom.doorPositions.Contains(4))
                        {
                            requiredAdditionalDoorways.Add(2);
                            canSpawnRight = false;
                            checkRoom.amountOfUnconnectedDoors--;
                            if (checkRoom.amountOfUnconnectedDoors <= 0)
                            {
                                checkRoom.amountOfUnconnectedDoors = 0;
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
        return canSpawnRight;
    }

    private bool doorUpPossible(int pos)
    {
        int roomPosToCheck = 0;
        bool isOccupied = false;

        bool canSpawnUp = false;

        roomPosToCheck = pos - 6; //up

        if (pos == 0 || pos == 1 || pos == 2 || pos == 3 || pos == 4 || pos == 5)
        {
            canSpawnUp = false;
        }
        else if (roomPosToCheck > 0)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPosToCheck)
                {
                    isOccupied = true;
                    if (checkRoom.hasUnconnectedDoors)
                    {
                        if (checkRoom.doorPositions.Contains(3))
                        {
                            requiredAdditionalDoorways.Add(1);
                            canSpawnUp = false;
                            checkRoom.amountOfUnconnectedDoors--;
                            if (checkRoom.amountOfUnconnectedDoors <= 0)
                            {
                                checkRoom.amountOfUnconnectedDoors = 0;
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
        return canSpawnUp;
    }

    private bool doorDownPossible(int pos)
    {
        int roomPosToCheck = 0;
        bool isOccupied = false;
        
        bool canSpawnDown = false;

        isOccupied = false;
        roomPosToCheck = pos + 6; //down

        if (pos == 30 || pos == 31 || pos == 32 || pos == 33 || pos == 34 || pos == 35)
        {
            canSpawnDown = false;
        }
        else if (roomPosToCheck < 36)
        {
            foreach (Room checkRoom in roomsList)
            {
                if (checkRoom.roomPosition == roomPosToCheck)
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
                                checkRoom.amountOfUnconnectedDoors = 0;
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

        return canSpawnDown;
    }

    private int CheckForRoomsWithOpenDoorways()
    {
        int amountOfUnconnected = 0;
        foreach(Room tempRoom in roomsList)
        {
            if (tempRoom.hasUnconnectedDoors && !tempRoom.isBossRoom)
            {
                if(tempRoom.amountOfUnconnectedDoors <= 0)
                {
                    tempRoom.amountOfUnconnectedDoors = 0;
                    tempRoom.hasUnconnectedDoors = false;
                }
                else
                {
                    int newRoomID = tempRoom.roomID;

                    List<bool> possibleSpawnsTEMP = possibleRooms(tempRoom.roomPosition);
                    List<int> spawnDirectionsTEMP = new List<int>();

                    if (possibleSpawnsTEMP[0] == true && roomsList[newRoomID].doorPositions.Contains(1))//up
                    {
                        if (tempRoom.roomPosition != 26 && tempRoom.roomPosition != 27)
                        {
                            spawnDirectionsTEMP.Add(1);
                        }
                    }
                    if (possibleSpawnsTEMP[1] == true && roomsList[newRoomID].doorPositions.Contains(3))//down
                    {
                        if (tempRoom.roomPosition != 8 && tempRoom.roomPosition != 9)
                        {
                            spawnDirectionsTEMP.Add(3);
                        }
                    }
                    if (possibleSpawnsTEMP[2] == true && roomsList[newRoomID].doorPositions.Contains(2))//right
                    {
                        if (tempRoom.roomPosition != 13 && tempRoom.roomPosition != 19)
                        {
                            spawnDirectionsTEMP.Add(2);
                        }
                    }
                    if (possibleSpawnsTEMP[3] == true && roomsList[newRoomID].doorPositions.Contains(4))//left
                    {
                        if (tempRoom.roomPosition != 16 && tempRoom.roomPosition != 22)
                        {
                            spawnDirectionsTEMP.Add(4);
                        }
                    }

                    if(spawnDirectionsTEMP.Count > 0)
                    {
                        amountOfUnconnected++;
                    }
                }
            }
        }
        return amountOfUnconnected;
    }

    private int GetAvailableRoom()
    {
        Room tempRoom = roomsList.Find(x => x.hasUnconnectedDoors == true);
        
        return (tempRoom.roomID);
    }

    public void GenerateBasicRoom(int doorwayPosition, int thisRoomPosition, bool spawnRoom)
    {
        Room generatedRoom = new Room();
        generatedRoom.roomID = amountOfGeneratedRooms;
        generatedRoom.roomPosition = thisRoomPosition;
        roomsList.Add(generatedRoom);

        requiredAdditionalDoorways.Clear();
        List<int> possibleDoorSpawns = new List<int>();

        int requiredDoorwaySide = 0;

        // if the room has to flip the door position to fit onto another door
        if (doorwayPosition != 0)
        {
            if (doorwayPosition == 1 || doorwayPosition == 2)
            {
                requiredDoorwaySide = 3;
            }
            else if (doorwayPosition == 3 || doorwayPosition == 4)
            {
                requiredDoorwaySide = 4;
            }
            else if (doorwayPosition == 5 || doorwayPosition == 6)
            {
                requiredDoorwaySide = 1;
            }
            else if (doorwayPosition == 7 || doorwayPosition == 8)
            {
                requiredDoorwaySide = 2;
            }
        }

        if (doorLeftPossible(thisRoomPosition) == true)
        {
            if(thisRoomPosition != 0 || thisRoomPosition != 6 || thisRoomPosition != 12 || thisRoomPosition != 18 || thisRoomPosition != 24 || thisRoomPosition != 30 || thisRoomPosition != 36 || thisRoomPosition != 16 || thisRoomPosition != 22)
            {
                 possibleDoorSpawns.Add(4);
            }
            else if (spawnRoom)
            {
                possibleDoorSpawns.Add(4);
            }
        }
        if (doorRightPossible(thisRoomPosition) == true)
        {
            if (thisRoomPosition != 5 || thisRoomPosition != 11 || thisRoomPosition != 17 || thisRoomPosition != 23 || thisRoomPosition != 29 || thisRoomPosition != 35 || thisRoomPosition != 13 || thisRoomPosition != 19)
            {
                    possibleDoorSpawns.Add(2);
            }
            else if (spawnRoom)
            {
                possibleDoorSpawns.Add(2);
            }
        }
        if (doorUpPossible(thisRoomPosition) == true)
        {
            if (thisRoomPosition != 0 || thisRoomPosition != 1 || thisRoomPosition != 2 || thisRoomPosition != 3 || thisRoomPosition != 4 || thisRoomPosition != 5 || thisRoomPosition != 26 || thisRoomPosition != 27)
            {
                    possibleDoorSpawns.Add(1);
            }
            else if (spawnRoom)
            {
                possibleDoorSpawns.Add(1);
            }
        }
        if (doorDownPossible(thisRoomPosition) == true)
        {
            if (thisRoomPosition != 30 || thisRoomPosition != 31 || thisRoomPosition != 32 || thisRoomPosition != 33 || thisRoomPosition != 34 || thisRoomPosition != 35 || thisRoomPosition != 36 || thisRoomPosition != 8 || thisRoomPosition != 9)
            {
                    possibleDoorSpawns.Add(3);
            }
            else if (spawnRoom)
            {
                possibleDoorSpawns.Add(3);
            }
        }

        int amountOfDoors = 0;

        if(possibleDoorSpawns.Contains(2) || possibleDoorSpawns.Contains(4) || requiredAdditionalDoorways.Contains(2) || requiredAdditionalDoorways.Contains(4))
        {
            if (possibleDoorSpawns.Contains(4) || requiredAdditionalDoorways.Contains(4))
            {
                if(thisRoomPosition == 0 || thisRoomPosition == 6 || thisRoomPosition == 12 || thisRoomPosition == 18 || thisRoomPosition == 24 || thisRoomPosition == 30 || thisRoomPosition == 13 || thisRoomPosition == 19)
                {
                    if (!spawnRoom)
                    {
                        possibleDoorSpawns.Remove(4);
                    }
                }
            }
            if (possibleDoorSpawns.Contains(2) || requiredAdditionalDoorways.Contains(2))
            {
                if (thisRoomPosition == 5 || thisRoomPosition == 11 || thisRoomPosition == 17 || thisRoomPosition == 23 || thisRoomPosition == 29 || thisRoomPosition == 35 || thisRoomPosition == 16 || thisRoomPosition == 22)
                {
                    if (!spawnRoom)
                    {
                        possibleDoorSpawns.Remove(2);
                    }
                }
            }
        }
        if(possibleDoorSpawns.Contains(1) || possibleDoorSpawns.Contains(3) || requiredAdditionalDoorways.Contains(1) || requiredAdditionalDoorways.Contains(3))
        {
            if (possibleDoorSpawns.Contains(1) || requiredAdditionalDoorways.Contains(1))
            {
                if (thisRoomPosition == 26 || thisRoomPosition == 27)
                {
                    if (!spawnRoom)
                    {
                        possibleDoorSpawns.Remove(1);
                    }
                }
            }
            if (possibleDoorSpawns.Contains(3) || requiredAdditionalDoorways.Contains(3))
            {
                if (thisRoomPosition == 8 || thisRoomPosition == 9)
                {
                    if (!spawnRoom)
                    {
                        possibleDoorSpawns.Remove(3);
                    }
                }
            }
        }

        if (possibleDoorSpawns.Contains(2) || requiredAdditionalDoorways.Contains(2))
        {
            if(thisRoomPosition == 5 || thisRoomPosition == 11 || thisRoomPosition == 17 || thisRoomPosition == 23 || thisRoomPosition == 29 || thisRoomPosition == 35)
            {
                if (possibleDoorSpawns.Contains(2))
                {
                    possibleDoorSpawns.Remove(2);
                }
                if (requiredAdditionalDoorways.Contains(2))
                {
                    requiredAdditionalDoorways.Remove(2);
                }
            }
        }

        if (possibleDoorSpawns.Contains(4) || requiredAdditionalDoorways.Contains(4))
        {
            if (thisRoomPosition == 0 || thisRoomPosition == 6 || thisRoomPosition == 12 || thisRoomPosition == 18 || thisRoomPosition == 24 || thisRoomPosition == 30)
            {
                if (possibleDoorSpawns.Contains(4))
                {
                    possibleDoorSpawns.Remove(4);
                }
                if(requiredAdditionalDoorways.Contains(4))
                {
                    requiredAdditionalDoorways.Remove(4);
                }
            }
        }

        if (possibleDoorSpawns.Contains(requiredDoorwaySide))
        {
            int removeID = requiredDoorwaySide;
            possibleDoorSpawns.Remove(removeID);
        }
        ///////////////////////////////////////////////////

        if (thisRoomPosition == 7 || thisRoomPosition == 8 || thisRoomPosition == 9 || thisRoomPosition == 10 || thisRoomPosition == 13 || thisRoomPosition == 16 || thisRoomPosition == 19 || thisRoomPosition == 22 || thisRoomPosition == 25 || thisRoomPosition == 26 || thisRoomPosition == 27 || thisRoomPosition == 28)
        {
            switch (possibleDoorSpawns.Count)
            {
                case 1:
                    amountOfDoors = 1;
                    break;
                case 2:
                    amountOfDoors = 2;
                    break;
                case 3:
                    amountOfDoors = 3;
                    break;
                case 4:
                    amountOfDoors = 4;
                    break;
                default:
                    amountOfDoors = Random.Range(1, possibleDoorSpawns.Count);
                    break;
            }
        }
        else
        {
            amountOfDoors = Random.Range(1, possibleDoorSpawns.Count);
        }

        List<int> doorsToSpawn = new List<int>();

        if(thisRoomPosition == 1)
        {
            if (possibleDoorSpawns.Contains(4))
            {
                doorsToSpawn.Add(4);
            }
        }
        else if (thisRoomPosition == 6)
        {
            if (possibleDoorSpawns.Contains(1))
            {
                doorsToSpawn.Add(1);
            }
        }
        else if (thisRoomPosition == 24)
        {
            if (possibleDoorSpawns.Contains(3))
            {
                doorsToSpawn.Add(3);
            }
        }
        else if (thisRoomPosition == 31)
        {
            if (possibleDoorSpawns.Contains(4))
            {
                doorsToSpawn.Add(4);
            }
        }
        else if (thisRoomPosition == 4)
        {
            if (possibleDoorSpawns.Contains(2))
            {
                doorsToSpawn.Add(2);
            }
        }
        else if (thisRoomPosition == 11)
        {
            if (possibleDoorSpawns.Contains(1))
            {
                doorsToSpawn.Add(1);
            }
        }
        else if (thisRoomPosition == 29)
        {
            if (possibleDoorSpawns.Contains(3))
            {
                doorsToSpawn.Add(3);
            }
        }
        else if (thisRoomPosition == 34)
        {
            if (possibleDoorSpawns.Contains(2))
            {
                doorsToSpawn.Add(2);
            }
        }

        if (possibleDoorSpawns.Count > 0)
        {
            for (int t = 0; t < amountOfDoors; t++)
            {
                int doorNumber = possibleDoorSpawns[Random.Range(0, possibleDoorSpawns.Count)];
                if (!doorsToSpawn.Contains(doorNumber))
                {
                    doorsToSpawn.Add(doorNumber);
                }
            }
        }

        if (CheckForRoomsWithOpenDoorways() <= 1)
        {
            if(possibleDoorSpawns.Count > 0 && doorsToSpawn.Count == 0)
            {
                int doorNumber = possibleDoorSpawns[Random.Range(0, possibleDoorSpawns.Count)];
                if (!doorsToSpawn.Contains(doorNumber))
                {
                    doorsToSpawn.Add(doorNumber);
                }
            }
            else if(possibleDoorSpawns.Count > 0 && doorsToSpawn.Count != possibleDoorSpawns.Count)
            {
                int doorNumber = possibleDoorSpawns[Random.Range(0, possibleDoorSpawns.Count)];
                if (!doorsToSpawn.Contains(doorNumber))
                {
                    doorsToSpawn.Add(doorNumber);
                }
            }
            
        }
        if(amountOfGeneratedRooms < 10 && doorsToSpawn.Count == 0 && possibleDoorSpawns.Count > 0)
        {
            int doorNumber = possibleDoorSpawns[Random.Range(0, possibleDoorSpawns.Count)];
            if (!doorsToSpawn.Contains(doorNumber))
            {
                doorsToSpawn.Add(doorNumber);
            }
        }

        if(amountOfGeneratedRooms > 20 && doorsToSpawn.Count <= 1 && possibleDoorSpawns.Count > 0)
        {
            int doorNumber = possibleDoorSpawns[Random.Range(0, possibleDoorSpawns.Count)];
            if (!doorsToSpawn.Contains(doorNumber))
            {
                doorsToSpawn.Add(doorNumber);
            }
        }
        

        if (doorsToSpawn.Count == 0)
        {
            if(possibleDoorSpawns.Count > 0 && amountOfGeneratedRooms >= 30)
            {
                int doorNumber = possibleDoorSpawns[Random.Range(1, possibleDoorSpawns.Count)];
                if (!doorsToSpawn.Contains(doorNumber))
                {
                    doorsToSpawn.Add(doorNumber);
                }
            }
        }

        foreach(int tempInt in requiredAdditionalDoorways)
        {
            if (doorsToSpawn.Contains(tempInt))
            {
                doorsToSpawn.Remove(tempInt);
            }
        }

        if (doorsToSpawn.Contains(requiredDoorwaySide))
        {
            doorsToSpawn.Remove(requiredDoorwaySide);
        }

        roomsList[amountOfGeneratedRooms].roomID = amountOfGeneratedRooms;
        roomsList[amountOfGeneratedRooms].amountOfUnconnectedDoors = doorsToSpawn.Count;
        roomsList[amountOfGeneratedRooms].doorPositions.AddRange(doorsToSpawn);
        roomsList[amountOfGeneratedRooms].doorPositions.Add(requiredDoorwaySide);
        roomsList[amountOfGeneratedRooms].doorPositions.AddRange(requiredAdditionalDoorways);
        if(doorsToSpawn.Count > 0)
        {
            roomsList[amountOfGeneratedRooms].hasUnconnectedDoors = true;
        }

        int extraDoors = 0;

        if (requiredAdditionalDoorways.Count > 0)
        {
            doorsToSpawn.AddRange(requiredAdditionalDoorways);
            extraDoors += requiredAdditionalDoorways.Count;
        }

        //EVERYTHING BELOW THIS WORKS AS INTENDED IF YOU ADD 4 DOORS STANDARD IT WILL ADD IT TO ALL OF THEM CORRECTLY

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
            Room_gameobject.name = "Room_" + amountOfGeneratedRooms + "_MODIFIED";
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
