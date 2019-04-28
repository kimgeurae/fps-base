using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    public int health;

    public EnemySO enemySO;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(int receivedDmg)
    {
        if (health - receivedDmg > 0)
            health -= receivedDmg;
        else
        {
            health = 0;
            Destroy(this.gameObject);
        }
    }

}
