using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    //escape menu
    [SerializeField]
    GameObject escapeMenu, settingsMenu;
    [SerializeField]
    CameraController cameraController;
    private void Start()
    {
        //pause set to false
        escapeMenu.SetActive(false);
    }
    void Update()
    {
        BringUp();
    }
    public void Resume()
    {
        //back to default state
        escapeMenu.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        cameraController.ChangeState(true);

        if ( GameObject.Find( "MusicPlayer" ) )
            GameObject.Find( "MusicPlayer" ).GetComponent<MusicPlayer>().SetState( true );
    }
    public void BringUp()
    {
        //if press escape: cursor free and visible, time set to 0 and no camera movement
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            escapeMenu.SetActive(true);
            cameraController.ChangeState(false);

            if ( GameObject.Find( "MusicPlayer" ) )
                GameObject.Find( "MusicPlayer" ).GetComponent<MusicPlayer>().SetState( false );
        }
    }
    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        escapeMenu.SetActive(false);
    }
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        escapeMenu.SetActive(true);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
