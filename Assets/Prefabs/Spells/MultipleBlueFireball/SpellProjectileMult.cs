using UnityEngine;

public class SpellProjectileMult : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    int numOfProjectiles;
    [SerializeField]
    float rotAngle = 0f;
    private void Awake()
    {
        Spawn(numOfProjectiles);
        Destroy(this.gameObject);
    }

    void Spawn(int n)
    {
        if (n % 2 == 0) transform.Rotate(0f, 0f, -rotAngle);

        else transform.Rotate(0f, 0f, -rotAngle * 2);

        for (int i = 0; i < n; i++)
        {

            ObjectPool.SpawnObject(prefab, transform.position, transform.rotation);
            transform.Rotate(0f, 0f, rotAngle);
        }
    }

}