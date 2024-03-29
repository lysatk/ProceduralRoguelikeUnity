using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// An scene-specific manager spawning units
/// </summary>
public class UnitManager : StaticInstance<UnitManager>
{
    [SerializeField]
    private Canvas worldSpaceCanvas;

    /// <summary>
    /// Spawns hero on the map
    /// </summary>
    /// <param name="mageName">name of the mage you want to spawn</param>
    /// <returns>reference to spawned character</returns>
    public GameObject SpawnHero(string mageName)
    {
        return SpawnUnit(mageName, GetPlayerSpawner());
    }

    /// <summary>
    /// spawn hero on given position
    /// </summary>
    /// <param name="mageName">name of the mage you want to spawn</param>
    /// <param name="vector2">position on map where you want player to be</param>
    /// <returns>reference to spawned character</returns>
    public GameObject SpawnHero(string mageName, Vector2 vector2)
    {
        return SpawnUnit(mageName, vector2);
    }

    /// <summary>
    /// Spawns enemy on the map
    /// </summary>
    /// <param name="e">type of the enemy to spawn</param>
    /// <param name="scaleMultiplier">multiplier to spawned enemy stats</param>
    /// <returns>reference to spawned enemy</returns>
    public GameObject SpawnEnemy(ExampleEnemyType e, float scaleMultiplier)
    {
        GameManager.enemies.Add(SpawnUnit(e, GetRandomSpawner(), scaleMultiplier));
        return GameManager.enemies.Last();
    }

    GameObject SpawnUnit(string unitName, Vector3 pos)
    {
        var ScriptableHero = ResourceSystem.Instance.GetExampleHero(unitName);

        if (ScriptableHero != null)
        {
            var heroSpawned = Instantiate(ScriptableHero.Prefab, pos, Quaternion.identity, transform);

            Camera.main.gameObject.GetComponent<CameraManager>().target = heroSpawned.transform;

            var stats = ScriptableHero.BaseStats;

            stats.CurrentHp = stats.MaxHp;

            heroSpawned.SetStats(stats);

            return heroSpawned.gameObject;
        }
        return null;
    }

    GameObject SpawnUnit(ExampleEnemyType enemyType, Vector3 pos, float scaleMultiplier)
    {
        var ScriptableEnemy = ResourceSystem.Instance.GetExampleEnemy(enemyType);

        if (ScriptableEnemy != null)
        {
            var enemySpawned = Instantiate(ScriptableEnemy.Prefab, pos, Quaternion.identity, transform);

            var stats = ScriptableEnemy.BaseStats;

            stats.MaxHp = Convert.ToInt32(stats.MaxHp * scaleMultiplier);
            stats.CurrentHp = stats.MaxHp;
            stats.Armor *= scaleMultiplier;
            stats.DmgModifier *= scaleMultiplier;

            enemySpawned.SetStats(stats);
            
            return enemySpawned.gameObject;
        }
        return null;
    }

  
    Vector3 GetRandomSpawner()
    {
        List<Vector2Int> spawners = new();
        for (int i = 0; i < GameManager.map.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.map.GetLength(1); j++)
            {
                if (GameManager.map[i, j] == 2)
                    spawners.Add(new(i, j));
            }
        }

        Vector2Int tempPoint = spawners.ElementAt(Random.Range(0, spawners.Count)); // get random spwaner

        //Choose direction 
        switch (Random.Range(0, 8))
        {
            case 0:
                tempPoint = new Vector2Int(tempPoint.x + 1, tempPoint.y);
                break;
            case 1:
                tempPoint = new Vector2Int(tempPoint.x - 1, tempPoint.y);
                break;
            case 2:
                tempPoint = new Vector2Int(tempPoint.x + 1, tempPoint.y + 1);
                break;
            case 3:
                tempPoint = new Vector2Int(tempPoint.x - 1, tempPoint.y + 1);
                break;
            case 4:
                tempPoint = new Vector2Int(tempPoint.x - 1, tempPoint.y - 1);
                break;
            case 5:
                tempPoint = new Vector2Int(tempPoint.x + 1, tempPoint.y - 1);
                break;
            case 6:
                tempPoint = new Vector2Int(tempPoint.x, tempPoint.y - 1);
                break;
            case 7:
                tempPoint = new Vector2Int(tempPoint.x, tempPoint.y + 1);
                break;
        }
        return TileToPosition(tempPoint);
    }
   public Vector3 GetPlayerSpawner()
    {
        for (int i = 0; i < GameManager.map.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.map.GetLength(1); j++)
            {
                if (GameManager.map[i, j] == 3)
                    return TileToPosition(new(i, j));
            }
        }
        return TileToPosition(new(GameManager.map.GetLength(0) / 2, GameManager.map.GetLength(1) / 2));
    }
    Vector2 TileToPosition(Vector2Int target)
    {
        return new(target.x * 1.6f, target.y * 1.6f);
    }
}