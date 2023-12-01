using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MovementSpeed;
    public Camera playerCam;
    public float speedCap;
    public CharacterController PlayerController;
    public Rigidbody Playerrig;
    public GameObject mousepoint;
    public GameObject dummypoint;
    public void Update()
    {
        var MoveDir = new Vector3(Input.GetAxis("Horizontal") * MovementSpeed, -1, Input.GetAxis("Vertical") * MovementSpeed);
        PlayerController.Move(MoveDir * Time.deltaTime);
        if (Playerrig.velocity.magnitude > speedCap)
        {
            Playerrig.velocity = Playerrig.velocity.normalized * speedCap;
        }
        //RaycastHit hit;
        //if(Physics.Raycast(Camera.current.transform.position, playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20)).normalized,out hit, Mathf.Infinity))
        //{
        //    Debug.Log(hit.point.y);
        //}
        gameObject.transform.eulerAngles = new Vector3(0,dummypoint.transform.eulerAngles.y,0);
        dummypoint.transform.LookAt(mousepoint.transform.position);
        mousepoint.transform.position = playerCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,20));





    }
}
