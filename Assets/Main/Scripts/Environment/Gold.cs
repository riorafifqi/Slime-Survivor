using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Gold : MonoBehaviour
{
    GoldManager goldManager;
    public bool canBePicked;

    public void Initialize(GoldManager goldManager)
    {
        this.goldManager = goldManager;
        canBePicked = true;
    }

    public void OnPickup(PlayerStats player)
    {
        canBePicked = false;
        this.transform.DOMove(player.transform.position, 0.1f).SetEase(Ease.Linear).OnComplete(() => {

            player.AddGold();

            goldManager.DestroyGold(this);
        });
    }
}
