using Assets._Scripts.Spells;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool : StaticInstance<GameManager>
{
    [SerializeField] private SpellProjectileBase _gameObject;
    [SerializeField] private int _poolSize = 20;

    private ObjectPool<SpellProjectileBase> _pool;

    private void Start()
    {
        _pool = new ObjectPool<SpellProjectileBase>(() =>
        {
            return Instantiate(_gameObject);
        }, spell =>
        {
            spell.gameObject.SetActive(true);
        }, spell =>
        {
            spell.gameObject.SetActive(false);
        }, spell =>
        {
            Destroy(spell.gameObject);
        }, false, 100, 1000);
    }


}
