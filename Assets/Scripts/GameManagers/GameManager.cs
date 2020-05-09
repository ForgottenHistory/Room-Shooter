using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    [HideInInspector]
    public NewGS currentWeapon;
    public bool neverEnd;
    public Player_Perspective perspective;
    public abstract void StartGame();
    public abstract void Cleared();
    public abstract void Win();
}
