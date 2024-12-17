using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Mathematics;
using Unity.Jobs;

public class HealthBarManager : MonoBehaviour
{
    ObjectPool<HealthBar> healthBarPool;

    [SerializeField] HealthBar healthBarPrefab;
    [SerializeField] Transform healthBarCollection;

    void Start()
    {
        healthBarPool = new(() =>
        {
            return Instantiate(healthBarPrefab, healthBarCollection);
        }, healthBar =>
        {
            healthBar.gameObject.SetActive(true);
        }, healthBar =>
        {
            healthBar.gameObject.SetActive(false);
        }, healthBar =>
        {
            Destroy(healthBar.gameObject);
        }, false, 100, 500);
    }

    public HealthBar GetHealthBar()
    {
        return healthBarPool.Get();
    }

    public void DestroyHealthBar(HealthBar healthBar)
    {
        healthBar.Terminate();
        healthBarPool.Release(healthBar);
    }
}