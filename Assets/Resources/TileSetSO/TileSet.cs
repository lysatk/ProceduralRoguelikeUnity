using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileSet", menuName = "ScriptableObjects/Tilesets", order = 1)]
public class TileSet : ScriptableObject
{
    public TileBase floorTile;
    public TileBase wallTile;
    public TileBase obstacleTile;
    public Color cameraBackgroundColor=new Color();
}
