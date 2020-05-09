using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlyingEnemy : Enemy
{

        //stuff
    GameObject cam;
    GameObject aim;
    [SerializeField]
    GameObject bulletPrefab;
    BulletStockpile stockpile;
    [SerializeField]
    int bullets = 1;
    float defaultTimer;
    bool activated = false;
    [SerializeField]
    //stats
    float health = 100f,
        enemyRange = 50f,
        minimumRange = 10f,
        bulletSpeed = 80f,
        firerate = 0.05f,
        movementSpeed = 10f;

    //get stuff
    public override void Initialize()
    {
        cam = Camera.main.gameObject;
        aim = transform.GetChild(0).gameObject;
        stockpile = GameObject.Find("Bullets").GetComponent<BulletStockpile>();
        stockpile.AddBullet(bulletPrefab.name, bullets, firerate, bulletSpeed, transform.parent.parent.name);
        defaultTimer = firerate;
    }

    void Update()
    {
        if (activated)
        {            
            //makes enemy look at player
            //transform.LookAt(cam.transform);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cam.transform.position - transform.position), 2f * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); //only y rot is changed, looks nicer 
            if (Vector3.Distance(transform.position, cam.transform.position) < enemyRange)
            {
                //enemy's weapon looks at player
                //aim.transform.LookAt(cam.transform);
                aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, Quaternion.LookRotation(cam.transform.position - aim.transform.position), 2f * Time.deltaTime);
                if (0 > firerate)
                {
                    Shoot(0);
                    firerate = defaultTimer;
                }
            }
            //fly towards player but needs fixing
            //else if (Vector3.Distance(transform.position, cam.transform.position) < minimumRange)
            //{
            //    transform.position = transform.forward * movementSpeed * Time.deltaTime;
            //    if (transform.position.y <= 20)
            //    {
            //        transform.position = new Vector3(transform.position.x, 20, transform.position.z);
            //    }
            //}
            firerate -= Time.deltaTime;
        }
    }
    //shoot bullet forward
    public override void Shoot(int shootPoint)
    {
        GameObject bullet = stockpile.GetBullet(bulletPrefab.name);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = aim.transform.rotation;
        bullet.GetComponent<EnemyBullet>().Activate(bulletSpeed);
    }
    //enemy damaged
    public override void Damaged(float damage, float impact)
    {
        health -= damage;
        //Die
        if (health <= 0)
        {
            GetComponent<Rigidbody>().useGravity = true;
            activated = false;
            GetComponent<Rigidbody>().velocity = -transform.forward * impact;
            tag = "Untagged";
            transform.parent.parent.GetChild(0).GetComponent<RoomArea>().EnemyDies();
            GetComponent<Renderer>().material.color = Color.cyan;
            ChangeColor(transform);
        }
    }
    //change color of children
    void ChangeColor(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.GetComponent<Renderer>())
            {
                child.GetComponent<Renderer>().material.color = Color.cyan;
            }
            ChangeColor(child);
        }
    }
    //activate
    public override void Activate()
    {
        activated = true;
        transform.LookAt(cam.transform);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
