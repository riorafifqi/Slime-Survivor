using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    PlayerStats playerStats;
    [SerializeField] LayerMask itemLayer;

    Collider2D[] overlapItems = new Collider2D[100];

    private void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        StartCoroutine(CheckSuroundingItems());
    }

    IEnumerator CheckSuroundingItems()
    {
        while (true)
        {
            int hitCount = Physics2D.OverlapCircleNonAlloc(this.transform.position, playerStats.PickUpRange, overlapItems, itemLayer);
            for (int i = 0; i < hitCount; i++)
            {
                Collider2D item = overlapItems[i];

                if (item.TryGetComponent(out Gold _gold))
                {
                    if (!_gold.canBePicked)
                        continue;

                    Debug.Log(item);
                    _gold.OnPickup(playerStats);
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}
