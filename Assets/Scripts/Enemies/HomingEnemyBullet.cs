using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemyBullet : EnemyBullet
{
    //basic bullet to hurt player
    //nothing fancy
    GameObject cam;
    [SerializeField]
    float speed = 1.1f, rotationTime = 1f, deathTimer = 60f;
    float defaultDeathTimer;
    private void Start()
    {
        defaultDeathTimer = deathTimer;
        cam = Camera.main.gameObject;
    }
    private void Update()
    {
        if (GetComponent<Animator>().enabled == false)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cam.transform.position - transform.position), Time.deltaTime * rotationTime );
            transform.parent.position += transform.forward * speed;
            //if(deathTimer <= 0)
            //{
            //    GetComponent<Renderer>().enabled = false;
            //    GetComponent<Rigidbody>().velocity = Vector3.zero;
            //}
            //deathTimer -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if enabled, hurt player
        if (other.name == "Player" && GetComponent<Renderer>().enabled == true)
        {
            other.GetComponent<PlayerController>().Damage(5);
        }
        //if hit player or wall, deactivate
        if (other.tag == "Player" || other.tag == "Wall" || other.tag == "Obstacle")
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    public override void Activate(float bulletSpeed)
    {
        //GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
        GetComponent<Renderer>().enabled = true;
        if (GetComponent<Animator>())
        {
            deathTimer = defaultDeathTimer;
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Play("Homing1");
        }
    }

    public void StopAnimator()
    {
        transform.parent.position = transform.position;
        GetComponent<Animator>().enabled = false;
    }
}