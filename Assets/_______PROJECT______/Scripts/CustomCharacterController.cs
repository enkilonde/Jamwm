using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomCharacterController : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;

    public float dodgeSpeed;
    public float dodgeTime;

    public CharacterController characterController;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Move(Vector2 direction)
    {
        characterController.Move(new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime);
    }

    public void Turn(Vector2 vector2)
    {
        Quaternion oldRot= characterController.transform.rotation;
        characterController.transform.LookAt(transform.position + new Vector3(vector2.x, 0, vector2.y));
        characterController.transform.rotation = Quaternion.Slerp(oldRot, characterController.transform.rotation, Time.deltaTime * turnSpeed);
    }    
    
    public void Dodge()
    {

    }

    public void ChargeAttackLeft()
    {

    }

    public void LaunchAttackLeft()
    {

    }

    public void ChargeAttackRight()
    {

    }

    public void LaunchAttackRight()
    {

    }
}
