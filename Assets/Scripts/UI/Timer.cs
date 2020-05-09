using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //timer text

    Text timerText;
    float timer = 0;
    [HideInInspector]
    public bool active = false;
    private void Start()
    {
        timerText = GetComponent<Text>();
        timerText.text = " ";
    }
    void Update()
    {
        if (active)
        {
            //update timertext
            timer += Time.deltaTime;
            timerText.text = timer.ToString();
        }
    }
}
