using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    //IMPORTANT
    //this script was replaced with an updated version of the top quality arena weapon script
    //might go back to this at a later date
    //look at newGS instead

    //parts
    Camera playerCam;
    //static GunScript activeScript;
    public Transform bulletSpawnPoint;
    public Transform aimPos;
    public GameObject bulletPrefab;
    public Text ammoText;
    public ParticleSystem shootParticles;
    List<GameObject> bullets = new List<GameObject>();

    //public values
    public float damage;
    public float range;
    public float fireRate;
    public float aimTime;
    public float reloadTime;
    public float bulletSpeed;
    public float recoilX;
    public float recoilZ;
    public float impact;
    public int magSize;
    public bool semiAuto;
    public bool switchable;

    //local values
    int ammo;
    int currentBullet = 0;
    float nextShot;
    bool reloading = false;
    bool isAim = false;
    bool playingAnimation = false;
    Vector3 originalPos;
    Vector3 recoilOffset;

    void Start()
    {
        playerCam = Camera.main;
        originalPos = transform.localPosition;
        ammo = magSize;
        CreateBullets();
        ammoText.text = ammo + " / " + magSize;
    }

    void Update()
    {
        recoilOffset = originalPos - transform.localPosition;

        //if not playing animation or is aiming, go back to original pos
        if (playingAnimation == false || isAim)
        {
            FixGunPosition();
        }

        //hold to fire
        if (Input.GetButton("Fire1") && Time.time > nextShot && reloading == false && ammo > 0 && semiAuto == false)
        {
            Shoot();
        }
        //click to fire
        else if (Input.GetButtonDown("Fire1") && Time.time > nextShot && reloading == false && ammo > 0 && semiAuto == true)
        {
            Shoot();
        }

        if (Input.GetButton("Fire2"))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPos.localPosition, aimTime);
        }
        else 
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, aimTime);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        if (Input.GetKeyDown(KeyCode.V) && switchable)
        {
            semiAuto = !semiAuto;
        }

        ammoText.text = ammo + " / " + magSize;
    }

    void Shoot()
    {
        nextShot = Time.time + fireRate;

        GameObject bullet = bullets[currentBullet];
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;

        bullet.GetComponent<DefaultBullet>().Activate(bulletSpeed, damage, impact);
        //recoil but it is done very wrong
        //playerCam.transform.rotation = Quaternion.Euler(playerCam.transform.localEulerAngles.x + recoilX, playerCam.transform.localEulerAngles.y + recoilZ, playerCam.transform.localEulerAngles.z);
        shootParticles.Play();

        if (isAim == true)
        {
            transform.position += transform.up * Time.deltaTime * recoilX / 4;
            transform.position += transform.forward * Time.deltaTime * UnityEngine.Random.Range(-recoilX, recoilX) / 4;
        }
        if (isAim == false)
        {
            transform.position += transform.up * Time.deltaTime * recoilX;
            transform.position += transform.forward * Time.deltaTime * UnityEngine.Random.Range(-recoilX, recoilX);
        }
        if (currentBullet >= magSize)
        {
            currentBullet = 0;
        }
        ammo--;
    }

    IEnumerator Reload()
    {
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo = magSize;
        reloading = false;
    }

    void CreateBullets()
    {
        for(int i = 0; i < magSize; i++)
        {
            bullets.Add(Instantiate(bulletPrefab));
            bullets[i].transform.parent = GameObject.Find("Bullets").transform;
            bullets[i].GetComponent<Renderer>().enabled = false;
        }
    }

    //gun go back to original pos
    void FixGunPosition()
    {
        if (isAim)
        {
            transform.localPosition += recoilOffset * recoilX * Time.deltaTime * 2;
        }
        else if (!isAim)
        {
            transform.localPosition += recoilOffset * Time.deltaTime * 2;
        }
    }

    //void SwitchActive(GunScript gun)
    //{
    //    activeScript = gun;
    //}
}
