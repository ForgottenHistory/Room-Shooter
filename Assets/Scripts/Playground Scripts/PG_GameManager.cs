using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PG_GameManager : GameManager
{
    [SerializeField]
    PlayerController player;

    public override void Cleared()
    {
        throw new System.NotImplementedException();
    }

    public override void StartGame()
    {
        throw new System.NotImplementedException();
    }

    public override void Win()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        //player.Initialize();
        Camera.main.GetComponent<CameraController>().ChangeState(true);
        currentWeapon.active = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
