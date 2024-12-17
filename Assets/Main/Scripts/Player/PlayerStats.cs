using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float PickUpRange { get; private set; }
    public float Speed { get; private set; }
    public int Gold { get; private set; }
    public int Level { get; private set; }

    [SerializeField] private int maxHealth;
    [SerializeField] private float pickUpRange;
    [SerializeField] private float speed;
    [SerializeField] private float invicibleDuration = 1f;

    [Header("UI")]
    [SerializeField] TMP_Text healthText;
    [SerializeField] Image healthFillImage;
    [SerializeField] TMP_Text goldText;

    [Header("SFX")]
    [SerializeField] AudioSource hitSfx;
    [SerializeField] AudioSource coinSfx;

    // Events
    public static Action OnPlayerDieE;
    public static Action<int> OnPlayerTakeDamageE;

    bool isVulnerable;

    private void OnEnable()
    {
        OnPlayerTakeDamageE += TakeDamage;
    }

    private void OnDisable()
    {
        OnPlayerTakeDamageE -= TakeDamage;
    }

    private void Start()
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
        PickUpRange = pickUpRange;
        Speed = speed;
        Gold = 0;
        Level = 1;
        isVulnerable = true;

        SetHealthUI();
        goldText.text = Gold.ToString();
    }

    public void TakeDamage(int damage)
    {
        hitSfx.Play();

        CurrentHealth -= damage;
        //isVulnerable = false;
        SetHealthUI();


        if (CurrentHealth <= 0)
        {
            Die();
            StopAllCoroutines();
        }

        //StartCoroutine(InvicibleTimer());
    }

    public IEnumerator InvicibleTimer()
    {
        yield return new WaitForSeconds(invicibleDuration);

        isVulnerable = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, pickUpRange);
    }

    private void Die()
    {
        OnPlayerDieE?.Invoke();
    }

    public void AddGold()
    {
        Gold++;
        coinSfx.Play();
        goldText.text = Gold.ToString();
    }

    public void SetHealthUI()
    {
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }

        healthText.text = CurrentHealth.ToString() + "/" + MaxHealth.ToString();
        healthFillImage.fillAmount = (float)CurrentHealth / (float)MaxHealth;
    }
}