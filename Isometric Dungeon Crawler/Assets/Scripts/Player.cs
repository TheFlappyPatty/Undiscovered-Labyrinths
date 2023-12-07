using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int Health;
    public Image HealthBar;
    private bool shooting = true;
    public static GameObject Checkpoint;
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



    public void Start()
    {
        if (Input.GetJoystickNames().Length == 0)
        {
            ControllerConnected = false;
        } else
        {
            ControllerConnected = true;
        }

    }
    public void Update()
    {
        HealthBar.fillAmount = Health;

        if(ControllerConnected == false) {
            var MoveDir = new Vector3(Input.GetAxis("Horizontal") * MovementSpeed, -1, Input.GetAxis("Vertical") * MovementSpeed);
            PlayerController.Move(MoveDir * Time.deltaTime);

            gameObject.transform.eulerAngles = new Vector3(0, dummypoint.transform.eulerAngles.y, 0);
            dummypoint.transform.LookAt(mousepoint.transform.position);
            mousepoint.transform.position = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
        }
        else
        {
            var MoveDir = new Vector3(Input.GetAxis("ControllerH") * MovementSpeed, -1, Input.GetAxis("ControllerV") * MovementSpeed);
            PlayerController.Move(MoveDir * Time.deltaTime);

            CrossHair.transform.localPosition = transform.localPosition + new Vector3(-Input.GetAxis("ControllerVRIght") * 2,0, -Input.GetAxis("ControllerHRight") * 2);
            transform.LookAt(CrossHair.transform);
            if(Input.GetAxis("Fire1C") == 1 && shooting == true)
            {
                StartCoroutine(Shooting(Currentammo, CurrentFirerate, CurrentDamage, CurrentVelocity, CurrentlifeTime));
                shooting = false;
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
    public IEnumerator Shooting(Rounds Ammotype,float fireRate,int Damage,int velocity,float lifetime)
    {
     if (Rounds.lazar == Ammotype) {
            RaycastHit target;
            Ray ray = new Ray(transform.position, transform.forward.normalized);
            if (Physics.Raycast(ray, out target, 50))
            {
                GetComponent<LineRenderer>().SetPosition(0,transform.position);
                GetComponent<LineRenderer>().SetPosition(1,target.point);
                if (target.collider.tag == "Enemy")
                {
               target.transform.gameObject.GetComponent<EnemyAi>().Health -= 50;
                }
            }
            Ammo -= 1;
            yield return new WaitForSeconds(0.05f);
            shooting = true;
        } else {
            shooting = false;
            var bullet = Instantiate(StandardAmmo, transform.position,transform.rotation, null);
            bullet.gameObject.GetComponent<Bullets>().Type = Ammotype;
            bullet.gameObject.GetComponent<Bullets>().bulletspeed = velocity;
            if (gun == Gun.MiniGun) bullet.transform.eulerAngles = new Vector3(bullet.transform.eulerAngles.x, bullet.transform.eulerAngles.y + Random.Range(-10, 10), bullet.transform.eulerAngles.z);
            bullet.gameObject.GetComponent<Bullets>().lifetime = lifetime;
            bullet.gameObject.GetComponent<Bullets>().Damage = Damage;
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
    public void die()
    {
        RestoreDefault();
        SceneManager.LoadScene(0);
        gameObject.transform.position = Checkpoint.transform.position;
    }
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
            gameObject.GetComponent<LineRenderer>().startColor = Color.black;
            gameObject.GetComponent<LineRenderer>().endColor = Color.black;
            gameObject.GetComponent<LineRenderer>().startWidth = 0.05f;
            gameObject.GetComponent<LineRenderer>().endWidth = 0.05f;
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