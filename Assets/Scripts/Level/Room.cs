using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //holds room info

    public bool startRoom;
    public bool endRoom;
    public int sizeY = 500;
    int posY;

    public int GetPosY()
    {
        if ( startRoom )
        {
            posY = 0;
        }
        if ( endRoom )
        {
            posY = GameObject.Find("GameManager").GetComponent<RoomGameManager>().length - 1;
        }
        return posY;
    }
}
