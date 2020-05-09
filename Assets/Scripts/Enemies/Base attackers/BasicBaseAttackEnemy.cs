using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBaseAttackEnemy : Enemy
{
    //moving enemy

    public GameObject bulletPrefab;
    public int bullets = 1;
    public float firerate = 0.1f;
    public float health = 100.0f;
    public float enemyRange = 400.0f;
    public float bulletSpeed = 50.0f;
    public float movementSpeed = 10.0f;
    public float ableToGoForwardTimer;

    GameObject cam;
    GameObject player;
    GameManager gm;
    GameObject aim;
    Transform wayPointParent;
    BulletStockpile stockpile;

    float default_ShootTimer;
    int currentWayPoint = 0;
    int wayPointMax;
    bool ableToGoForward = false;

    bool activated = false;
    public override void Initialize()
    {
        if(GameObject.Find("GameManager"))
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        

        if (transform.parent.parent.Find("RoadWayPoints"))
        {
            wayPointParent = transform.parent.parent.Find("RoadWayPoints");
            wayPointMax = wayPointParent.childCount - 1;
            transform.position = wayPointParent.GetChild(0).position;
            transform.position = new Vector3(transform.position.x, transform.localScale.y / 2, transform.position.z);
            ableToGoForward = true;
        }
        else
            Debug.Log("No waypoints!");


        if (GameObject.Find("Player"))
            player = GameObject.Find("Player");

        if(transform.childCount != 0)
            aim = transform.GetChild(0).gameObject;

        if (GameObject.Find("Bullets").GetComponent<BulletStockpile>())
        {
            stockpile = GameObject.Find("Bullets").GetComponent<BulletStockpile>();
            stockpile.AddBullet(bulletPrefab.name, bullets, firerate, bulletSpeed, transform.parent.parent.name);
        }

        cam = Camera.main.gameObject;
        default_ShootTimer = firerate;
    }

    void Update()
    {
        if (activated)
        {
            if(currentWayPoint < wayPointMax)
            {
                Vector3 nextPoint = wayPointParent.GetChild(currentWayPoint + 1).position;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(nextPoint - transform.position), 4f * Time.deltaTime);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0); //only y rot is changed, looks nicer
                if (Vector3.Distance(transform.position, new Vector3(nextPoint.x, transform.position.y, nextPoint.z)) < 0.5f)
                {
                    StartCoroutine(ReachedPoint());
                }
                else if (ableToGoForward)
                {
                    transform.position += transform.forward * movementSpeed * Time.deltaTime;
                }
            }

            //if (Vector3.Distance(transform.position, cam.transform.position) < enemyRange)
            //{
            //    //enemy's weapon looks at player
            //    if (gm.perspective == Player_Perspective.FIRST_PERSON)
            //    {
            //        aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, Quaternion.LookRotation(cam.transform.position - transform.position), 2f * Time.deltaTime);
            //    }
            //    else if (gm.perspective == Player_Perspective.TOP_DOWN)
            //    {
            //        aim.transform.rotation = Quaternion.Slerp(aim.transform.rotation, Quaternion.LookRotation(player.transform.position - aim.transform.position), 2f * Time.deltaTime);
            //        aim.transform.rotation = Quaternion.Euler(0, aim.transform.eulerAngles.y, 0); //only y rot is changed, looks nicer
            //    }

            //    if (0 > firerate)
            //    {
            //        Shoot(0);
            //        firerate = default_ShootTimer;
            //    }
            //}

            //firerate -= Time.deltaTime;
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

    IEnumerator ReachedPoint()
    {
        currentWayPoint++;
        ableToGoForward = false;
        yield return new WaitForSeconds(ableToGoForwardTimer);
        ableToGoForward = true;
    }
}
