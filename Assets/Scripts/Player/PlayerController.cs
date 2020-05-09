using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct PlayerController_InitValues
{
    public int movementSpeed;
    public int jumpHeight;
    public int jumpLimit;
    public int fallSpeed;
    public int damageShieldTime;

    public int flyingFill;
    public int flyingUseRate;
    public int flyingRecovoryRate;
    public int flyingPower;
}

public class PlayerController : MonoBehaviour
{

    //playercontroller from Top Quality Arena
    //able to walk, jump, crouch
    //a bit updated

    //variables
    [Header("Variables")]
    public int health = 100;
    public float movementSpeed = 50;
    public float jumpHeight = 20;
    public float jumpLimit = 5;
    public float fallSpeed = 1;
    public float damageShieldTime = 1;

    bool crouching = false;
    [HideInInspector]
    public bool moving = false;
    bool active = true;
    bool ableToBeDamaged = true;

    [Header("Jetpack")]
    public float flyingFill = 1.0f;
    public float flyingUseRate = 1.0f;
    public float flyingRecovoryRate = 1.0f;
    public float flyingPower = 2.0f;
    float flyingFillMax;
    bool flying = false;

    [Header("Game Objects")]
    public GameObject playerPerspectiveCamera;
    public GameObject hurtEffect;
    public GameObject loseBtn;
    public Text healthText;
    public Slider flyingSlider;

    //scripts, components
    Rigidbody rb;
    BoxCollider boxCol;
    GameManager gameManager;
    DataManager dataManager;
    Plane plane = new Plane(Vector3.up, Vector3.zero);

    void Start()
    {
        flyingFillMax = flyingFill;
        flyingSlider.maxValue = flyingFill;
        flyingSlider.value = flyingFill;
        
        rb = GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        healthText.text = health.ToString();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (GameObject.Find("DataManager"))
        {
            CheckForUpgrades(GameObject.Find("DataManager").GetComponent<DataManager>().playerController_Values);
        }
    }



    void Update()
    {
        if (active)
        {
            if(gameManager.perspective == Player_Perspective.TOP_DOWN)
            {
                Input_TopDown();
            }
            else if(gameManager.perspective == Player_Perspective.FIRST_PERSON)
            {
                Input_FirstPerson();
            }
        }
    }

    void Input_FirstPerson()
    {
        //basic movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * movementSpeed * Time.deltaTime;
            moving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * movementSpeed * Time.deltaTime;
            moving = true;
        }

        //jump
        //must be near to ground (near, not on)
        if (Input.GetKeyDown(KeyCode.Space)
            && Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), jumpLimit))
        {
            rb.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }
        if (rb.velocity.y < -2)
        {
            rb.velocity += Vector3.down * fallSpeed;
        }

        flying = true;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), jumpLimit))
        {
            flying = false;
        }

        if (Input.GetKey(KeyCode.Space) && flying)
        {
            if (flyingFill > 0)
            {
                rb.AddForce(Vector3.up * flyingPower, ForceMode.Force);
                flyingFill -= Time.deltaTime * flyingUseRate;
                flyingSlider.value = flyingFill;
            }
        }

        if (flyingFill < flyingFillMax)
        {
            flyingFill += Time.deltaTime * flyingRecovoryRate;
            flyingSlider.value = flyingFill;
        }

        //crouch mode
        //will move slower
        //shorter boxcollider
        //player will have to move to make room for new collider
        if (Input.GetKeyDown(KeyCode.LeftControl) && crouching == false)
        {
            boxCol.size = new Vector3(1, 1, 1);
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
            crouching = !crouching;
            movementSpeed -= 5;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && crouching == true || Input.GetKeyDown(KeyCode.Space) && crouching == true)
        {
            boxCol.size = new Vector3(1, 2, 1);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            crouching = !crouching;
            movementSpeed += 5;
        }

        //make player y rotation same as camera
        transform.rotation = Quaternion.Euler(0, playerPerspectiveCamera.transform.eulerAngles.y, 0);

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            StartCoroutine(notMoving());
        }

        if ( Input.GetKeyUp( KeyCode.P ) )
        {
            DebugManager.GetInstance().ChangeConsoleState();
        }
    }

    void Input_TopDown()
    {
        //Look at Mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }

        //basic movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * movementSpeed * Time.deltaTime;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.forward * movementSpeed * Time.deltaTime;
            moving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * movementSpeed * Time.deltaTime;
            moving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Vector3.right * movementSpeed * Time.deltaTime;
            moving = true;
        }

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            StartCoroutine(notMoving());
        }
    }
    //damage player
    public void Damage(int amount)
    {
        if (ableToBeDamaged)
        {
            ableToBeDamaged = false;
            health -= amount;
            healthText.text = health.ToString();
            hurtEffect.GetComponent<Animator>().Play("PlayerHurt");
            if (health <= 0 && active)
            {
                active = false;
                loseBtn.SetActive(true);
                if(Camera.main.GetComponent<CameraController>())
                    Camera.main.GetComponent<CameraController>().ChangeState(false);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            StartCoroutine(damageShield());
        }
    }

    public void ChangeState(bool newState)
    {
        active = newState;
    }

    IEnumerator notMoving()
    {
        yield return new WaitForSeconds(0.6f);
        moving = false;
    }

    IEnumerator damageShield()
    {
        yield return new WaitForSeconds(damageShieldTime);
        ableToBeDamaged = true;
    }

    void CheckForUpgrades(PlayerController_InitValues values)
    {

        if (PlayerPrefs.HasKey("ImmunityTimeUpgrade"))
            damageShieldTime += 0.1f * values.damageShieldTime;
        if (PlayerPrefs.HasKey("FallSpeedUpgrade"))
            fallSpeed += 0.08f * values.fallSpeed;
        if (PlayerPrefs.HasKey("JumpLimitUpgrade"))
            jumpLimit += 0.1f * values.jumpLimit;
        if (PlayerPrefs.HasKey("JumpHeightUpgrade"))
            jumpHeight += 0.1f * values.jumpHeight;
        if (PlayerPrefs.HasKey("SpeedUpgrade"))
            movementSpeed += 0.7f * values.movementSpeed;
        if (PlayerPrefs.HasKey("JetpackChargeUpgrade"))
            flyingFill += 0.2f * values.flyingFill;
        if (PlayerPrefs.HasKey("JetpackUseRateUpgrade"))
            flyingUseRate += 0.06f * values.flyingUseRate;
        if (PlayerPrefs.HasKey("JetpackRecovoryRateUpgrade"))
            flyingRecovoryRate += 0.06f * values.flyingRecovoryRate;
        if (PlayerPrefs.HasKey("JetpackPowerUpgrade"))
            flyingPower += 0.2f * values.flyingPower;
    }
}
