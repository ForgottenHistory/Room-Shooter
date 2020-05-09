using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayScript : MonoBehaviour
{
    [SerializeField]
    GameObject loadingscreen;
    //launch level
    public void PlayGame()
    {
        loadingscreen.SetActive(true);
        SceneManager.LoadScene(1);
    }
}
