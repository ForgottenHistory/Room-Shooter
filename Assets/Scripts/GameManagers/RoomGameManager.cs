using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomGameManager : GameManager
{
    //script keeps track of player, weapon, level and misc

    public int length = 6; //length of map
    [SerializeField] Text proceedText = null;
    [SerializeField] GameObject winBtn = null;
    [SerializeField] GameObject loadingScreen = null;
    [SerializeField] GameObject clickBtn = null;
    
    GameObject currentDoor = null;
    int room = 0; //currentroom

    //initializes different scripts
    private void Awake()
    {
        GameObject.Find( "DebugManager" ).GetComponent<DebugManager>().Initialize();
        proceedText.gameObject.SetActive(false);
        loadingScreen.SetActive(true);
        Createlevel();
        Invoke("CreateBullets", 1f);
    }
    //player click startbutton
    public override void StartGame() 
    {
        //GameObject.Find("Player").GetComponent<PlayerController>().Initialize();
        GameObject.Find("TimerText").GetComponent<Timer>().active = true;
        currentWeapon.active = true;
        Camera.main.GetComponent<CameraController>().ChangeState(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        proceedText.gameObject.SetActive(true);
        clickBtn.SetActive(false);
        GetComponent<MusicPlayer>().Initialize();
    }

    //check if win or proceed
    public override void Cleared()
    {
        if (room >= length)
        {
            Win();
            DebugManager.GetInstance().Print( this.ToString(), "Info: Player win");
        }
        else
        {
            proceedText.text = "PROCEED TO NEXT ROOM";
            DebugManager.GetInstance().Print( this.ToString(), "Info: Cleared room " + room);
        }
    }

    //change room
    public void ChangeRoom(GameObject newDoor)
    {
        room++;
        DebugManager.GetInstance().Print( this.ToString(), "Info: Walked into room " + room);
        proceedText.GetComponent<Text>().text = " ";
        //fix error for first room
        if(currentDoor != null)
        {
            currentDoor.SetActive(true);
        }
        currentDoor = newDoor;
    }

    //player win
    public override void Win()
    {
        winBtn.SetActive(true);
        proceedText.gameObject.SetActive(false);
        Camera.main.GetComponent<CameraController>().ChangeState(false);
        Cursor.visible = true;
        GameObject.Find("TimerText").GetComponent<Timer>().active = false;
        Cursor.lockState = CursorLockMode.None;
    }

    void Createlevel()
    {
        GetComponent<LevelManager>().CreateRoomLevel(length);
    }
    void CreateBullets()
    {
        GameObject.Find("Bullets").GetComponent<BulletStockpile>().Initialize();
        loadingScreen.SetActive(false);
    }
}
