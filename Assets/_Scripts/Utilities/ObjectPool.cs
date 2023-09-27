using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;
using System.Collections;

public class ObjectPool : StaticInstance<GameManager>
{
    public static List<PooledSpellInfo> objectPools = new List<PooledSpellInfo>();
    public static GameObject SpawnObject(GameObject obj, Vector3 pos, quaternion rot,string layerString)
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
            spawnableObj = Instantiate(obj, pos, rot);
        }
        else
        {
            spawnableObj.transform.position = pos;
            spawnableObj.transform.rotation = rot;
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
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

    public static void ClearPools()
    {
        objectPools = new List<PooledSpellInfo>();
    }
}

public class PooledSpellInfo
{
    public string lookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}