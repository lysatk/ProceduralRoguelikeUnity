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
    private static GameObject _projectileOther;

    private void Awake()
    {
        CreateParentObjects();
    }

    #region SpawnReturn
    public static GameObject SpawnObject(GameObject obj, Vector3 pos, quaternion rot, string layerString, SpellSource spellSource = SpellSource.None)
    {
        PooledSpellInfo pool = objectPools.Find(p => p.lookupString == obj.name);
        Debug.LogWarning("RecivedLayer" + layerString);
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
            spawnableObj.SetActive(false);
            spawnableObj.layer = LayerMask.NameToLayer(layerString);
            spawnableObj.SetActive(true);
            if (parentObject != null)
            {
                spawnableObj.transform.SetParent(parentObject.transform);

                SetLayerBasedOnParent(spawnableObj);
            }
        }
        else
        {
            spawnableObj.transform.position = pos;
            spawnableObj.transform.rotation = rot;
            spawnableObj.layer = LayerMask.NameToLayer(layerString);
            spawnableObj.SetActive(true);
            pool.InactiveObjects.Remove(spawnableObj);
        }
        spawnableObj.layer = LayerMask.NameToLayer(layerString);
        Debug.Log("finally set as:" + layerString);
        if (spawnableObj.GetComponent<SpellProjectileMult>() != null)
        { var _ = spawnableObj.GetComponent<SpellProjectileMult>().layerName = layerString; }
        return spawnableObj;
    }


    public static void ReturnObject(GameObject obj)
    {
        string name = obj.name.Substring(0, obj.name.Length - 7); //removing "(Clone)" from obj.name
        PooledSpellInfo pool = objectPools.Find(p => p.lookupString == name);

        if (pool == null)
        {
            Debug.Log("trying to release an non-pooled spell:" + obj.name);
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


        _projectileOther = new GameObject("OtherSpells");
        _projectileOther.transform.SetParent(_projectileParent.transform);
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

                return _projectileOther;
            default:
                return null;
        }
    }
    private static GameObject GetParentObjectByLayerName(string layerName)
    {
        switch (layerName)
        {
            case "PlayerSpell":
                return _projectilePlayer;
            case "EnemySpell":
                return _projectileEnemy;
            case "Spell":
                return _projectileOther;
            default:
                return _projectileParent;
        }
    }

    public static void ClearPools()
    {
        ReturnAllObjects();

        foreach (var pool in objectPools)
        {
            foreach (var obj in pool.InactiveObjects)
            {
                Destroy(obj);
            }
        }
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

    public static void DestroySpellsAll()
    {
        foreach (var pool in objectPools)
        {
            foreach (var obj in pool.InactiveObjects)
            {
                Destroy(obj);
            }

            var activeObjects = FindObjectsOfType<GameObject>().Where(obj => obj.name.StartsWith(pool.lookupString));
            foreach (var activeObj in activeObjects)
            {
                Destroy(activeObj);
            }
        }
        objectPools.Clear();
    }



    public static void ReturnSpellsWithoutSource()
    {

        var spellsToReturn = new List<GameObject>();

        foreach (var pool in objectPools)
        {
            foreach (var activeSpell in pool.InactiveObjects.Where(obj => obj.activeSelf))
            {
                if (activeSpell.transform.parent == null)
                {
                    spellsToReturn.Add(activeSpell);
                }
            }
        }

        foreach (var spell in spellsToReturn)
        {
            ReturnObject(spell);
        }
    }

    public static void ReturnAllObjects()
    {

        foreach (var pool in objectPools)
        {

            var activeObjects = FindObjectsOfType<GameObject>().Where(obj => obj.activeSelf && obj.name.StartsWith(pool.lookupString + "(Clone)"));


            foreach (var activeObj in activeObjects)
            {
                activeObj.SetActive(false);
                pool.InactiveObjects.Add(activeObj);
            }
        }
    }


    public static void DestroyAllPooledObjects()
    {
        foreach (var pool in objectPools)
        {

            foreach (var inactiveObj in pool.InactiveObjects)
            {
                Destroy(inactiveObj);
            }

            var activeObjects = FindObjectsOfType<GameObject>().Where(obj => obj.name.StartsWith(pool.lookupString + "(Clone)"));
            foreach (var activeObj in activeObjects)
            {
                Destroy(activeObj);
            }
        }

        objectPools.Clear();
    }

    public static void SetLayerBasedOnParent(GameObject child)
    {
        if (child != null && child.transform.parent != null)
        {
            GameObject parent = child.transform.parent.gameObject;

            if (parent == _projectilePlayer)
            {
                child.layer = 12;
            }
            else if (parent == _projectileEnemy)
            {
                child.layer = 13;
            }
            else
            {
                child.layer = 6;
            }
        }
    }


}

public class PooledSpellInfo
{
    public string lookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}