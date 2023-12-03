using UnityEngine;

public class SpellProjectileMult : MonoBehaviour
{
    [SerializeField]
    Spell prefab;
    [SerializeField]
    int numOfProjectiles;
    [SerializeField]
    float rotAngle = 0f;
    public string layerName;
    private void Awake()
    {
        gameObject.SetActive(false);

        layerName = LayerMask.LayerToName(gameObject.layer);
    }
    private void OnEnable()
    {
        
        
        Debug.LogWarning("OnEnable" + layerName);
        ObjectPool.ReturnObject(this.gameObject);
       
       Spawn(numOfProjectiles);
    }
   
    void Spawn(int n)
    {
        if (n % 2 == 0)
            transform.Rotate(0f, 0f, -rotAngle);
        else
            transform.Rotate(0f, 0f, -rotAngle * 2);

       

        for (int i = 0; i < n; i++)
        {
            layerName = LayerMask.LayerToName(gameObject.layer);
            Debug.LogWarning("LayerInLoop"+layerName);
            prefab.Attack(transform.position, transform.rotation, layerName);
           // GameObject spawnedObject =ObjectPool.SpawnObject(prefab, transform.position, transform.rotation, layerName);
           // spawnedObject.layer =gameObject.layer;
            transform.Rotate(0f, 0f, rotAngle);
        }
    }


}