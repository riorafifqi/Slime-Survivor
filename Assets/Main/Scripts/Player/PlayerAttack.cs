using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    Joystick joystick;
    Weapon[] weapons;
    
    [SerializeField] Transform weaponsParent;

    private void Start()
    {
        joystick = FindObjectOfType<Joystick>();
        weapons = weaponsParent.GetComponentsInChildren<Weapon>(true);

        for (int i = 0; i < weapons.Length; i++) 
        {
            weapons[i].gameObject.SetActive(false);
            StartCoroutine(StartWeapon(weapons[i]));
        }
    }

    private void Update()
    {
        SetDirection();
    }

    private void SetDirection()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            float angle = Vector2.SignedAngle(Vector2.up, new Vector2(joystick.Horizontal, joystick.Vertical));
            weaponsParent.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private IEnumerator StartWeapon(Weapon weapon)
    {
        while (true)
        {
            weapon.gameObject.SetActive(true);

            yield return new WaitForSeconds(weapon.attackSpeed);
        }
    }
}