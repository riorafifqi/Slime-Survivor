using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ranged : Enemy
{
    ProjectileManager projectileManager;

    public override void Setup()
    {
        if (!projectileManager)
            projectileManager = FindObjectOfType<ProjectileManager>();
        
        base.Setup();
    }

    public override IEnumerator Moving()
    {
        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            FlipSprite();

            Attack();

            yield return new WaitForEndOfFrame();
        }
    }


    public override void Attack()
    {
        if (canAttack)
        {
            Vector2 direction = player.transform.position - transform.position;

            projectileManager.SpawnProjectile(this.transform.position, direction, attackDamage);
            
            canAttack = false;

            StartCoroutine(StartAttackCooldown());
        }
    }
}
