using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using NavMeshPlus.Components;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGenerator : MonoBehaviour
{

    [Tooltip("The Tilemap to draw walls onto")]
    public Tilemap tilemapWall;

    [Tooltip("The Tilemap to draw walls onto")]
    public Tilemap tilemapFloor;

    [Tooltip("The Tiles to draw walls with")]
    [SerializeField]
    public List<TileSet> tileSets = new List<TileSet>();

    [Tooltip("Width of our map")]
    public int width;

    [Tooltip("Height of our map")]
    public int height;

    [Tooltip("The settings of our map")]
    public MapSettings mapSetting;

    public NavMeshSurface Surface2D;

    static int tileIndex = 0;

    static public void SetTileIndex(int _, LevelGenerator lg)
    {

        if (lg.tileSets.Count > _)
            tileIndex = _;
    }
    static public int GetTileIndex()
    {
        return tileIndex;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ClearMap();
            GenerateMap();
        }
    }

    [ExecuteInEditMode]
    public int[,] GenerateMap()
    {
        ClearMap();

        int[,] map = new int[width, height];
        float seed;

        if (mapSetting.randomSeed)
        {
            seed = Time.time;
        }
        else
        {
            seed = mapSetting.seed;
        }

        map = MapFunctions.GenerateArray(width, height, false);

        map = MapFunctions.RandomWalkCave(map, seed, mapSetting.clearAmount);

        MapFunctions.RenderMap(map, tilemapWall, tilemapFloor, tileSets[tileIndex].wallTile, tileSets[tileIndex].obstacleTile, tileSets[tileIndex].floorTile, tileSets[tileIndex].cameraBackgroundColor);

        GameManager.map = map;

        Surface2D.BuildNavMeshAsync();
        return map;
    }

    public void ClearMap()
    {
        tilemapWall.ClearAllTiles();
        tilemapFloor.ClearAllTiles();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        LevelGenerator levelGen = (LevelGenerator)target;
        if (levelGen.mapSetting != null)
        {
            Editor mapSettingEditor = CreateEditor(levelGen.mapSetting);
            mapSettingEditor.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                levelGen.GenerateMap();
            }

            if (GUILayout.Button("Clear"))
            {
                levelGen.ClearMap();
            }
        }
    }
}
#endif