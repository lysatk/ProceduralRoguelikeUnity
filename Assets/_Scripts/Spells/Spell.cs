using Assets._Scripts.Utilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spell")]
public class Spell : ScriptableObject
{
    /// <summary> Spell name </summary>
    public string Name;
    /// <summary> Spell description </summary>
    public string Description;
    /// <summary> Spell icon </summary>
    public Sprite image;
    /// <summary> Spell prefab (MonoBehaviour)</summary>
    public GameObject Prefab; 

    /// <summary> Spell damage</summary>
    public float spellDamage;
    /// <summary> Spell conditions for example burn enemy </summary>
    public List<ConditionBase> conditions;
    /// <summary> Spell Speed </summary>
    public float speed;
    /// <summary> Spell live time </summary>
    public float destroyTime;
    /// <summary> Spell caster</summary>
    public Collider2D caster;

    /// <summary> Cast from playerCharacter or from staffPoint</summary>
    public bool CastFromHeroeNoStaff = false;
    /// <summary> Spell cooldown </summary>
    public float cooldown;
    /// <summary> Spell Slot</summary>
    public SpellSlot spellSlot;

    /// <summary>position and rotation to create spell prefab</summary>
    /// <param name="position">Spawn position</param>
    /// <param name="rotation">Spawn rotation</param>
    public void Attack(Vector3 position, Quaternion rotation,string layerString,ObjectPool.SpellSource spellSource=ObjectPool.SpellSource.None)
    {
        Debug.LogWarning("Initial:"+layerString);
      var obj= ObjectPool.SpawnObject(Prefab, position, rotation,layerString,spellSource); 
      obj.layer=LayerMask.NameToLayer(layerString);
        Debug.Log("Layer of the object final:"+obj.layer);
    }

    //public void AttackMult(Vector3 position, Quaternion rotation, string layerString, Spell spellToMult, int numOfProjectiles, float rotAngle, ObjectPool.SpellSource spellSource = ObjectPool.SpellSource.None)
    //{
    //    Debug.LogWarning("Initial:" + layerString);
    //    // Spawn the initial object
    //    GameObject obj = ObjectPool.SpawnObject(Prefab, position, rotation, layerString, spellSource);

    //    // Adjust rotation based on the number of projectiles
    //    Quaternion initialRotation = rotation;
    //    if (numOfProjectiles % 2 == 0)
    //    {
    //        initialRotation *= Quaternion.Euler(0f, 0f, -rotAngle);
    //    }
    //    else
    //    {
    //        initialRotation *= Quaternion.Euler(0f, 0f, -rotAngle * 2);
    //    }

    //    for (int i = 0; i < numOfProjectiles; i++)
    //    {
    //        Debug.LogWarning("LayerInLoop: " + layerString);
    //        // Use the modified rotation for spawning projectiles
    //        Quaternion projectileRotation = initialRotation * Quaternion.Euler(0f, 0f, rotAngle * i);
    //        spellToMult.Attack(position, projectileRotation, layerString);

    //        // Note: The following commented-out lines seem redundant or incorrect,
    //        // as they refer to 'transform', which might not be related to the current context.
    //        // GameObject spawnedObject = ObjectPool.SpawnObject(prefab, position, projectileRotation, layerString);
    //        // spawnedObject.layer = gameObject.layer;
    //    }

    //    Debug.Log("Layer of the initial object final: " + LayerMask.LayerToName(obj.layer));
    //}


    
}