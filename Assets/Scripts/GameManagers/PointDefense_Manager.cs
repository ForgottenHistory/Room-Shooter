using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointDefense_Manager : GameManager
{
    //script keeps track of player, weapon, level and misc

    int m_CurrentWave; //currentroom
    public int m_AmountOfWaves = 6; //length of map
    public Text m_MainText;
    public GameObject winBtn;
    public GameObject loadingScreen;

    //initializes different scripts
    private void Awake()
    {
        m_MainText.gameObject.SetActive(false);
        m_CurrentWave = 0;
        loadingScreen.SetActive(true);
        Createlevel();
        Invoke("CreateBullets", 1f);
    }
    //player click startbutton
    public override void StartGame()
    {
        perspective = Player_Perspective.TOP_DOWN;
        //GameObject.Find("Player").GetComponent<PlayerController>().Initialize();
        //GameObject.Find("TimerText").GetComponent<Timer>().active = true;
        //currentWeapon.active = true;
        //Camera.main.GetComponent<CameraController>().ChangeState(true);
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        //m_MainText.gameObject.SetActive(true);
        GetComponent<MusicPlayer>().Initialize();
    }
    //check if win or proceed
    public override void Cleared()
    {
        if (m_CurrentWave >= m_AmountOfWaves)
        {
            Win();
            Debug.Log("Info: Player win");
        }
        else
        {
            m_MainText.text = "PROCEED TO NEXT ROOM";
            Debug.Log("Info: Cleared room " + m_CurrentWave);
        }
    }
    //change room
    public void ChangeWave()
    {
        Debug.Log("Info: Player completed " + m_CurrentWave);
        m_CurrentWave++;
        m_MainText.GetComponent<Text>().text = " ";
    }
    //player win
    public override void Win()
    {
        winBtn.SetActive(true);
        //m_MainText.gameObject.SetActive(false);
        //Camera.main.GetComponent<CameraController>().ChangeState(false);
        //Cursor.visible = true;
        //GameObject.Find("TimerText").GetComponent<Timer>().active = false;
        //Cursor.lockState = CursorLockMode.None;
    }

    void Createlevel()
    {
        GetComponent<LevelManager>().CreatePointDefenseLevel(m_AmountOfWaves);
        StartGame();
    }
    void CreateBullets()
    {
        GameObject.Find("Bullets").GetComponent<BulletStockpile>().Initialize();
        loadingScreen.SetActive(false);
    }
}
