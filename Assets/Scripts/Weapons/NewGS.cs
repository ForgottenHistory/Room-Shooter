using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGS : MonoBehaviour
{
    //weaponscript from Top Quality Arena
    //a bit updated

    [Header("Positioning and Components")]
    public AudioClip shootAudio;
    public AudioClip reloadAudio;
    public AudioClip switchAudio;
    public ParticleSystem muzzleFlash;
    public GameObject bullet;
    public GameObject crosshair;
    //gameobject in the middle of screen
    Transform point;
    //bullets come from here
    //placed at barrel
    public GameObject shootFromPoint;
    //bullets
    List<GameObject> bullets = new List<GameObject>();
    //audio
    AudioSource audioSource;
    Animator animator;
    //gameManager
    GameManager gm;
    //positions
    Vector3 originalPosition = new Vector3(0, 0, 0);
    Vector3 recoilOffset;

    [Header("Stats")]
    public int damage;
    public int magazineSize;
    public int bulletsInMagazine;
    public float reloadTime;
    public float recoilMultiplier;
    public float fireRate;
    public float hipfireSpread;
    public int impact;
    public float range;
    public float bulletSpeed;
    public bool shotgun;
    public int pellets;
    public bool isAutomatic;
    public bool canChangeMode;

    //variables
    public bool playingAnimation = false;
    bool reloading = false;
    float timer = 0;
    public bool isAim = false;
    public bool active = false;
    bool shot = false;
    int currentBullet = 0;


    public void Start()
    {
        //get gamemanager and components
        if (GameObject.Find("GameManager").GetComponent<GameManager>() == false)
        {
            active = true;
            Debug.Log("Info: No gamemanager. Obj: " + name);
        }
        else
        {
            gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.currentWeapon = this;
        }
        audioSource = GetComponent<AudioSource>();
        animator = transform.parent.GetComponent<Animator>();
        //load magazine
        bulletsInMagazine = magazineSize;
        //get point where the bullet should go (middle of screen)
        point = transform.parent.Find("ShootingPoint");
        //creating bullets depend on pellets. cant be 0 or under
        if(pellets <= 0)
        {
            pellets = 1;
        }
        CreateBullets();
    }

    void Update()
    {
        if (active)
        {
            //distance has weapon traveled due to shooting
            recoilOffset = originalPosition - transform.localPosition;

            //shoot, automatic mode
            //hold to fire
            if (Input.GetMouseButton(0) && bulletsInMagazine > 0 && timer <= 0 && !reloading && isAutomatic)
            {
                Shoot();
                shot = true;
                timer = fireRate;
            }
            //shoot, not automatic mode
            //press to fire
            else if (Input.GetMouseButtonDown(0) && bulletsInMagazine > 0 && timer <= 0 && !reloading && !isAutomatic)
            {
                Shoot();
                shot = true;
                timer = fireRate;
            }

            //if not playing animation or is aiming, go back to original pos
            if (playingAnimation == false || isAim)
            {
                FixGunPosition();
            }

            //actions

            //reload
            if (Input.GetKeyDown(KeyCode.R) && !isAim && !playingAnimation)
            {
                StartCoroutine(Reload());
            }
            //switch mode
            if (Input.GetKeyDown(KeyCode.V))
            {
                StartCoroutine(SwitchMode());
            }
            //aim
            if (Input.GetMouseButtonDown(1) && isAim == false && !reloading)
            {
                StartCoroutine(IsAiming());
            }
            //stop aim
            if (Input.GetMouseButtonUp(1) && isAim == true)
            {
                StartCoroutine(IsDeAiming());
            }
            //firerate timer
            if (timer <= 0)
            {
                shot = false;
            }
            if (shot == true)
            {
                timer = timer - Time.deltaTime;
            }
        }
    }
    void Shoot()
    {
        //play effects
        audioSource.clip = shootAudio;
        audioSource.Play();
        muzzleFlash.Play();

        //makes bullets go more towards middle of screen
        shootFromPoint.transform.LookAt(point);
        //is aiming
        if (isAim)
        {
            animator.Play("IsAim", 0, 0.0f);
            //gun go up when shoot
            transform.position += transform.up * Time.deltaTime * recoilMultiplier / 4;
            transform.position += transform.forward * Time.deltaTime * Random.Range(-recoilMultiplier, recoilMultiplier) / 4;
            if (shotgun)
            {
                for (int i = 0; i < pellets; i++)
                {
                    //shotguns should not become snipers when aiming down sights
                    Quaternion result = Quaternion.Euler(new Vector3(shootFromPoint.transform.eulerAngles.x + Random.Range(-hipfireSpread, hipfireSpread / 2),
                        shootFromPoint.transform.eulerAngles.y + Random.Range(-hipfireSpread, hipfireSpread / 2),
                        shootFromPoint.transform.eulerAngles.z));
                    GameObject bullet = bullets[currentBullet];
                    bullet.transform.position = shootFromPoint.transform.position;
                    bullet.transform.rotation = result;
                    bullet.GetComponent<DefaultBullet>().Activate(bulletSpeed, damage, impact);

                    CurrentBulletUp();
                }
            }
            else
            {
                //for gameplay purposes are aim down sights 100% accurate
                GameObject bullet = bullets[currentBullet];
                bullet.transform.position = shootFromPoint.transform.position;
                bullet.transform.rotation = shootFromPoint.transform.rotation;
                bullet.GetComponent<DefaultBullet>().Activate(bulletSpeed, damage, impact);

                CurrentBulletUp();
            }
        }
        //is not aiming
        else if (!isAim)
        {
            animator.Play("Shooting", -1, 0.0f);
            //gun go up when shoot
            transform.position += transform.up * Time.deltaTime * recoilMultiplier;
            transform.position += transform.forward * Time.deltaTime * Random.Range(-recoilMultiplier, recoilMultiplier);
            if (shotgun)
            {
                for (int i = 0; i < pellets; i++)
                {
                    //create random direction for bullet
                    Quaternion result = Quaternion.Euler(new Vector3(shootFromPoint.transform.eulerAngles.x + Random.Range(-hipfireSpread, hipfireSpread),
                        shootFromPoint.transform.eulerAngles.y + Random.Range(-hipfireSpread, hipfireSpread),
                        shootFromPoint.transform.eulerAngles.z));

                    GameObject bullet = bullets[currentBullet];
                    bullet.transform.position = shootFromPoint.transform.position;
                    bullet.transform.rotation = result;
                    bullet.GetComponent<DefaultBullet>().Activate(bulletSpeed, damage, impact);

                    CurrentBulletUp();
                }
            }
            else
            {
                Rigidbody plrb = GameObject.Find("Player").GetComponent<Rigidbody>();
                //player is standing still or close to
                if (plrb.velocity.x < 2 && plrb.velocity.y < 2 && plrb.velocity.z < 2 && GameObject.Find("Player").GetComponent<PlayerController>().moving == false)
                {
                    GameObject bullet = bullets[currentBullet];
                    bullet.transform.position = shootFromPoint.transform.position;
                    bullet.transform.rotation = shootFromPoint.transform.rotation;
                    bullet.GetComponent<DefaultBullet>().Activate(bulletSpeed, damage, impact);

                    CurrentBulletUp();
                }
                else
                {
                    //create random direction for bullet
                    Quaternion result = Quaternion.Euler(new Vector3(shootFromPoint.transform.eulerAngles.x + Random.Range(-hipfireSpread, hipfireSpread),
                           shootFromPoint.transform.eulerAngles.y + Random.Range(-hipfireSpread, hipfireSpread),
                           shootFromPoint.transform.eulerAngles.z));
                    GameObject bullet = bullets[currentBullet];
                    bullet.transform.position = shootFromPoint.transform.position;
                    bullet.transform.rotation = result;
                    bullet.GetComponent<DefaultBullet>().Activate(bulletSpeed, damage, impact);

                    CurrentBulletUp();
                }
            }
        }
        //one less bullet in magazine
        bulletsInMagazine--;
    }

    //actions have timer to limit use

    //is aiming
    IEnumerator IsAiming()
    {
        if(crosshair != null)
            crosshair.SetActive(false);
        playingAnimation = true;
        isAim = true;
        animator.SetBool("Aiming", true);
        animator.Play("Aiming");
        yield return new WaitForSeconds(0.4f);
    }
    //is not aiming anymore
    IEnumerator IsDeAiming()
    {
        if(crosshair != null)
            crosshair.SetActive(true);
        playingAnimation = true;
        isAim = false;
        animator.Play("UnAim");
        animator.SetBool("Aiming", false);
        yield return new WaitForSeconds(0.4f);
        playingAnimation = false;
    }
    //reload
    public IEnumerator Reload()
    {
        playingAnimation = true;
        animator.SetBool("Reloading", false);
        animator.Play("Reload");
        audioSource.clip = reloadAudio;
        audioSource.Play();
        reloading = true;
        yield return new WaitForSeconds(reloadTime);
        bulletsInMagazine = magazineSize;
        reloading = false;
        playingAnimation = false;
    }
    //switch firemode
    public IEnumerator SwitchMode()
    {
        playingAnimation = true;
        isAutomatic = !isAutomatic;
        animator.Play("SwitchMode");
        audioSource.clip = switchAudio;
        audioSource.Play();
        yield return new WaitForSeconds(1f);
        playingAnimation = false;
    }

    //gun go back to original pos
    void FixGunPosition()
    {
        if (isAim)
        {
            transform.localPosition += recoilOffset * recoilMultiplier * Time.deltaTime * 2;
        }
        else if (!isAim)
        {
            transform.localPosition += recoilOffset * Time.deltaTime * 2;
        }
    }
    //up current bullet
    void CurrentBulletUp()
    {
        currentBullet++;
        if (currentBullet >= magazineSize)
        {
            currentBullet = 0;
        }
    }
    //create bullets
    void CreateBullets()
    {
        for (int i = 0; i < magazineSize * pellets; i++)
        {
            bullets.Add(Instantiate(bullet));
            bullets[i].transform.parent = GameObject.Find("Bullets").transform;
            bullets[i].GetComponent<Renderer>().enabled = false;
        }
    }
    //change values
    public void ChangeFireRate(float newFireRate)
    {
        fireRate = newFireRate;
    }
    public void ChangeMode()
    {
        if (canChangeMode == true && !playingAnimation)
        {
            StartCoroutine(SwitchMode());
        }
    }
}