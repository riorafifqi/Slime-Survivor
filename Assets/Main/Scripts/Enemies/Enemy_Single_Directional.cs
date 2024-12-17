using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Single_Directional : Enemy
{
    [SerializeField] Vector2 moveDirection;
    public override IEnumerator Moving()
    {
        while (true)
        {
            transform.Translate(moveDirection.normalized * speed * Time.deltaTime);

            FlipSprite();

            if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
            {
                Attack();
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
