using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectileManager : MonoBehaviour
{
    ObjectPool<Projectile> projectilePool;

    [SerializeField] Projectile projectilePrefab;
    
    PlayerStats playerStats;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();

        projectilePool = new(() =>
        {
            return Instantiate(projectilePrefab);
        }, _projectile =>
        {
            _projectile.gameObject.SetActive(true);
            _projectile.Initialize(this, playerStats);
        }, _projectile =>
        {
            _projectile.gameObject.SetActive(false);
        }, _projectile =>
        {
            Destroy(_projectile.gameObject);
        }, false, 100, 500);
    }

    public void SpawnProjectile(Vector2 spawnPoint, Vector2 direction, int damage)
    {
        Projectile _projectile = projectilePool.Get();
        _projectile.transform.position = spawnPoint;
        StartCoroutine(_projectile.Shoot(direction, damage));
    }

    public void DestroyProjectile(Projectile p)
    {
        projectilePool.Release(p);
    }
}
