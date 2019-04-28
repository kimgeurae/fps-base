using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float bulletSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate((new Vector3(0f, 0f, bulletSpeed * 2) * Time.deltaTime));
        Destroy(this.gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

}
