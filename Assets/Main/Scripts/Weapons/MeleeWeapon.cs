using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    //[SerializeField] Vector2 areaSize;
    [SerializeField] float radius;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector2 offset = Vector2.zero;
    [SerializeField] AudioSource hitSfx;

    private void OnEnable()
    {
        StartCoroutine(Attack());
    }

    public override IEnumerator Attack(Enemy enemy = null)
    {
        //Collider2D[] hits = Physics2D.OverlapBoxAll(this.transform.position, areaSize, 0f, enemyLayer);
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)transform.position + offset, radius, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            hit.GetComponent<Enemy>().TakeDamage(damage);
        }

        hitSfx.Play();

        yield return new WaitForSeconds(0.1f);

        this.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((Vector2)transform.position + offset, radius);
        //Gizmos.DrawWireCube((Vector2)transform.position + (Vector2)(transform.parent.rotation * offset), areaSize);
    }
}
