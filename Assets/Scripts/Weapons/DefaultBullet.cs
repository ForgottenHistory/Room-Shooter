using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : MonoBehaviour
{
    //defaultscript for player bullet

    //default values
    float damage = 35;
    float impact = 500;

    //bullet enters something
    private void OnTriggerEnter(Collider other)
    {
        //if enemy, damage it
        if(other.tag == "Enemy" && GetComponent<Renderer>().enabled)
        {
            other.GetComponent<Enemy>().Damaged(damage, impact);
        }

        if(other.tag == "Enemy" || other.tag == "Wall")
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if(other.tag == "Target")
        {
            other.GetComponent<BasicTarget>().ChangeState(false);
        }
    }

    //weapon activate bullet
    public void Activate(float bulletSpeed, float damage, float impact)
    {
        GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        GetComponent<Renderer>().enabled = true;
        this.damage = damage;
        this.impact = impact;
    }
}
