using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRange : MonoBehaviour
{
    List<GameObject> targets = new List<GameObject>();
    List<float> uptimes = new List<float>();
    List<bool> headshots = new List<bool>();
    int times = 10;
    int round = 0;

    private void Start()
    {
        foreach(Transform t in transform.GetChild(0))
        {
            targets.Add(t.gameObject);
        }
    }
    public void StartRange()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().ChangeState(false);
        StartCoroutine(TargetUp());
    }

    void EndRange()
    {
        float uptimeAverage = 0;
        foreach(float time in uptimes)
        {
            uptimeAverage += time;
        }
        uptimeAverage = uptimeAverage / uptimes.Count;
        Debug.Log("Average Uptime: " + uptimeAverage);
    }

    public void ReceiveInfo(float uptime, bool headshot)
    {
        uptimes.Add(uptime);
        headshots.Add(headshot);
    }

    IEnumerator TargetUp()
    {
        yield return new WaitForSeconds(5);
        targets[Random.Range(0, targets.Count - 1)].GetComponent<BasicTarget>().ChangeState(true);
        round++;
        if(round < times)
        {
            StartCoroutine(TargetUp());
        }
    }
}
