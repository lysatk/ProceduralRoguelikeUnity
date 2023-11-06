using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using System.Collections;




public class ObjectPool : MonoBehaviour
{

    public enum SpellSource
    {
        Player,
        Enemy,
        None
    }

    public static SpellSource spellSource;
    public static List<PooledSpellInfo> objectPools = new List<PooledSpellInfo>();

    private static GameObject _projectileParent;

    private static GameObject _projectilePlayer;
    private static GameObject _projectileEnemy;

    private void Awake()
    {
        CreateParentObjects();
    }



    #region SpawnReturn
    public static GameObject SpawnObject(GameObject obj, Vector3 pos, quaternion rot, string layerString, SpellSource spellSource = SpellSource.None)
    {
        PooledSpellInfo pool = objectPools.Find(p => p.lookupString == obj.name);

        if (pool == null)
        {

            pool = new PooledSpellInfo() { lookupString = obj.name };
            objectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            GameObject parentObject = SetParentObjectSourceType(spellSource);

            spawnableObj = Instantiate(obj, pos, rot);

            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);
            }
        }
        else
        {

            spawnableObj.transform.rotation = rot;
            spawnableObj.transform.position = pos;
            spawnableObj.SetActive(true);

            pool.InactiveObjects.Remove(spawnableObj);

        }
        spawnableObj.layer = LayerMask.NameToLayer(layerString);
        return spawnableObj;
    }


    public static void ReturnObject(GameObject obj)
    {
        string name = obj.name.Substring(0, obj.name.Length - 7); //removing "(Clone)" from obj.name
        PooledSpellInfo pool = objectPools.Find(p => p.lookupString == name);

        if (pool == null)
        {
            Debug.Log("trying to release an non-pooled obj:" + obj.name);
        }
        else
        {
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }

    #endregion

    public void CreateParentObjects()
    {
        _projectileParent = new GameObject("PooledObjects");

        _projectilePlayer = new GameObject("playerSpells");
        _projectilePlayer.transform.SetParent(_projectileParent.transform);

        _projectileEnemy = new GameObject("enemySpells");
        _projectileEnemy.transform.SetParent(_projectileParent.transform);
    }

    private static GameObject SetParentObjectSourceType(SpellSource spellSource)
    {

        switch (spellSource)
        {
            case SpellSource.Player:

                return _projectilePlayer;

            case SpellSource.Enemy:
                return _projectileEnemy;

            case SpellSource.None:
                return null;

            default:
                return null;
        }

    }

    public static void ClearPools()
    {
        objectPools.Clear();
    }

    public static void ReturnSpellsByParent(SpellSource spellSource)
    {
        GameObject parentObject = SetParentObjectSourceType(spellSource);
        if (parentObject != null)
        {
          
            var spellsToReturn = new List<GameObject>();
            foreach (Transform spellTransform in parentObject.transform)
            {
                if (spellTransform.gameObject.activeSelf)
                {
                    spellsToReturn.Add(spellTransform.gameObject);
                }
            }

           
            foreach (var spell in spellsToReturn)
            {
                ReturnObject(spell);
            }
        }
    }

    public static void DestroySpellsByParent(SpellSource spellSource)
    {
        GameObject parentObject = SetParentObjectSourceType(spellSource);
        if (parentObject != null)
        {
            
            var spellsToDestroy = new List<GameObject>();
            foreach (Transform spellTransform in parentObject.transform)
            {
                spellsToDestroy.Add(spellTransform.gameObject);
            }

           
            foreach (var spell in spellsToDestroy)
            {
                Destroy(spell);
            }
        }
    }

}

public class PooledSpellInfo
{
    public string lookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}