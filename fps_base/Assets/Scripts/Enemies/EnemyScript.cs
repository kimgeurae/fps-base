using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    int health;

    public EnemySO enemySO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LocalApplyDamage(int receivedDmg)
    {
        health = enemySO.ApplyDamage(health, receivedDmg);
    }

}
