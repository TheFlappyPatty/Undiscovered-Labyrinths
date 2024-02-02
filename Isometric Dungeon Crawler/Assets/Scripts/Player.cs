using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class Player : MonoBehaviour
{
    [Header("Player Utility")]
    //Movement
    public float MovementSpeed;
    public Camera playerCam;
    public float speedCap;
    public CharacterController PlayerController;
    public Rigidbody Playerrig;
    private bool ControllerConnected;
    public bool InEncounter;

    [Space]
    //Player stats
    [Header("Player stats")]
    public int Ammo;
    public float Health;
    public Image HealthBar;
    public Slider AmmoBar;
    private bool shooting = true;
    public static Vector3 Checkpoint;
    public static bool Movetoggle = false;
    public Rounds WeaponCurrent;

    [Space]
    [Header("Function stuff")]
    //Current Weaponstats
    private Rounds Currentammo = Rounds.standard;
    private Gun gun;
    private float CurrentFirerate = 480;
    private int CurrentDamage = 10;
    private int CurrentVelocity = 15;
    private float CurrentlifeTime = 10;
    private bool defaultisequiped;
    public GameObject StandardAmmo;
    public Material LaserTexture;
    //mouse tracking and controller
    public GameObject CrossHair;
    public GameObject mousepoint;
    public GameObject dummypoint;

    [Space]
    [Header("Pause Menu")]
    public Canvas PauseMenu;
    public Canvas PlayerUi;
    private bool GameIsPaused = false;



    public void Start()
    {
        StartCoroutine(CheckpointCheck());

        if (Input.GetJoystickNames().Length == 0)
        {
            ControllerConnected = false;
        } else
        {
            ControllerConnected = true;
        }

    }
    public IEnumerator CheckpointCheck()
    {

        if (Checkpoint == new Vector3(0,0,0))
        {
            //gameObject.transform.position = Checkpoint;
        }
        else
        {

            Movetoggle = true;
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.position = Checkpoint;
            yield return new WaitForSeconds(0.01f);
            Movetoggle = false;
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(Checkpoint);
            StartCoroutine(CheckpointCheck());
        }
        HealthBar.fillAmount = Health/100;
        Health = Mathf.Clamp(Health, 0, 100);
        if (ControllerConnected == false)
        {
            var MoveDir = new Vector3(Input.GetAxis("Horizontal") * MovementSpeed, -1, Input.GetAxis("Vertical") * MovementSpeed);
            if(Movetoggle == false)PlayerController.Move(MoveDir * Time.deltaTime);

            gameObject.transform.eulerAngles = new Vector3(0, dummypoint.transform.eulerAngles.y, 0);
            dummypoint.transform.LookAt(mousepoint.transform.position);
            mousepoint.transform.position = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 50));
            if (Input.GetAxis("Fire1") == 1 && shooting == true)
            {
                StartCoroutine(Shooting(Currentammo, CurrentFirerate, CurrentDamage, CurrentVelocity, CurrentlifeTime));
                shooting = false;
            }
        }
        else
        {
            var MoveDir = new Vector3(Input.GetAxis("ControllerH") * MovementSpeed, -1, Input.GetAxis("ControllerV") * MovementSpeed);
            if(Movetoggle == false)PlayerController.Move(MoveDir * Time.deltaTime);

            CrossHair.transform.localPosition = transform.localPosition + new Vector3(-Input.GetAxis("ControllerVRIght") * 2,0, -Input.GetAxis("ControllerHRight") * 2);
            transform.LookAt(CrossHair.transform);
            if(Input.GetAxis("Fire1C") == 1 && shooting == true)
            {
                if(gun != Gun.Shotgun)
                {
                    StartCoroutine(Shooting(Currentammo, CurrentFirerate, CurrentDamage, CurrentVelocity, CurrentlifeTime));
                }
                else
                {
                    var shots = 5;
                    while(shots-- > 0)
                    {
                        StartCoroutine(Shooting(Currentammo, CurrentFirerate, CurrentDamage, CurrentVelocity, CurrentlifeTime));
                    }
                }
                shooting = false;
            }
            AmmoBar.value = Ammo;
            if(Input.GetButtonDown("Pause"))
            {
                if(GameIsPaused == true)
                {
                    unPause();
                }
                else
                {
                    Pause();
                }
            }
        }

        if (Health <=0)
        {
            die();
        }
        if (InEncounter)
        {

        }
        else
        {
            var Campos = new Vector3(transform.localPosition.x,transform.localPosition.y + 16f,transform.localPosition.z - 13f);
            playerCam.transform.localPosition = Campos;
        }

        if (Playerrig.velocity.magnitude > speedCap)
        {
            Playerrig.velocity = Playerrig.velocity.normalized * speedCap;
        }
    }

    public void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0;
        PauseMenu.enabled = true;
        PlayerUi.enabled = false;
    }
    public void unPause()
    {
        GameIsPaused = false;
        Time.timeScale = 1;
        PauseMenu.enabled = false;
        PlayerUi.enabled = true;
    }

    //When the player shoots the gun
    public IEnumerator Shooting(Rounds Ammotype,float fireRate,int Damage,int velocity,float lifetime)
    {
     if (Rounds.lazar == Ammotype) {
            if(shooting == true)
            {
                shooting = false;
            RaycastHit target;
            Ray ray = new Ray(transform.position, transform.forward.normalized);
            if (Physics.Raycast(ray, out target, 50))
            {
                GetComponent<LineRenderer>().SetPosition(0,transform.position);
                GetComponent<LineRenderer>().SetPosition(1,target.point);
                if (target.collider.tag == "Enemy")
                {
                    target.transform.gameObject.GetComponent<EnemyAi>().Health -= Damage;
                }
                    else if (target.collider.tag == "ActivePillar")
                    {
                        BossScript.bossHealth -= Damage;
                    }
                }
                Ammo -= 1;
                yield return new WaitForSeconds(0.05f);
                shooting = true;
            }

        } else {
            shooting = false;

                var bullet = Instantiate(StandardAmmo, transform.position, transform.rotation, null);
            bullet.gameObject.GetComponent<Bullets>().Type = Ammotype;
            bullet.gameObject.GetComponent<Bullets>().bulletspeed = velocity;
            if (gun == Gun.MiniGun) bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x, bullet.transform.eulerAngles.y + Random.Range(-10, 10), bullet.transform.eulerAngles.z);
            if (gun == Gun.Shotgun) bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x, bullet.transform.eulerAngles.y + Random.Range(-15, 15), bullet.transform.eulerAngles.z);
            bullet.gameObject.GetComponent<Bullets>().lifetime = lifetime;
            bullet.gameObject.GetComponent<Bullets>().Damage = Damage;
            bullet.gameObject.GetComponent<Bullets>().weapion = gun;
            Ammo -= 1;
            yield return new WaitForSeconds(60 / fireRate);
            shooting = true;
     }
        if(Ammo <= 0 && defaultisequiped == false)
        {
            if(Currentammo == Rounds.lazar)
            {
                Destroy(gameObject.GetComponent<LineRenderer>());
            }
            RestoreDefault();
        }
    }
    //when the player dies
    public void die()
    {
        RestoreDefault();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //resets the gun to defaults
    public void RestoreDefault()
    {
        WeaponCurrent = Currentammo;
      Currentammo = Rounds.standard;
      CurrentFirerate = 480;
      CurrentDamage = 10;
      CurrentVelocity = 15;
      CurrentlifeTime = 10;
      defaultisequiped = true;
        gun = Gun.None;
    }




    //Defines weapon stats and ammoType
    public void Pickedupweapon(Rounds ammotype, float Firerate, int Damage, int Velocity, float Lifetime,Gun pew)
    {
        WeaponCurrent = ammotype;
        Currentammo = ammotype;
        CurrentFirerate = Firerate;
        CurrentDamage = Damage;
        CurrentVelocity = Velocity;
        CurrentlifeTime = Lifetime;
        defaultisequiped = false;
        gun = pew;
        if(ammotype == Rounds.lazar)
        {
            gameObject.AddComponent<LineRenderer>();
            gameObject.GetComponent<LineRenderer>().startColor = Color.white;
            gameObject.GetComponent<LineRenderer>().endColor = Color.red;
            gameObject.GetComponent<LineRenderer>().startWidth = 0.2f;
            gameObject.GetComponent<LineRenderer>().endWidth = 0.2f;
            gameObject.GetComponent<LineRenderer>().positionCount = 2;
            gameObject.GetComponent<LineRenderer>().useWorldSpace = true;
            gameObject.GetComponent<LineRenderer>().material = LaserTexture;
        }
    }
}
public enum Rounds{
    standard,
    Explosive,
    lazar
}
