using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArea : MonoBehaviour
{
    //room manager script
    //counts enemies

    static RoomArea activated;
    int enemyCount = 0;
    GameManager gm;
    
    ////////////////////////////////////////////////////////////////

    private void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    
    ////////////////////////////////////////////////////////////////
    
    public void EnemyDies()
    {
        enemyCount--;

        ////////////////////////////////////////////////////////////////

        if ( enemyCount <= 0 && gm.neverEnd == false )
        {
            ////////////////////////////////////////////////////////////////
            
            gm.Cleared();

            if ( gm is RoomGameManager )
                transform.parent.Find("Door").gameObject.SetActive(false);
            
            ////////////////////////////////////////////////////////////////

            foreach ( Transform o in transform.parent.Find("Obstacles"))
            {
                o.GetComponent<AnimatedObstacle>().Activate(false);
            }
            
            ////////////////////////////////////////////////////////////////

            if ( GameObject.Find("DataManager"))
            {
                GameObject.Find("DataManager").GetComponent<DataManager>().misc_Values.coinsAmount++;
                GameObject.Find("DataManager").GetComponent<DataManager>().SaveValues();
            }
            
            ////////////////////////////////////////////////////////////////
        }

        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////

    void ActivateEnemies()
    {
        ////////////////////////////////////////////////////////////////
        
        if ( transform.parent.Find("Enemies"))
        {
            foreach (Transform enemy in transform.parent.Find("Enemies"))
            {
                enemy.GetComponent<Enemy>().Activate();
                enemyCount++;
            }
        }

        ////////////////////////////////////////////////////////////////

        if ( transform.parent.Find("Obstacles"))
        {
            foreach (Transform o in transform.parent.Find("Obstacles"))
            {
                o.GetComponent<AnimatedObstacle>().Activate(true);
            }
        }

        ////////////////////////////////////////////////////////////////

        if ( transform.parent.Find("Spawners"))
        {
            foreach (Transform s in transform.parent.Find("Spawners"))
            {
                s.GetComponent<ObjectSpawner>().ChangeState(true);
                if(s.GetComponent<ObjectSpawner>().objectToSpawn.GetComponent<Enemy>())
                    enemyCount += s.GetComponent<ObjectSpawner>().amountOfTimes;
            }
        }
        
        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player" && activated != this)
        {
            ////////////////////////////////////////////////////////////////
            
            activated = this;
            if( gm is RoomGameManager )
            {
                RoomGameManager rGameManager = gm as RoomGameManager;
                rGameManager.ChangeRoom( transform.parent.Find( "Door" ).gameObject );
            }

            ActivateEnemies();

            ////////////////////////////////////////////////////////////////

            if ( enemyCount <= 0 )
            {
                gm.Cleared();

                if( gm.GetComponent<RoomGameManager>() )
                    transform.parent.Find("Door").gameObject.SetActive(false);
            }
            
            ////////////////////////////////////////////////////////////////
        }
    }
    
    ////////////////////////////////////////////////////////////////
}
