using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Single_Directional : Enemy
{
    [SerializeField] Vector2 moveDirection;

    public override void Setup()
    {
        base.Setup();

        base.moveDirection = moveDirection;
    }

/*    public override IEnumerator Moving()
    {
        while (true)
        {
            transform.Translate(base.moveDirection * speed * Time.deltaTime);

            FlipSprite();

            yield return new WaitForEndOfFrame();
        }
    }*/
}
