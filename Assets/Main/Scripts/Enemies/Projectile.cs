using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    ProjectileManager projectileManager;
    PlayerStats playerStats;

    [SerializeField] float lifeTime;
    [SerializeField] float speed;

    public void Initialize(ProjectileManager projectileManager, PlayerStats playerStats)
    {
        this.projectileManager = projectileManager;
        this.playerStats = playerStats;
    }

    public IEnumerator Shoot(Vector2 direction, int damage)
    {
        float elapsedTime = 0f;
        RotateToDirection(direction);

        while (elapsedTime < lifeTime)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);

            if (Vector2.Distance(playerStats.transform.position, this.transform.position) <= 0.5f)
            {
                PlayerStats.OnPlayerTakeDamageE?.Invoke(damage);
                break;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        projectileManager.DestroyProjectile(this);
    }

    public void RotateToDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, angle);
    }
}
