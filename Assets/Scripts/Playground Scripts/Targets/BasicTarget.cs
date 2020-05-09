using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTarget : MonoBehaviour
{
    GameObject head;
    float uptime = 0, timer = 0;
    bool active = true, headshot;
    
    void Update()
    {
        if (active)
        {
            timer += Time.deltaTime;
        }
    }
    public void ChangeState(bool newState)
    {
        active = newState;
        if(active == false)
        {
            uptime = timer;
            GetComponent<Animator>().Play("Down");
            transform.parent.parent.GetComponent<TargetRange>().ReceiveInfo(uptime, headshot);
        }
        else
        {
            uptime = 0;
            timer = 0;
            GetComponent<Animator>().Play("Up");
        }
    }
}
