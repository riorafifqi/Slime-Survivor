using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GoldManager : MonoBehaviour
{
    [SerializeField] Gold goldPrefab;
    [SerializeField] Transform goldParent;
    ObjectPool<Gold> goldPool;

    PlayerStats player;

    private void OnEnable()
    {
        WaveManager.OnEnemyDieE += OnEnemyDieHandler;
    }

    private void OnDisable()
    {
        WaveManager.OnEnemyDieE -= OnEnemyDieHandler;
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        goldPool = new(() =>
        {
            return Instantiate(goldPrefab, goldParent);
        }, _gold =>
        {
            _gold.gameObject.SetActive(true);
            _gold.Initialize(this);
        }, _gold =>
        {
            _gold.gameObject.SetActive(false);
        }, _gold =>
        {
            Destroy(_gold.gameObject);
        }, false, 100, 500);
    }

    public Gold SpawnGold()
    {
        return goldPool.Get();
    }

    public void DestroyGold(Gold gold)
    {
        goldPool.Release(gold);
    }

    public void OnEnemyDieHandler(Enemy enemy)
    {
        Gold _gold = SpawnGold();
        _gold.transform.position = enemy.transform.position;
    }
}
