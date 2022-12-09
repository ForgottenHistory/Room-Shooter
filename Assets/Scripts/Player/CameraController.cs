using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    ////////////////////////////////////////////////////////////////
    //
    //                      CAMERA CONTROLLER
    //
    ////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////
    // PUBLIC VARIABLES
    ////////////////////////////////////////////////////////////////

    //angle minimums
    [SerializeField]
    float xMin = -60f, 
        xMax = 60f;

    public float sens = 5f;

    ////////////////////////////////////////////////////////////////
    // PRIVATE VARIABLES
    ////////////////////////////////////////////////////////////////

    float rotX;
    float rotY;

    GameObject player;

    Vector3 offset;

    bool scriptEnabled = false;

    ////////////////////////////////////////////////////////////////
    // INITIALIZE
    ////////////////////////////////////////////////////////////////

    void Start()
    {
        player = GameObject.Find("Player");
        offset = transform.position - player.transform.position;

        if (PlayerPrefs.HasKey("Sensitivity"))
            sens = PlayerPrefs.GetFloat("Sensitivity");
    }

    ////////////////////////////////////////////////////////////////
    // UPDATE
    ////////////////////////////////////////////////////////////////

    void Update()
    {
        ////////////////////////////////////////////////////////////////
        
        if ( scriptEnabled == false) return;
        transform.position = player.transform.position + offset;
        
        ////////////////////////////////////////////////////////////////

        // Add mouse input
        rotX += Input.GetAxis("Mouse Y") * sens;
        rotY += Input.GetAxis("Mouse X") * sens;

        rotX = Mathf.Clamp(rotX, xMin, xMax); // Limit up and down

        // Change rotations on player & camera
        transform.eulerAngles = new Vector3(-rotX, rotY, transform.eulerAngles.z);
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, transform.eulerAngles.y, player.transform.rotation.z);

        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////
    // ACTIVATE & DISABLE
    ////////////////////////////////////////////////////////////////

    public void ChangeState(bool newState)
    {
        scriptEnabled = newState;
    }
}