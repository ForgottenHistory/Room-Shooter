using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    //stationary enemy. aims at player


    GameObject cam;
    GameObject player;
    GameManager gm;
    GameObject aim;
    BulletStockpile stockpile;
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    int bullets = 1;
    [SerializeField]
    float firerate = 0.1f,
     health = 100f,
     enemyRange = 400f,
     bulletSpeed = 50f;

    float defaultTimer;
    bool activated = false;
    public override void Initialize()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        cam = Camera.main.gameObject;
        player = GameObject.Find("Player");
        aim = transform.GetChild(0).gameObject;
        if (GameObject.Find("Bullets").GetComponent<BulletStockpile>())
        {
            stockpile = GameObject.Find("Bullets").GetComponent<BulletStockpile>();
            stockpile.AddBullet(bulletPrefab.name, bullets, firerate, bulletSpeed, transform.parent.parent.name);
        }
        defaultTimer = firerate;
    }
    void Update()
    {
        if (activated)
        {
            //makes enemy look at player
            if(gm.perspective == Player_Perspective.FIRST_PERSON)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cam.transform.position - transform.position), 2f * Time.deltaTime);
            }
            else if (gm.perspective == Player_Perspective.TOP_DOWN)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), 2f * Time.deltaTime);
            }

            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); //only y rot is changed, looks nicer

            if (Vector3.Distance(transform.position, cam.transform.position) < enemyRange)
            {
                //enemy's weapon looks at player
                aim.transform.LookAt(cam.transform);
                if (gm.perspective == Player_Perspective.FIRST_PERSON)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(cam.transform.position - transform.position), 2f * Time.deltaTime);
                }
                else if (gm.perspective == Player_Perspective.TOP_DOWN)
                {
                    aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, Quaternion.LookRotation(player.transform.position - aim.transform.position), 2f * Time.deltaTime);
                    aim.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); //only y rot is changed, looks nicer
                }

                if (0 > firerate)
                {
                    Shoot(0);
                    firerate = defaultTimer;
                }
            }

            firerate -= Time.deltaTime;
        }
    }
    //shoot bullet forward
    public override void Shoot(int shootPoint)
    {
        GameObject bullet;
        if (GameObject.Find("Bullets").GetComponent<BulletStockpile>())
            bullet = stockpile.GetBullet(bulletPrefab.name);
        else
            bullet = Instantiate(bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = aim.transform.rotation;
        bullet.GetComponent<EnemyBullet>().Activate(bulletSpeed);
    }
    //enemy gets damaged
    public override void Damaged(float damage, float impact)
    {
        health -= damage;
        if (health <= 0)
        {
            activated = false;
            GetComponent<Rigidbody>().velocity = -transform.forward * impact;
            tag = "Untagged";
            transform.parent.parent.GetChild(0).GetComponent<RoomArea>().EnemyDies();
            transform.GetComponent<Renderer>().material.color = Color.cyan;
            ChangeColor(transform);
        }
    }
    //change color on all children
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
    public override void Activate()
    {
        activated = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
