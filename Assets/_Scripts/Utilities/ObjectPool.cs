using Assets._Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class ObjectPool : StaticInstance<GameManager>
{
    [SerializeField] private SpellProjectileBase _gameObject;


    static public ObjectPool<SpellProjectileBase> _pool;

    private void Start()
    {
   
        _pool = new ObjectPool<SpellProjectileBase>(CreateSpell, OnBorrowSpell, OnReturnSpell, OnDestroySpell, false, 100, 1000);
    }

    static public void BorrowSpellFromPool(Vector3 position, Quaternion rotation)
    {
        SpellProjectileBase spell = _pool.Get();

      
        spell.transform.position = position;
        spell.transform.rotation = rotation;
    }

    private SpellProjectileBase CreateSpell()
    {
        
        var spell = Instantiate(_gameObject);

      
        // spell.SpellAwake(); 

        return spell;
    }

    private void OnBorrowSpell(SpellProjectileBase spell)
    {

        // spell.transform.position = position;
        // spell.transform.rotation = rotation;

        spell.gameObject.SetActive(true);
    }

    private void OnReturnSpell(SpellProjectileBase spell)
    {
 
        spell.gameObject.SetActive(false);
    }

    private void OnDestroySpell(SpellProjectileBase spell)
    {
        Destroy(spell.gameObject);
    }
}
