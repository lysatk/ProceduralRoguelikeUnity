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
    public void Attack(Vector3 position, Quaternion rotation,string layerString)
    {
       ObjectPool.SpawnObject(Prefab, position, rotation,layerString); 
      
    }
}