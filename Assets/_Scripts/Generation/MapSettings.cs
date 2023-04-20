using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

///<summary>
///This script makes the MapLayer class a scriptable object
///To create a new MapLayer, right click in the project view and select Map Layer Settings
///You can then use this script with the LevelGenerator to generate your level

[System.Serializable]
[CreateAssetMenu(fileName = "NewMapSettings", menuName = "Map Settings", order = 0)]
public class MapSettings : ScriptableObject
{
   
    public bool randomSeed;
    public float seed;
    public int fillAmount;
    public int smoothAmount;
    public int clearAmount;
    public int interval;
    public int minPathWidth, maxPathWidth, maxPathChange, roughness, windyness;
    public bool edgesAreWalls;
    public float modifier;
}

//Custom UI for our class
[CustomEditor(typeof(MapSettings))]
public class MapSettings_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        MapSettings mapLayer = (MapSettings)target;
        GUI.changed = false;
        EditorGUILayout.LabelField(mapLayer.name, EditorStyles.boldLabel);
        mapLayer.randomSeed = EditorGUILayout.Toggle("Random Seed", mapLayer.randomSeed);

        //Only appear if we have the random seed set to false
        if (!mapLayer.randomSeed)
        {
            mapLayer.seed = EditorGUILayout.FloatField("Seed", mapLayer.seed);
        }

        //Shows different options depending on what algorithm is selected
        mapLayer.clearAmount = EditorGUILayout.IntSlider("Amount To Clear", mapLayer.clearAmount, 0, 100);


        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        AssetDatabase.SaveAssets();

        if (GUI.changed)
            EditorUtility.SetDirty(mapLayer);
    }
}
