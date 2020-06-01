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

    public int amountOfRoomsGenerated = 0;
    private float tileSize = 1.28f;
    private int roomLength = 13;
    private int wallLength = 4;

    private int levelWidth = 150;
    private int levelHeight = 150;

    private List<float> tilePositionsX = new List<float>();
    private List<float> tilePositionsY = new List<float>();

    private List<float> roomPositionsX = new List<float>();
    private List<float> roomPositionsY = new List<float>();

    private float roomLenth = 19.2f;
    private float roomHeight = 16.64f;

    public GameObject levelParentObject;

    private int amountOfGeneratedRooms = 0;

    private List<GameObject> generatedRooms = new List<GameObject>();

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

        GenerateDungeonFloor(3);
        GenerateRooms(36);
    }

    private void GenerateRooms(int amount)
    {
        int currentRow = 0;
        int currentRoom = 0;

        int bossRoomDoorwayPos = Random.Range(1, 9);

        for (int i = 0; i < amount; i++)
        {
            if (i == 14)
            {
                CreateBossRoom(1, bossRoomDoorwayPos);
            }
            else if (i == 15)
            {
                CreateBossRoom(2, bossRoomDoorwayPos);
            }
            else if (i == 20)
            {
                CreateBossRoom(3, bossRoomDoorwayPos);
            }
            else if (i == 21)
            {
                CreateBossRoom(4, bossRoomDoorwayPos);
            }
            else
            {
                GenerateBasicRoom();
            }
            

            Vector3 roomPosition = new Vector3(roomPositionsX[currentRoom], roomPositionsY[currentRow], 0);
            generatedRooms[i].transform.position = roomPosition;

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

    private List<GameObject> createHorizontalWall(string wallPlacement, List<GameObject> wallPrefabToAdd, int doorwayPlacement, bool bossRoom, int bossPart)
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
            if (doorwayPlacement == 1 && wallPlacement == "Top")
            {
                returnList.Add(doorwayPrefabs[2]);
            }
            else if (doorwayPlacement == 3 && wallPlacement == "Bottom")
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

    private List<GameObject> createSideWallsRow(bool spawnDoor, int doorwayPlacement, bool spawnedDoor)
    {
        List<GameObject> returnList = new List<GameObject>();

        if (!spawnDoor)
        {
            if (!spawnedDoor)
            {
                returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);

                returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
            }
            else if (spawnedDoor && doorwayPlacement == 4)
            {
                returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
            }
            else if (spawnedDoor && doorwayPlacement == 2)
            {
                returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);
            }
        }
        else
        {
            if (doorwayPlacement == 4)
            {
                returnList.Add(doorwayPrefabs[1]);
            }
            else
            {
                returnList.Add(leftWallPrefabs[Random.Range(0, leftWallPrefabs.Count - 1)]);
            }

            if (doorwayPlacement == 2)
            {
                returnList.Add(doorwayPrefabs[3]);
            }
            else
            {
                returnList.Add(rightWallPrefabs[Random.Range(0, rightWallPrefabs.Count - 1)]);
            }
        }
        return returnList;
    }

    public void CreateBossRoom(int part, int doorwaySide) 
    {
        Room generatedRoom = new Room();
        generatedRoom.roomID = amountOfRoomsGenerated;

        Debug.Log(doorwaySide);

        List<List<GameObject>> roomTiles = new List<List<GameObject>>();
        bool spawnedDoor = false;

        #region top Wall spawning
        if (part == 1 || part == 2)
        {
            roomTiles.Add(createHorizontalWall("Top", topWallPrefabs, doorwaySide, true, part));
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
            roomTiles.Add(createHorizontalWall("Bottom", bottomWallPrefabs, doorwaySide, true, part));
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

    public void GenerateBasicRoom()
    {
        Room generatedRoom = new Room();
        generatedRoom.roomID = amountOfRoomsGenerated;

        int doorwaySide = Random.Range(1, 5);

        List<List<GameObject>> roomTiles = new List<List<GameObject>>();

        #region top Wall spawning
        roomTiles.Add(createHorizontalWall("Top", topWallPrefabs, doorwaySide, false, 0));
        #endregion

        int tileNumber = 0;

        for(int i = 0; i < 3; i++)
        {
            roomTiles.Add(createSideWallsRow(false, doorwaySide, false));
        }

        bool spawnedDoor = false;

        for (int i = 0; i < 3; i++)
        {
            if(doorwaySide == 4 || doorwaySide == 2)
            {
                if (!spawnedDoor)
                {
                    roomTiles.Add(createSideWallsRow(true, doorwaySide, false));
                    spawnedDoor = true;
                }
                else
                {
                    roomTiles.Add(createSideWallsRow(false, doorwaySide, true));
                }
            }
            else
            {
                roomTiles.Add(createSideWallsRow(false, doorwaySide, false));
            }
        }

        for (int i = 0; i < 3; i++)
        {
            roomTiles.Add(createSideWallsRow(false, doorwaySide, false));
        }

        roomTiles.Add(createHorizontalWall("Bottom", bottomWallPrefabs, doorwaySide, false, 0));

        int rowNumber = 0;

        GameObject Room_gameobject = new GameObject();
        Room_gameobject.transform.parent = levelParentObject.transform;
        Room_gameobject.name = "Room_" + amountOfGeneratedRooms;

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
                if (tileNumber == 0 && doorwaySide == 4)
                {
                    if (rowNumber == 5 || rowNumber == 6)
                    {
                        tileNumber += 13;
                    }
                }

                Vector3 tilePos = new Vector3(tilePositionsX[tileNumber], tilePositionsY[rowNumber], 0);
                var roomTile = Instantiate(tile, tilePos, Quaternion.identity);
                roomTile.transform.parent = Room_gameobject.transform;

                if (tileNumber == 0) //corner top and bottom
                {
                    if (rowNumber == 0 || rowNumber == 10)
                    {
                        tileNumber += 2;
                    }
                    else
                    {
                        tileNumber += 13;
                    }
                    
                }
                else if (tileNumber == 6 && doorwaySide == 1 && rowNumber == 0) //top door
                {
                    tileNumber += 3;
                }
                else if(tileNumber == 6 && doorwaySide == 3 && rowNumber == 10) // bottom door
                {
                    tileNumber += 3;
                }
                else if (rowNumber == 0 || rowNumber == 10)
                {
                    tileNumber ++;
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
}
