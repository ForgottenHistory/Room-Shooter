using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTemp : MonoBehaviour
{
    //what happens when you win script

    public GameObject loadingscreen;
    public void Restart()
    {
        loadingscreen.SetActive(true);
        SceneManager.LoadScene(0);
    }
}
