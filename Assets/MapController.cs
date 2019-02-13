﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    public static MapController instance;
    public Grid grid;
    public Tilemap walkableTiles;


    //2D grid. value is the weight of the tile
    public int[,] map;

    //change origin. these offsets are the actual coordinates of the left corner tile
    public int mapHeightOffset;
    public int mapWidthOffset;

    public int mapWidth;
    public int mapHeight;

    private void Awake()
    {
        //each scene load I want a new instance of the mapcontroller but it needs to stay static
        //needs to load start again in each scene
        if (instance == null)
        {
            instance = this;
        }
        InitMap();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void InitMap()
    {
        //This method of programmatically figuring out what the layout of the map is to ease the workload of designing maps
        //But this is not really optimal for load times??
        //initialize map
        //                x         y
        map = new int[mapWidth, mapHeight];
        for (int x = mapWidthOffset; x < mapWidthOffset + mapWidth; x++)
        {
            //Debug.Log("MapHeightOffset: " + mapHeightOffset);
            //Debug.Log("MapWidthOffset: " + mapWidthOffset);
            //Debug.Log("map bound: " + (mapHeightOffset - mapHeight));
            //Debug.Log("X: " + x);           
            for (int y = mapHeightOffset; y > (mapHeightOffset - mapHeight); y--)
            {
                Vector2Int currCoords = MapMath.GridToMap(new Vector3Int(x, y, 0));
                //Debug.Log("X: " + x + " Y: " + y);
                if (walkableTiles.GetTile(new Vector3Int(x, y, 0)))
                {
                    //Debug.Log(walkableTiles.GetTile(new Vector3Int(x, y, 0)).name);
                    switch (walkableTiles.GetTile(new Vector3Int(x, y, 0)).name)
                    {
                        //Grass Tiles
                        case "landscapeTiles_067":
                            map[currCoords.x, currCoords.y] = (int)TileWeight.UNOBSTRUCTED;
                            break;
                    }
                }
            }

        }
    }
    
}
public enum TileWeight
{
    //grass
    UNOBSTRUCTED = 1,
    LIGHT_OBS = 2,
    MEDIUM_OBS = 3,
    HEAVY_OBS = 4,
    //mountains, other units
    OBSTRUCTED = 99
}
