using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBossBasic : MonoBehaviour
{
    [SerializeField]
    List<Spell> prefabList;
    [SerializeField]
    int numOfProjectiles;
    [SerializeField]
    float rotAngle = 0f;
    [SerializeField]
    float spawnDelay = 0.5f;
    string layerName;
    private void Awake()
    {
        gameObject.SetActive(false);
        layerName = LayerMask.LayerToName(gameObject.layer);
    }
    private void OnEnable()
    {
        Debug.Log("OnEnable");
        layerName = LayerMask.LayerToName(gameObject.layer);
        Debug.Log(layerName);

        StartCoroutine(SpawnWithDelay(numOfProjectiles));
    }

    IEnumerator SpawnWithDelay(int n)
    {
        layerName = LayerMask.LayerToName(gameObject.layer);

        if (n % 2 == 0) transform.Rotate(0f, 0f, -rotAngle);
        else transform.Rotate(0f, 0f, -rotAngle * 2);

        for (int i = 0; i < n; i++)
        {
            Debug.Log("From Spellspawner"+layerName);
            RandSpellFromList().Attack( transform.position, transform.rotation, layerName);

            transform.Rotate(0f, 0f, rotAngle);

            yield return new WaitForSeconds(spawnDelay);
        }
       ObjectPool.ReturnObject(gameObject);

    }
    Spell RandSpellFromList()
    {
        int randomIndex = Random.Range(0, prefabList.Count);
        return prefabList[randomIndex];

    }
}
