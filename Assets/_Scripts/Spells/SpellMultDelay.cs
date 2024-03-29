using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellMultDelay : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int numOfProjectiles;
    [SerializeField]
    float rotAngle = 0f;
    [SerializeField]
    float spawnDelay = 0.5f; // Adjust this to control the delay between spawns
    string layerName;
    private void Awake()
    {
        gameObject.SetActive(false);
        layerName = LayerMask.LayerToName(gameObject.layer);
    }
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
            ObjectPool.SpawnObject(prefab, transform.position, transform.rotation, layerName);
            transform.Rotate(0f, 0f, rotAngle);
            yield return new WaitForSeconds(spawnDelay); // Delay between spawns
        }

        ObjectPool.ReturnObject(gameObject);
    }
}
