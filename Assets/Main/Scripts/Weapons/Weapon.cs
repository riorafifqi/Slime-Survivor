using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] public float attackSpeed;
    [SerializeField] protected int damage;

    public abstract IEnumerator Attack(Enemy enemy = null);
}
