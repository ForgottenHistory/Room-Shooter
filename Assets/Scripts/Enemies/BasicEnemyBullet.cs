using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBullet : EnemyBullet
{
    //basic bullet to hurt player
    //nothing fancy
    [SerializeField]
    bool hitWall = true;
    private void OnTriggerEnter(Collider other)
    {
        //if enabled, hurt player
        if (other.name == "Player" && GetComponent<Renderer>().enabled == true)
        {
            other.GetComponent<PlayerController>().Damage(5);
        }
        //if hit player or wall, deactivate
        if (other.tag == "Player" || other.tag == "Wall" && hitWall || other.tag == "Obstacle" && hitWall)
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public override void Activate(float bulletSpeed)
    {
        GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        GetComponent<Renderer>().enabled = true;
    }
}