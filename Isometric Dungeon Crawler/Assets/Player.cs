using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int MovementSpeed;
    public CharacterController PlayerController;
    public void Update()
    {
        var MoveDir = new Vector3(Input.GetAxis("Horizontal") * MovementSpeed,0,Input.GetAxis("Vertical") * MovementSpeed);
        PlayerController.Move(MoveDir);
    }
}
