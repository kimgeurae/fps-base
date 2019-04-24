﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Gun", menuName = "Gun")]
public class GunSO : ScriptableObject
{
    [Header("Gun Info")]
    public new string name;
    [TextArea] // Spacement
    public string description;

    [Header("Properties")]
    public int dmg;
    public float critChance;
    public int critDmgRate;
    public int range;
    public float fireRate; // 0 means semi-auto
    public float verticalRecoil;
    public float horizontalRecoil;
    public float reloadTime;
    [HideInInspector]
    public int actualMagazineBullets = 0;
    [HideInInspector]
    public int actualTotalBullets = 0;
    public int maxMagazineBullets;
    public int maxTotalBullets;
    public bool isReloading = false;
    public bool hasLoaded = false;

    public enum WeaponType { AR, SMG, SR, DMR, SG, PBG }
    public WeaponType weaponType;
    [HideInInspector]
    public enum ShootingType { raycast, projectile, }
    [HideInInspector]
    public ShootingType shootingType;
    public enum FiringMode { semi, auto, burst, charged, buckshot, }

    [Header("Visuals")]
    public GameObject _gunVisual;
    public Mesh _gunMesh;
    public Material _gunMat;
    [HideInInspector]
    public MeshFilter _gunFilter;
    [HideInInspector]
    public MeshRenderer _gunRenderer;

    [Header("References")]
    public Transform _gunBarrelEnd;
    public GameObject _bulletPrefab;
    public GameObject _fpscam;

    [Header("Debug")]
    public bool debugActive;

    public GunSO()
    {
        Debug.Log("Called Construtor");
    }

    private void Awake()
    {
        Debug.Log("Called Awake");
    }

    private void OnEnable()
    {
        Debug.Log("Called OnEnable");
    }

    public void Shoot()
    {
        Debug.Log("Shoot Method Running");
        switch (shootingType)
        {
            case ShootingType.raycast:
                if (actualMagazineBullets > 0)
                {
                    Debug.Log("ShootRaycast running");
                    RaycastHit hit;
                    if (debugActive)
                        Debug.DrawLine(_fpscam.transform.position + Vector3.forward * 0.6f, _fpscam.transform.forward * range, Color.cyan, 0.25f);
                    if (Physics.Raycast(_fpscam.transform.position + Vector3.forward * 0.6f, _fpscam.transform.forward, out hit, range))
                    {
                        if (hit.transform.gameObject.CompareTag("Enemy"))
                        {
                            bool isCrit = Random.value < critChance;
                            if (isCrit)
                                hit.transform.gameObject.GetComponent<EnemyScript>().LocalApplyDamage(dmg * critDmgRate);
                            else
                                hit.transform.gameObject.GetComponent<EnemyScript>().LocalApplyDamage(dmg);
                        }
                    }
                    LowerMagazineBullets(1);
                }
                break;
            case ShootingType.projectile:
                Instantiate(_bulletPrefab, _gunBarrelEnd.position, Quaternion.identity);
                break;
        }
    }

    public int Reload(int a)
    {
        return a;
    }

    public void LoadParametersValue()
    {
        _fpscam = GameObject.FindGameObjectWithTag("MainCamera");
        _gunVisual = GameObject.FindGameObjectWithTag("GunVisual");
        _gunFilter = _gunVisual.GetComponent<MeshFilter>();
        _gunFilter.mesh = _gunMesh;
        _gunRenderer = _gunVisual.GetComponent<MeshRenderer>();
        _gunRenderer.material = _gunMat;
        if (!hasLoaded)
            actualTotalBullets = maxTotalBullets;
        if (!hasLoaded)
            actualMagazineBullets = maxMagazineBullets;
        if (weaponType == WeaponType.SR)
        {
            fireRate = 0;
        }
        if (_bulletPrefab != null)
        {

        }
        hasLoaded = true;
    }

    private void LowerMagazineBullets(int shootsFired)
    {
        Debug.Log("LowerMagazineBullets method fired");
        if (actualMagazineBullets > 0)
        {
            Debug.Log("Magazine Bullets was lowered");
            Debug.Log(actualMagazineBullets);
            actualMagazineBullets -= shootsFired;
        }
    }

    public void ReloadGun()
    {
        if (actualMagazineBullets == 0)
        {
            if (actualTotalBullets > maxMagazineBullets)
            {
                actualMagazineBullets = maxMagazineBullets;
                actualTotalBullets -= maxMagazineBullets;
            }
            else
            {
                actualMagazineBullets = actualTotalBullets;
                actualTotalBullets = 0;
            }
        }
        else
        {
            if (actualTotalBullets > maxMagazineBullets - actualMagazineBullets)
            {
                int valueToBeSubtracted = maxMagazineBullets - actualMagazineBullets;
                actualMagazineBullets = maxMagazineBullets;
                actualTotalBullets -= valueToBeSubtracted;
            }
            else
            {
                actualMagazineBullets += actualTotalBullets;
                actualTotalBullets = 0;
            }
        }
    }

}