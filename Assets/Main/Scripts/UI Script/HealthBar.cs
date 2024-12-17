using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Enemy enemy;

    [SerializeField] Image healthfill;
    [SerializeField] Vector3 offset = new Vector3(0f, 0.5f, 0f);

    public void Initialize(Enemy enemy)
    {
        this.enemy = enemy;
        StartCoroutine(UpdateHealthBar());
    }

    public void Terminate()
    {
        this.enemy = null;
        //StopAllCoroutines();
    }

    public void SetHealth()
    {
        float enemyHealthPercentage = enemy.GetHealthPercentage();
        healthfill.fillAmount = enemyHealthPercentage;
    }

    private void Update()
    {
        SetHealth();
    }

    public IEnumerator UpdateHealthBar()
    {
        while (enemy != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
            this.transform.position = screenPos + offset;

            yield return new WaitForEndOfFrame();
        }
    }
}
