using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //angle minimums
    [SerializeField]
    float xMin = -60f, 
        xMax = 60f;

    //variables
    public float sens = 5f;
    float rotX;
    float rotY;
    GameObject player;
    Vector3 offset;

    bool scriptEnabled = false;

    void Start()
    {
        player = GameObject.Find("Player");
        //offset from player
        offset = transform.position - player.transform.position;

        if (PlayerPrefs.HasKey("Sensitivity"))
            sens = PlayerPrefs.GetFloat("Sensitivity");
    }

    void Update()
    {
        if (scriptEnabled == false) return;
        transform.position = player.transform.position + offset;

        //rotations
        rotX += Input.GetAxis("Mouse Y") * sens;
        rotY += Input.GetAxis("Mouse X") * sens;

        rotX = Mathf.Clamp(rotX, xMin, xMax); //limit player

        //change rotations
        transform.eulerAngles = new Vector3(-rotX, rotY, transform.eulerAngles.z);
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.x, transform.eulerAngles.y, player.transform.rotation.z);
    }

    //change scriptenabled
    public void ChangeState(bool newState)
    {
        scriptEnabled = newState;
    }
}