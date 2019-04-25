using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public Text displayBulletInfo;
    public Text displayDebugShootingMode;
    public Text displayDebugFiringMode;
    [HideInInspector]
    public GunSO gunSO;
    public GunSO[] _guns = new GunSO[3];
    private int gunIndex;
    private int pGunIndex
    {
        get
        {
            return gunIndex;
        }
        set
        {
            if (gunIndex >= _guns.Length - 1)
            {
                gunIndex = 0;
            }
            else
            {
                gunIndex = value;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _guns.Length; i++)
        {
            _guns[i].hasLoaded = false;
        }
        gunSO = _guns[pGunIndex];
        gunSO.LoadParametersValue();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForAllInputs();
        DisplayHudInfo();
        gunSO.AutoCheck();
    }

    // Add all input check methods in this method
    void CheckForAllInputs()
    {
        CheckForFiringInput();
        CheckForGunChangeInput();
        CheckForReloadInput();
        CheckForFiringModeInput();
    }

    void CheckForFiringInput()
    {
        if (Input.GetButton("Fire1") && !gunSO.isReloading)
        {
            gunSO.Shoot();
            Debug.Log("Passed Input Check");
        }
        if (Input.GetButtonUp("Fire1"))
        {
            gunSO.semiCanShoot = true;
        }
    }

    void CheckForGunChangeInput()
    {
        if (Input.GetButtonDown("Swap"))
        {
            Debug.Log("Input Confirmado para Swap");
            pGunIndex += 1;
            gunSO = _guns[gunIndex];
            gunSO.LoadParametersValue();
            DisplayHudInfo();
        }
    }

    void CheckForFiringModeInput()
    {
        if (Input.GetButtonDown("ChangeFiringMode"))
        {
            Debug.Log("Input Confirmado para ChangeFiringMode");
        } 
    }

    public void CheckForReloadInput()
    {
        if (Input.GetButtonDown("Reload"))
        {
            Debug.Log("Input Confirmado para Reload");
            if (!gunSO.isReloading)
            {
                gunSO.ReloadGun();
            }
        }
    }

    public void DisplayHudInfo()
    {
        displayBulletInfo.text = gunSO.actualMagazineBullets.ToString() + " / " + gunSO.actualTotalBullets.ToString();
        displayDebugShootingMode.text = gunSO.shootingType.ToString();
        displayDebugFiringMode.text = gunSO.activeFiringMode.ToString();
    }

    public void StartBurstFire()
    {
        if (!gunSO.isBurstFiring)
        {
            gunSO.isBurstFiring = true;
            IEnumerator coroutine = gunSO.FireBurstShoot();
            StopCoroutine(coroutine);
            StartCoroutine(coroutine);
        }
    }

}