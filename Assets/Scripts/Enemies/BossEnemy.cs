using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : Enemy
{
    ////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    ////////////////////////////////////////////////////////////////

    [SerializeField] float health = 100f;
    [SerializeField] List<float> bulletSpeeds = new List<float>();
    [SerializeField] List<GameObject> bulletPrefabs = new List<GameObject>();

    [SerializeField] Text BossHealthCounter = null;
    [SerializeField] Text BossHealthNameText = null;
    [SerializeField] Slider BossHealthMeter = null;
    
    ////////////////////////////////////////////////////////////////

    //components, objects, lists
    Animator anim = null;
    GameObject cam = null;
    BulletStockpile stockpile = null;
    
    ////////////////////////////////////////////////////////////////

    Dictionary<string, int> bullets = new Dictionary<string, int>();

    List<Transform> shootPoints = new List<Transform>();
    List<Transform> points = new List<Transform>();
    
    int numberOfAttackAnims = 0;
    
    ////////////////////////////////////////////////////////////////

    public override void Initialize()
    {
        ////////////////////////////////////////////////////////////////
        // GET STUFF
        ////////////////////////////////////////////////////////////////

        FindShootPoints( transform );
        
        //get animations
        anim = GetComponent<Animator>();
        for (int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (anim.runtimeAnimatorController.animationClips[i].name.Contains("Attack"))
            {
                numberOfAttackAnims++;
            }
        }

        stockpile = GameObject.Find( "Bullets" ).GetComponent<BulletStockpile>();
        cam = Camera.main.gameObject;

        ////////////////////////////////////////////////////////////////
        // ADD SHOOTPOINTS
        ////////////////////////////////////////////////////////////////

        foreach ( Transform list in shootPoints )
        {
            foreach ( Transform point in list )
            {
                points.Add(point);
            } 
        }


        ////////////////////////////////////////////////////////////////
        // ADD BULLETS TO STOCKPILE
        ////////////////////////////////////////////////////////////////

        for ( int i = 0; i < points.Count; i++)
        {
            foreach (Transform sp in points[i])
            {
                if (bullets.ContainsKey(bulletPrefabs[i].name))
                {
                    bullets[bulletPrefabs[i].name]++;
                }
                else
                {
                    bullets.Add(bulletPrefabs[i].name, 1);
                }
            }
        }
        
        ////////////////////////////////////////////////////////////////

        List<string> bulletsKeys = bullets.Keys.ToList();
        for( int i = 0; i < bulletPrefabs.Count; i++ )
        {
            stockpile.AddBullet( bulletPrefabs[ i ].name, bullets[ bulletPrefabs[ i ].name ], 0.01f, bulletSpeeds[ i ], transform.parent.parent.name );
        }
        
        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////

    //shoot bullet forward
    public override void Shoot(int shootPoint)
    {
        //iterate through shootpoints
        foreach (Transform sp in points[shootPoint])
        {
            GameObject bullet = stockpile.GetBullet(bulletPrefabs[shootPoint].name);
            bullet.transform.position = sp.position;
            bullet.transform.rotation = sp.rotation;
            
            if (bullet.GetComponent<EnemyBullet>())
            {
                bullet.GetComponent<EnemyBullet>().Activate(bulletSpeeds[shootPoint]);
            }
            else
            {
                bullet.transform.GetChild(0).GetComponent<EnemyBullet>().Activate(bulletSpeeds[shootPoint]);
            }
        }
    }

    ////////////////////////////////////////////////////////////////

    //enemy gets damaged
    public override void Damaged(float damage, float impact)
    {
        health -= damage;
        BossHealthMeter.value = health;
        BossHealthCounter.text = health.ToString();
        
        if (health <= 0)
        {
            tag = "Untagged";
            transform.parent.parent.GetChild(0).GetComponent<RoomArea>().EnemyDies();
            ChangeColor(transform);
            anim.SetBool("dead", true);
            anim.Play("Dying");
            BossHealthMeter.gameObject.SetActive(false);
            BossHealthNameText.text = " ";
            Destroy(anim);
        }
    }

    ////////////////////////////////////////////////////////////////

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
    
    ////////////////////////////////////////////////////////////////

    void FindShootPoints(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == "ShootPoints")
            {
                   shootPoints.Add(child);
            }
            FindShootPoints(child);
        }
    }
    
    ////////////////////////////////////////////////////////////////

    public override void Activate()
    {
        BossHealthMeter.maxValue = health;
        BossHealthMeter.value = health;
        BossHealthMeter.gameObject.SetActive(true);
        BossHealthCounter.text = health.ToString();
        BossHealthNameText.text = name.ToUpper();
        anim.SetBool("activated", true);
    }
    
    ////////////////////////////////////////////////////////////////

    public void RandomizeAnimation()
    {
        int number = Random.Range(0, numberOfAttackAnims);
        anim.SetInteger("attack_index", number);
    }

    ////////////////////////////////////////////////////////////////
}
