using System.Collections;
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
    public float maxBullets;
    [HideInInspector]
    public float actualBullets;
    public float maxMagazineBullets;
    [HideInInspector]
    public float magazineBullets;

    public enum WeaponType {AR, SMG, SR, DMR, SG, }
    public WeaponType weaponType;
    [HideInInspector]
    public enum ShootingType {raycast, projectile, }
    [HideInInspector]
    public ShootingType shootingType;

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
        actualBullets = maxBullets;
        magazineBullets = maxMagazineBullets;
        if (weaponType == WeaponType.SR)
        {
            fireRate = 0;
        }
        if (_bulletPrefab != null)
        {
            
        }
    }

    private void Awake()
    {
        _fpscam = GameObject.FindGameObjectWithTag("MainCamera");
        _gunVisual = GameObject.FindGameObjectWithTag("GunVisual");
        _gunFilter = _gunVisual.GetComponent<MeshFilter>();
        _gunFilter.mesh = _gunMesh;
        _gunRenderer = _gunVisual.GetComponent<MeshRenderer>();
        _gunRenderer.material = _gunMat;
    }

    private void OnEnable()
    {
        
    }

    public void Shoot(ShootingType st)
    {
        Debug.Log("Shoot Method Running");
        switch (shootingType)
        {
            case ShootingType.raycast:
                Debug.Log("ShootRaycast running");
                RaycastHit hit;
                if (debugActive)
                    Debug.DrawLine(_fpscam.transform.position, _fpscam.transform.forward * 100f, Color.yellow, 0.25f);
                if (Physics.Raycast(_fpscam.transform.position, _fpscam.transform.forward, out hit, 100f))
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

}
