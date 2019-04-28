using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{

    #region UI Variables.
    [Header("UI References")]
    [Tooltip("Select from the Canvas in the scene hierarchy, the text that will be used for the bullet count")]
    public Text displayBulletCountInfo;
    [Tooltip("Select from the Canvas in the scene hierarchy, the text that will be used for the ShootingType if the weapon have Debug mode = true")]
    public Text displayDebugShootingType;
    [Tooltip("Select from the Canvas in the scene hierarchy, the text that will be used for the Firing Mode if the weapon have Debug mode = true")]
    public Text displayDebugFiringMode;
    #endregion

    #region Gun Critical Rate & Chance.
    [Header("Gun Critical Rate & Chance")]
    [Tooltip("Define a value for the critical chance for the AR Type.")]
    [Range(0, 1)]
    private float arCriticalRate;
    [Tooltip("Define a value for the critical rate of the AR Type.")]
    private float arCriticalChance;
    #endregion

    #region Shoot Variables.
    private float shootDelay;
    #endregion

    #region Scriptable Object Related Variables.
    [Header("Scriptable Object Related Variables")]
    [HideInInspector]
    public GunSO scriptableObject;
    public GunSO[] _guns;
    private int gunsIndexValue = 0;                                 // Used for storing the data about which gun from the _guns array we're using.
    #endregion

    #region Reference Variables.
    private GameObject _fpscam;
    private GameObject _gunVisual;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private Transform _bulletSpawn;
    #endregion

    private void Start()
    {
        SetGun();
        SetScriptableObjectValues();
        LoadReferenceVariables();
        SetShootingVariables();
        LoadGunVisual();
    }

    private void ReloadGunInfos()
    {
        SetGun();
        LoadReferenceVariables();
        SetShootingVariables();
        LoadGunVisual();
    }

    private void SetScriptableObjectValues()
    {
        foreach (GunSO gun in _guns)
        {
            switch (gun.gunType)
            {
                case GunSO.GunType.AR:
                    gun.criticalChance = 0.1f;
                    gun.criticalDamageRate = 2.5f;
                    gun.currentMagazineBullets = gun.maxMagazineBullets;
                    gun.currentTotalBullets = gun.maxMagazineBullets * gun.startingMagazines;
                    Debug.Log(gun.firingModeIndex);
                    gun.currentFiringMode = gun.firingMode[0];
                    break;
                case GunSO.GunType.SMG:
                    gun.criticalChance = 0.3f;
                    gun.criticalDamageRate = 1.5f;
                    gun.currentMagazineBullets = gun.maxMagazineBullets;
                    gun.currentTotalBullets = gun.maxMagazineBullets * gun.startingMagazines;
                    gun.currentFiringMode = gun.firingMode[0];
                    break;
                case GunSO.GunType.SG:
                    gun.criticalChance = 0.2f;
                    gun.criticalDamageRate = 2f;
                    gun.currentMagazineBullets = gun.maxMagazineBullets;
                    gun.currentTotalBullets = gun.maxMagazineBullets * gun.startingMagazines;
                    gun.currentFiringMode = gun.firingMode[0];
                    break;
                case GunSO.GunType.DMR:
                    gun.criticalChance = 0.1f;
                    gun.criticalDamageRate = 1.2f;
                    gun.currentMagazineBullets = gun.maxMagazineBullets;
                    gun.currentTotalBullets = gun.maxMagazineBullets * gun.startingMagazines;
                    gun.currentFiringMode = gun.firingMode[0];
                    break;
                case GunSO.GunType.SR:
                    gun.criticalChance = 0.1f;
                    gun.criticalDamageRate = 4f;
                    gun.currentMagazineBullets = gun.maxMagazineBullets;
                    gun.currentTotalBullets = gun.maxMagazineBullets * gun.startingMagazines;
                    gun.currentFiringMode = gun.firingMode[0];
                    break;
                case GunSO.GunType.PBG:
                    gun.criticalChance = 0.1f;
                    gun.criticalDamageRate = 10f;
                    gun.currentMagazineBullets = gun.maxMagazineBullets;
                    gun.currentTotalBullets = gun.maxMagazineBullets * gun.startingMagazines;
                    gun.currentFiringMode = gun.firingMode[0];
                    break;
            }
        }
    }

    private void LoadReferenceVariables()
    {
        _fpscam = GameObject.FindGameObjectWithTag("MainCamera");
        _gunVisual = _fpscam.transform.GetChild(0).gameObject;
        _meshFilter = _gunVisual.GetComponent<MeshFilter>();
        _meshRenderer = _gunVisual.GetComponent<MeshRenderer>();
        _bulletSpawn = _gunVisual.transform.GetChild(0);
    }

    private void LoadGunVisual()
    {
        _meshFilter.mesh = scriptableObject._gunMesh;
        _meshRenderer.material = scriptableObject._gunMaterial;
    }

    private void SetShootingVariables()
    {
        shootDelay = Time.time + 60 / scriptableObject.firerate;
    }

    private void SetGun()
    {
        scriptableObject = _guns[gunsIndexValue];
    }

    private void Update()
    {
        PlayerGunInputs();
        ManageAndUpdateUI();
    }

    private void ManageAndUpdateUI()
    {
        string bulletInfo = scriptableObject.currentMagazineBullets.ToString() + " / " + scriptableObject.currentTotalBullets.ToString();
        displayBulletCountInfo.text = bulletInfo;
        displayDebugShootingType.text = scriptableObject.shootingType.ToString();
        displayDebugFiringMode.text = scriptableObject.currentFiringMode.ToString();
    }

    private void ReloadGun()
    {
        int amountToReload = scriptableObject.maxMagazineBullets - scriptableObject.currentMagazineBullets;
        if (scriptableObject.currentTotalBullets >= amountToReload)
        {
            scriptableObject.currentMagazineBullets = scriptableObject.currentMagazineBullets + amountToReload;
            scriptableObject.currentTotalBullets = scriptableObject.currentTotalBullets - amountToReload;
        }
        else if (scriptableObject.currentTotalBullets > 0 && scriptableObject.currentTotalBullets < amountToReload)
        {
            amountToReload = scriptableObject.currentTotalBullets;
            scriptableObject.currentMagazineBullets = scriptableObject.currentMagazineBullets + amountToReload;
            scriptableObject.currentTotalBullets = 0;
        }
        else
        {
            Debug.Log("Acabou bala R.I.P");
        }

        /* Metodo avançado.
        scriptableObject.currentTotalBullets = scriptableObject.currentMagazineBullets + scriptableObject.currentTotalBullets;
        scriptableObject.currentMagazineBullets = scriptableObject.currentTotalBullets >= scriptableObject.maxMagazineBullets ? scriptableObject.maxMagazineBullets : scriptableObject.currentTotalBullets;
        scriptableObject.currentTotalBullets = scriptableObject.currentTotalBullets - scriptableObject.currentMagazineBullets;
        */
    }

    private void PlayerGunInputs()
    {
        if (Input.GetButton("Fire1") && Time.time >= shootDelay && scriptableObject.currentMagazineBullets > 0)
        {
            switch (scriptableObject.currentFiringMode)
            {
                case GunSO.FiringMode.auto:
                    shootDelay = Time.time + 60f / scriptableObject.firerate;
                    Shoot();
                    break;
                case GunSO.FiringMode.semi:
                case GunSO.FiringMode.buckshot:
                case GunSO.FiringMode.charged:
                    // Uses one more verification to disable autofiring
                    if (scriptableObject.hasReleasedTrigger)
                    {
                        shootDelay = Time.time + 60f / scriptableObject.firerate;
                        Shoot();
                        scriptableObject.hasReleasedTrigger = false;
                    }
                    break;
                case GunSO.FiringMode.burst:
                    if (scriptableObject.hasReleasedTrigger)
                    {
                        shootDelay = Time.time + (60f / scriptableObject.firerate) * 3;
                        Shoot();
                        scriptableObject.hasReleasedTrigger = false;
                    }
                    break;
            }
        }
        if (Input.GetButtonUp("Fire1"))
        {
            scriptableObject.hasReleasedTrigger = true;
        }
        if (Input.GetButtonDown("Reload"))
        {
            ReloadGun();
        }
        if (Input.GetButtonDown("Swap"))
        {
            //gunsIndexValue = gunsIndexValue < _guns.Length ? gunsIndexValue++ : gunsIndexValue = 0;
            if (gunsIndexValue < _guns.Length - 1)
                gunsIndexValue++;
            else
                gunsIndexValue = 0;
            ReloadGunInfos();
            Debug.Log("Swap");
        }
        if (Input.GetButtonDown("ChangeFiringMode"))
        {
            scriptableObject.firingModeIndex++;
            scriptableObject.currentFiringMode = scriptableObject.firingMode[scriptableObject.firingModeIndex];
        }
        
    }

    private void Shoot()
    {
        switch (scriptableObject.currentFiringMode)
        {
            case GunSO.FiringMode.charged:
            case GunSO.FiringMode.semi:
            case GunSO.FiringMode.auto:
                ShootRaycast(false);
                SubtractMagazineBullets(1);
                break;
            case GunSO.FiringMode.buckshot:
                for (int i = 0; i < scriptableObject.numberOfPellets; i++)
                {
                    ShootRaycast(true);
                }
                SubtractMagazineBullets(1);
                break;
            case GunSO.FiringMode.burst:
                StartCoroutine("BurstShoot");
                break;
        }
    }

    private IEnumerator BurstShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            ShootRaycast(false);
            SubtractMagazineBullets(1);
            yield return new WaitForSeconds(60 / (scriptableObject.firerate *1.25f));
        }
    }

    private void ShootRaycast(bool spread)
    {
        RaycastHit hit;
        if (!spread)
        {
            Debug.DrawRay(_fpscam.transform.position, _fpscam.transform.TransformDirection(Vector3.forward) * scriptableObject.range, Color.blue, 0.05f);
            if (Physics.Raycast(_fpscam.transform.position, _fpscam.transform.forward, out hit, 100f))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<EnemyScript>().ApplyDamage(scriptableObject.dmg);
                }
            }
            Instantiate(scriptableObject._bullet, _bulletSpawn.transform.position, _bulletSpawn.transform.rotation);
        }
        else
        {
            Vector3 bulletSpread = new Vector3(Random.value * 2f, Random.value * 2f, Random.value * 2f);
            Debug.DrawRay(_fpscam.transform.position, (_fpscam.transform.transform.forward * scriptableObject.range) + bulletSpread, Color.blue, 10f);
            if (Physics.Raycast(_fpscam.transform.position, _fpscam.transform.forward + bulletSpread, out hit, 100f))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<EnemyScript>().ApplyDamage(scriptableObject.dmg);
                }
            }
            var b = Instantiate(scriptableObject._bullet, _bulletSpawn.transform.position, _bulletSpawn.transform.rotation);
            b.transform.Rotate(bulletSpread.x * 2, bulletSpread.y * 2, bulletSpread.z * 2);
            Destroy(b.gameObject, .3f);
        }
    }

    private void SubtractMagazineBullets(int shootsFired)
    {
        if (scriptableObject.currentMagazineBullets > 0)
            scriptableObject.currentMagazineBullets -= shootsFired;
    }

    /*
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
    */
}