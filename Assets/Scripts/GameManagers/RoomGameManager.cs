using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomGameManager : GameManager
{
    ////////////////////////////////////////////////////////////////
    //
    //                     GAME MANAGER
    //
    // Keep track of player & level
    //
    ////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    ////////////////////////////////////////////////////////////////

    public int length = 6; // How many rooms to create (length of the whole level)

    // UI
    [SerializeField] Text proceedText = null;
    [SerializeField] GameObject winBtn = null;
    [SerializeField] GameObject loadingScreen = null;
    [SerializeField] GameObject clickBtn = null;

    ////////////////////////////////////////////////////////////////
    // PRIVATE VARIABLES
    ////////////////////////////////////////////////////////////////

    GameObject currentDoor = null; // Door at the current room
    int inRoom = 0; // Player in this room

    ////////////////////////////////////////////////////////////////
    // INITIALIZE  
    ////////////////////////////////////////////////////////////////
    
    private void Awake()
    {
        // UI
        GameObject.Find( "DebugManager" ).GetComponent<DebugManager>().Initialize();
        proceedText.gameObject.SetActive(false);
        loadingScreen.SetActive(true);

        // 1
        Createlevel();
        
        // 2
        Invoke("CreateBullets", 1f);
    }

    ////////////////////////////////////////////////////////////////
    // START GAME
    ////////////////////////////////////////////////////////////////

    public override void StartGame() 
    {
        // Player
        //GameObject.Find("Player").GetComponent<PlayerController>().Initialize();
        currentWeapon.active = true;

        // Camera & Cursor
        Camera.main.GetComponent<CameraController>().ChangeState(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // UI
        proceedText.gameObject.SetActive(true);
        clickBtn.SetActive(false);

        // Initialize components
        GetComponent<MusicPlayer>().Initialize();
        GameObject.Find("TimerText").GetComponent<Timer>().active = true;
    }

    ////////////////////////////////////////////////////////////////
    // ROOM CLEARED
    ////////////////////////////////////////////////////////////////

    public override void Cleared()
    {
        // Check if at level lenght, if yes WIN
        if ( inRoom >= length)
        {
            Win();
            DebugManager.GetInstance().Print( this.ToString(), "Info: Player win");
        }
        else
        {
            proceedText.text = "PROCEED TO NEXT ROOM";
            DebugManager.GetInstance().Print( this.ToString(), "Info: Cleared room " + inRoom );
        }
    }

    ////////////////////////////////////////////////////////////////
    // WALK TO NEXT ROOM
    ////////////////////////////////////////////////////////////////

    public void ChangeRoom(GameObject newDoor)
    {
        inRoom++;
        DebugManager.GetInstance().Print( this.ToString(), "Info: Walked into room " + inRoom );
        proceedText.GetComponent<Text>().text = " ";
       
        // fix error for first room
        if(currentDoor != null)
        {
            currentDoor.SetActive(true);
        }
        currentDoor = newDoor;
    }

    ////////////////////////////////////////////////////////////////
    // WIN
    ////////////////////////////////////////////////////////////////

    public override void Win()
    {
        // UI
        winBtn.SetActive(true);
        proceedText.gameObject.SetActive(false);
        GameObject.Find("TimerText").GetComponent<Timer>().active = false;

        // Camera & Cursor
        Camera.main.GetComponent<CameraController>().ChangeState(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    ////////////////////////////////////////////////////////////////
    // CREATE
    ////////////////////////////////////////////////////////////////

    void Createlevel()
    {
        GetComponent<LevelManager>().CreateRoomLevel(length);
    }
    
    ////////////////////////////////////////////////////////////////
    
    void CreateBullets()
    {
        GameObject.Find("Bullets").GetComponent<BulletStockpile>().Initialize();
        loadingScreen.SetActive(false);
    }
    
    ////////////////////////////////////////////////////////////////
}
