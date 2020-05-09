using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract void Initialize();
    public abstract void Shoot(int shootPoint);
    public abstract void Damaged(float damage, float impact);
    public abstract void Activate();
}
