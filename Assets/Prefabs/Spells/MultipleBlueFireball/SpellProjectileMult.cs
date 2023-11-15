using UnityEngine;

public class SpellProjectileMult : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int numOfProjectiles;
    [SerializeField]
    float rotAngle = 0f;
    string layerName;
    private void OnEnable()
    {
        layerName = LayerMask.LayerToName(gameObject.layer);
        Spawn(numOfProjectiles);
        
        ObjectPool.ReturnObject(this.gameObject);
    }

    void Spawn(int n)
    {
        if (n % 2 == 0) transform.Rotate(0f, 0f, -rotAngle);

        else transform.Rotate(0f, 0f, -rotAngle * 2);

        for (int i = 0; i < n; i++)
        {

            ObjectPool.SpawnObject(prefab, transform.position, transform.rotation,layerName);
            transform.Rotate(0f, 0f, rotAngle);
        }
    }

}