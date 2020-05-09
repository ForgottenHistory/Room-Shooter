using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOffMap : MonoBehaviour
{
    //player or enemy fall off map, dead
    //might happen due to bugs 

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            other.GetComponent<PlayerController>().Damage(10000);
        }
        else if(other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().Damaged(100000, 0);
        }
    }
}
