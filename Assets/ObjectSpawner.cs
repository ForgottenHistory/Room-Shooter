using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public float timer;
    float default_Timer;
    public bool activeState;
    public int amountOfTimes;
    public float startupTimer;
    public bool randomSpawns;
    float default_StartupTimer;
    int spawnedCounter = 0;
    
    // Update is called once per frame
    private void Start()
    {
        default_Timer = timer;
        default_StartupTimer = startupTimer;
    }
    void Update()
    {
        if (activeState)
        {
            if (timer < 0 && spawnedCounter < amountOfTimes && startupTimer < 0 || timer < 0 && amountOfTimes <= 0 && startupTimer < 0)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
                if (spawnedObject.GetComponent<Enemy>())
                {
                    spawnedObject.transform.SetParent(transform.parent.parent.Find("Enemies"));
                    spawnedObject.GetComponent<Enemy>().Initialize();
                    spawnedObject.GetComponent<Enemy>().Activate();
                }

                if (randomSpawns)
                {
                    float randomX = Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2);
                    float randomZ = Random.Range(-transform.localScale.z / 2, transform.localScale.z / 2);
                    spawnedObject.transform.position = new Vector3(randomX + transform.position.x, spawnedObject.transform.position.y, randomZ + transform.position.z);
                }

                timer = default_Timer;
                spawnedCounter++;
            }

            timer -= Time.deltaTime;
            startupTimer -= Time.deltaTime;
        }
    }

    public void ChangeState(bool state)
    {
        activeState = state;
        startupTimer = default_StartupTimer;
    }
    public void ResetAmount()
    {
        amountOfTimes = 0;
    }
}
