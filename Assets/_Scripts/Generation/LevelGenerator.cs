using UnityEngine;
using UnityEngine.Tilemaps;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelGenerator : MonoBehaviour {

	[Tooltip("The Tilemap to draw walls onto")]
	public Tilemap tilemapWall;

    [Tooltip("The Tilemap to draw walls onto")]
    public Tilemap tilemapFloor;

    [Tooltip("The Tile to draw walls with")]
	public TileBase tileWall;

    [Tooltip("The Tile to draw floor with")]
    public TileBase tileFloor;

    [Tooltip("The Tile to be used as a spawnerPoint")]
	public TileBase tileSpawner;

	[Tooltip("Width of our map")]
	public int width;

	[Tooltip("Height of our map")]
	public int height;
	
	[Tooltip("The settings of our map")]
	public MapSettings mapSetting;

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

		//Generate the map depending omapSen the algorithm selected
				//First generate our array
				map = MapFunctions.GenerateArray(width, height, false);
				//Next generate the random walk cave
				map = MapFunctions.RandomWalkCave(map, seed, mapSetting.clearAmount);		
		//Render the result
		MapFunctions.RenderMap(map, tilemapWall,tilemapFloor,tileWall,tileSpawner,tileFloor);

		GameManager.map = map;

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

		//Reference to our script
		LevelGenerator levelGen = (LevelGenerator)target;
		
		//Only show the mapsettings UI if we have a reference set up in the editor
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