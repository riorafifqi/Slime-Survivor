using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] int maxHealth;
    int currentHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected float attackRange;
    protected Vector2 moveDirection;
    [SerializeField] protected int attackDamage;
    [SerializeField] LayerMask enemyLayerMask;
    [SerializeField] protected float attackCooldown;
    [SerializeField] protected float lifeTime = 0;      // 0 to disable lifeTime
    [SerializeField] bool isSingleDirectional = false;
    protected bool canAttack = true;    

    protected PlayerStats player;
    SpriteRenderer spriteRenderer;
    BoxCollider2D collider;
    Animator animator;

    HealthBarManager healthBarManager;
    HealthBar activeHealthBar;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    public virtual void Setup()
    {
        //StartCoroutine(Moving());

        StartCoroutine(CountLifetime());
    }

    public void Initialize(PlayerStats _player, HealthBarManager _healthBar)
    {
        player = _player;
        healthBarManager = _healthBar;

        currentHealth = maxHealth;

        Setup();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < attackRange)
        {
            Attack();
        }
    }

    /*public virtual IEnumerator Moving()
    {
        // Move to player
        while (true)
        {
            moveDirection = player.transform.position;

            transform.position = Vector2.MoveTowards(transform.position, moveDirection, speed * Time.deltaTime);

            FlipSprite();

            yield return new WaitForEndOfFrame();
        }
    }*/

    private IEnumerator CountLifetime()
    {
        if (lifeTime <= 0)
        {
            yield break;
        }

        yield return new WaitForSeconds(lifeTime);

        Die();
    }

    public void FlipSprite()
    {
        spriteRenderer.flipX = player.transform.position.x > transform.position.x? true : false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        WaveManager.OnEnemyDieE(this);
    }

    public virtual void Attack()
    {
        if (canAttack)
        {
            PlayerStats.OnPlayerTakeDamageE?.Invoke(attackDamage);

            canAttack = false;

            StartCoroutine(StartAttackCooldown());
        }
    }

    public IEnumerator StartAttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.left * 0.75f);
    }

    private void OnBecameInvisible()
    {
        // Destroy health
        healthBarManager.DestroyHealthBar(activeHealthBar);
        activeHealthBar = null;
    }

    private void OnBecameVisible()
    {
        activeHealthBar = healthBarManager.GetHealthBar();
        activeHealthBar.Initialize(this);
    }

    public float GetHealthPercentage()
    {
        float healthPercentage = (float)currentHealth / (float)maxHealth;
        return healthPercentage;
    }

    public string GetName()
    {
        return name;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public bool GetMoveMethod()
    {
        return isSingleDirectional;
    }

    public Vector2 GetMoveTarget()
    {
        return moveDirection;
    }
}