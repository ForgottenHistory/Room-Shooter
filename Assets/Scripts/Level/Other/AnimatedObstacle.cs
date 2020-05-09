using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedObstacle : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    ////////////////////////////////////////////////////////////////


    [SerializeField] float bulletSpeed = 50f;
    [SerializeField] float firerate = 0.1f;
    [SerializeField] float  timer = 5f;

    [SerializeField] List<GameObject> bulletPrefabs = new List<GameObject>();

    ////////////////////////////////////////////////////////////////

    BulletStockpile stockpile = null;
    Animator anim = null;
    GameObject cam = null;
    Transform shootPoints = null;

    List<string> attacksName = new List<string>();
    List<int> attacksPos = new List<int>();
    Dictionary<string, int> bullets = new Dictionary<string, int>();
    
    bool activated = false;
    
    ////////////////////////////////////////////////////////////////

    public void Initialize()
    {
        ////////////////////////////////////////////////////////////////
        
        shootPoints = transform.Find( "ShootPoints" );
        stockpile = GameObject.Find( "Bullets" ).GetComponent<BulletStockpile>();
        cam = Camera.main.gameObject;
        anim = GetComponent<Animator>();
        
        if(GetComponent<Collider>())
            GetComponent<Collider>().enabled = false;

        ////////////////////////////////////////////////////////////////
        // GET ATTACK ANIMATIONS
        ////////////////////////////////////////////////////////////////

        for ( int i = 0; i < anim.runtimeAnimatorController.animationClips.Length; i++)
        {
            if (anim.runtimeAnimatorController.animationClips[i].name.Contains("Attack"))
            {
                attacksName.Add(anim.runtimeAnimatorController.animationClips[i].name);
                attacksPos.Add(i);
            }
        }
        
        ////////////////////////////////////////////////////////////////

        for ( int i = 0; i < shootPoints.childCount; i++)
        {
            foreach(Transform sp in shootPoints.GetChild(i))
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
            stockpile.AddBullet(bulletPrefabs[i].name, bullets[bulletPrefabs[i].name], firerate, bulletSpeed, transform.parent.parent.name);
        }
        
        ////////////////////////////////////////////////////////////////
    }
    
    ////////////////////////////////////////////////////////////////

    void Update()
    {
        if ( activated == false )
            return;
    
        ////////////////////////////////////////////////////////////////
        
        if (0 > timer )
        {
            //play attack
            if (attacksName.Count == 1)
            {
                anim.Play(attacksName[0]);
                timer = anim.runtimeAnimatorController.animationClips[attacksPos[0]].length;
            }
            //random attack
            else if (attacksName.Count > 1)
            {
                int animation = attacksPos[Random.Range(0, attacksName.Count - 1)];
                anim.Play(attacksName[animation]);
                timer = anim.runtimeAnimatorController.animationClips[animation].length;
            }
            else
            {
                Debug.Log("No attack animation for " + name);
            }
        }

        timer -= Time.deltaTime;
        
        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////

    public void Shoot(int shootPoint)
    {
        //iterate through shootpoints
        foreach ( Transform sp in shootPoints.GetChild( shootPoint ) )
        {
            GameObject bullet = stockpile.GetBullet( bulletPrefabs[ shootPoint ].name );
            bullet.transform.position = sp.position;
            bullet.transform.rotation = sp.rotation;
            if ( bullet.GetComponent<EnemyBullet>() )
            {
                bullet.GetComponent<EnemyBullet>().Activate( bulletSpeed );
            }
            else
            {
                bullet.transform.GetChild( 0 ).GetComponent<EnemyBullet>().Activate( bulletSpeed );
            }
        }
    }

    ////////////////////////////////////////////////////////////////
    
    public void Activate(bool state)
    {
        activated = state;
        if( state == false )
        {
            anim.enabled = false;
        }
    }
    
    ////////////////////////////////////////////////////////////////
}
