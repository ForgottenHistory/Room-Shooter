using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatedEnemy : Enemy
{
    //AI enemy
    //plays random attack animations if possible

    //editor values
    public GameObject bulletPrefab;
    public float health = 100f;
    public float enemyRange = 50;
    public float bulletSpeed = 50f;
    public int bullets = 1;

    //components, objects, lists
    Animator anim;
    NavMeshAgent agent;
    GameObject cam;
    List<Transform> shootPoints = new List<Transform>();
    List<string> attacksName = new List<string>();
    List<int> attacksPos = new List<int>();
    BulletStockpile stockpile;

    //local variables
    float timer = 5;
    public bool activated = false;

    public override void Initialize()
    {
        //get animations
        anim = GetComponent<Animator>();

        if( GetComponent<NavMeshAgent>() )
            agent = GetComponent<NavMeshAgent>();

        for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (anim.runtimeAnimatorController.animationClips[i].name.Contains("Attack"))
            {
                attacksName.Add(anim.runtimeAnimatorController.animationClips[i].name);
                attacksPos.Add(i);
            }
        }
        //add shootpoints
        foreach (Transform sp in transform.Find("ShootPoints"))
        {
            shootPoints.Add(sp);
        }

        stockpile = GameObject.Find("Bullets").GetComponent<BulletStockpile>();
        stockpile.AddBullet(bulletPrefab.name, bullets, 0.1f, bulletSpeed, transform.parent.parent.name);

        cam = Camera.main.gameObject;
    }

    void Update()
    {
        if (activated && anim != null)
        {
            RaycastHit hit;
            //in range and can see
            if (Vector3.Distance(transform.position, cam.transform.position) < enemyRange 
                && Physics.Raycast(transform.position, transform.TransformDirection(cam.transform.position), out hit, enemyRange))
            {
                anim.SetBool("nearPlayer", true);
                if (0 > timer)
                {
                    //play a random attack
                    if (attacksName.Count == 1)
                    {
                        anim.Play(attacksName[0]);
                    }
                    //random attack
                    else if (attacksName.Count > 1)
                    {
                        anim.Play(attacksName[Random.Range(0, attacksName.Count - 1)]);
                    }
                    timer = anim.runtimeAnimatorController.animationClips[attacksPos[0]].length;
                }
            }
            else
            {
                anim.SetBool("nearPlayer", false);
            }
            timer -= Time.deltaTime;
        }
    }
    //shoot bullet forward
    public override void Shoot(int shootPoint)
    {
        foreach (Transform sp in shootPoints)
        {
            GameObject bullet = stockpile.GetBullet(bulletPrefab.name);
            bullet.transform.position = sp.transform.position;
            bullet.transform.rotation = sp.transform.rotation;
            bullet.GetComponent<EnemyBullet>().Activate(bulletSpeed);
        }
    }
    //enemy gets damaged
    public override void Damaged(float damage, float impact)
    {
        health -= damage;
        if (health <= 0)
        {
            GetComponent<Rigidbody>().velocity = -transform.forward * impact;
            tag = "Untagged";
            transform.parent.parent.GetChild(0).GetComponent<RoomArea>().EnemyDies();
            Destroy(anim);
            Destroy(agent);
            ChangeColor(transform);
            activated = false;
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
        if(anim != null)
        {
            anim.SetBool("activated", true);
        }
    }
}
