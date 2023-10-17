using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBossBasic : MonoBehaviour
{
    [SerializeField]
    List<GameObject> prefabList;
    [SerializeField]
    int numOfProjectiles;
    [SerializeField]
    float rotAngle = 0f;
    [SerializeField]
    float spawnDelay = 0.5f;
    string layerName;
    private void OnEnable()
    {
        layerName = LayerMask.LayerToName(gameObject.layer);    
        StartCoroutine(SpawnWithDelay(numOfProjectiles));
    }

    IEnumerator SpawnWithDelay(int n)
    {
        if (n % 2 == 0) transform.Rotate(0f, 0f, -rotAngle);
        else transform.Rotate(0f, 0f, -rotAngle * 2);

        for (int i = 0; i < n; i++)
        {
            ObjectPool.SpawnObject(RandSpellFromList(), transform.position, transform.rotation, layerName);

            transform.Rotate(0f, 0f, rotAngle);

            yield return new WaitForSeconds(spawnDelay);
        }
       ObjectPool.ReturnObject(gameObject);

    }
    GameObject RandSpellFromList()
    {
        int randomIndex = Random.Range(0, prefabList.Count);
        return prefabList[randomIndex];

    }
}
