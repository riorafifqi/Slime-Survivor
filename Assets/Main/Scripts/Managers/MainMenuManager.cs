using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] AudioSource mainMenuBGM;

    private void Start()
    {
        mainMenuBGM.Play();
    }
}
