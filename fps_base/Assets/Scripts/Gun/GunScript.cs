using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GunSO gunSO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForFiringInput();
    }

    void CheckForFiringInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gunSO.Shoot(gunSO.shootingType);
            Debug.Log("Passed Input Check");
        }
    }

}
