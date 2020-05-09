using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAnimatedEnemy : Enemy
{
    //stationary enemy. does not aim at player
    //plays random attack animations

        //not used anywhere
        //needs revamp

    Animator anim;
    GameObject cam;
    BulletStockpile stockpile;
    List<Transform> shootPoints = new List<Transform>();
    List<string> attacksName = new List<string>();
    List<int> attacksPos = new List<int>();
    [SerializeField]
    GameObject bulletPrefab;
    float timer = 5;
    bool activated = false;
    [SerializeField]
    float health = 100f,
         enemyRange = 400f,
         bulletSpeed = 50f;
    [SerializeField]
    int bulletCount = 5,
        bullets = 2;

    public override void Initialize()
    {
        //get animations
        anim = GetComponent<Animator>();
        for(int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (anim.runtimeAnimatorController.animationClips[i].name.Contains("Attack"))
            {
                attacksName.Add(anim.runtimeAnimatorController.animationClips[i].name);
                attacksPos.Add(i);
            }
        }
        //add shootpoints
        foreach(Transform sp in transform.Find("ShootPoints"))
        {
            shootPoints.Add(sp);
        }

        stockpile = GameObject.Find("Bullets").GetComponent<BulletStockpile>();
        stockpile.AddBullet(bulletPrefab.name, bullets, 0.1f, bulletSpeed, transform.parent.parent.name);
        cam = Camera.main.gameObject;
    }

    void Update()
    {
        if(activated)
        {
            if (Vector3.Distance(transform.position, cam.transform.position) < enemyRange)
            {
                if (0 > timer)
                {
                    //play a random attack
                    string attackClip = attacksName[0];
                    anim.Play(attackClip);
                    timer = anim.runtimeAnimatorController.animationClips[attacksPos[0]].length;
                }
            }
            timer -= Time.deltaTime;
        }
    }
    public override void Shoot(int Shootpoint)
    {
        foreach(Transform sp in shootPoints)
        {
            GameObject bullet = stockpile.GetBullet(bulletPrefab.name);
            bullet.transform.position = sp.transform.position;
            bullet.transform.rotation = sp.transform.rotation;
            bullet.GetComponent<EnemyBullet>().Activate(bulletSpeed);
        }
    }
    public override void Damaged(float damage, float impact)
    {
        health -= damage;
        if (health <= 0)
        {
            activated = false;
            GetComponent<Rigidbody>().velocity = -transform.forward * impact;
            tag = "Untagged";
            transform.parent.parent.GetChild(0).GetComponent<RoomArea>().EnemyDies();
            Destroy(anim);
            ChangeColor(transform);
        }
    }
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
