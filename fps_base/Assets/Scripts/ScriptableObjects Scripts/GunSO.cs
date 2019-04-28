using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Gun", menuName = "Gun")]
public class GunSO : ScriptableObject
{
    #region Variables.
    #region Gun Chart header.
    [Header("Gun Chart")]
    #endregion
    #region Gun Info (Name, Description, Enums).
    [Header("Basic Info")]
    [Tooltip("Name of the Gun, used when hovering the gun.")]
    public new string name;
    [Tooltip("Basic description of the gun, don't make it too long.")]
    [TextArea]
    public string description;
    [HideInInspector]
    public enum GunType { AR, SMG, SR, DMR, SG, PBG, }
    [Tooltip("The type of the gun.")]
    public GunType gunType;
    [HideInInspector]
    public enum ShootingType { raycast, projectile, }
    [Tooltip("The type of shoot that the gun will use.")]
    public ShootingType shootingType;
    [HideInInspector]
    public enum FiringMode { semi, auto, burst, charged, buckshot, }
    [Tooltip("Select the firing modes that will be avaliable to the player.")]
    public FiringMode[] firingMode;
    [HideInInspector]
    public FiringMode currentFiringMode;
    #endregion
    #region Gun Properties.
    [Header("Properties")]
    [Tooltip("Base damage of the gun.")]
    public int dmg;
    [Tooltip("Effective range of the gun.")]
    public int range;
    [Tooltip("Fire rate of the weapon. Value in shoots per second.")]
    public int firerate;
    [Tooltip("The up recoil of the gun.")]
    [Range(0, 50)]
    public float verticalRecoil;
    [Tooltip("The sides recoil of the gun.")]
    [Range(0, 50)]
    public float horizontalRecoil;
    [Tooltip("Time that takes for the player to reload the gun. In seconds.")]
    public float reloadTime;
    [Tooltip("The maximum amount of bullets that can fit in the magazine.")]
    public int maxMagazineBullets;
    [Tooltip("The amount of magazines that player have at start. E.G. If the magazine fits 30 bullets, and you want the maximum amount of bullets avaliable at the start of the game to be 120, this should be 4")]
    public int startingMagazines;
    [Tooltip("The number of pellets for the shootgun.")]
    public int numberOfPellets;

    // Private Variables: (Related to Gun Properties)
    [HideInInspector]
    public float criticalChance;               // The chance for the player to land a critical hit.
    [HideInInspector]
    public float criticalDamageRate;             // The value that will multiply the base dmg value when landing a critical hit.
    [HideInInspector]
    public int currentMagazineBullets;         // The amount of bullets that are currently avaliable in the magazine.
    [HideInInspector]
    public int currentTotalBullets;            // The amount of bullets that are currently avaliable in total.
    [HideInInspector]
    public bool isReloading;                   // Control variable that tell us if the player is reloading. Used to prevent shooting/reloading if the player is already reloading.
    [HideInInspector]
    public bool isShooting;                    // Control variable that tell us if te player is shooting. Used to prevent shooting/reloading if the player is already shooting (E.G. used to prevent fast firing in burst mode).
    [HideInInspector]
    public bool startHasLoadedBullets;         // Tell us either or not the Method has loaded the bullet when a gun is loaded for the first time.
    [HideInInspector]
    public bool hasReleasedTrigger;
    private int pFiringModeIndex = 0;              // This variable is the holder holder of the value and cannot be acessed externally, to acess it use firingModeIndex.
    [HideInInspector]
    public int firingModeIndex                 // This variable controls which firing mode is selected.
    {
        get
        {
            return pFiringModeIndex;
        }
        set
        {
            if (pFiringModeIndex < firingMode.Length - 1)
            {
                pFiringModeIndex = value;
            }
            else
            {
                pFiringModeIndex = 0;
            }
        }
    }
    #endregion
    #region Gun Visual.
    [Header("Visuals")]
    [Tooltip("The mesh of the gun that will be displayed")]
    public Mesh _gunMesh;
    [Tooltip("The material of the gun that will be displayed")]
    public Material _gunMaterial;
    [Tooltip("The bullet that will spawn")]
    public GameObject _bullet;
    #endregion
    #region Gun Extras.
    public bool debugMode;                     // Toggle debug mode for testing in editor mode.
    #endregion
    #endregion
}