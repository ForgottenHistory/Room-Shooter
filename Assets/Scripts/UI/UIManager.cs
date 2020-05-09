using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //gamemanager
    GameManager gm;
    //ammotext
    public GameObject hurtEffect;
    public Text healthText;
    public Text ammoText;
    //singleton
    public static UIManager control;
    private void Awake()
    {
        if (control == null)
        {
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    //update ammotext with ammo
    void Update()
    {
        ammoText.text = gm.currentWeapon.bulletsInMagazine.ToString() + " / " + gm.currentWeapon.magazineSize.ToString();
    }

    public void Hurt(float health)
    {
        healthText.text = health.ToString();
        hurtEffect.GetComponent<Animator>().Play("PlayerHurt");
    }
}
