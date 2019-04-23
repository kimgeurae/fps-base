using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Enemy", menuName = "Enemy")]
public class EnemySO : ScriptableObject
{

    int enemyMaxHealth;

    public int ApplyDamage(int health, int dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, enemyMaxHealth);
        return health;
    }

}
