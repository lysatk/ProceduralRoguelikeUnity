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
            Debug.Log("1");
            pool = new PooledSpellInfo() { lookupString = obj.name };
            Debug.Log("2");
            objectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if (spawnableObj == null)
        {
            Debug.Log("3");
            spawnableObj = Instantiate(obj, pos, rot);

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
        Debug.Log("9");
        string name = obj.name.Substring(0, obj.name.Length - 7); //removing "(Clone)" from obj.name
        Debug.Log("10");
        PooledSpellInfo pool = objectPools.Find(p => p.lookupString == name);

        if (pool == null)
        {
            Debug.Log("trying to release an non-pooled obj:" + obj.name);

        }
        else
        {
            Debug.Log("11");
            obj.SetActive(false);
            Debug.Log("BackInPool"+name+": "+ pool.InactiveObjects.Count.ToString());
            pool.InactiveObjects.Add(obj);
        }
    }

    public static void ClearPools()
    {

        objectPools.Clear();
         
    }
}

public class PooledSpellInfo
{
    public string lookupString;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}