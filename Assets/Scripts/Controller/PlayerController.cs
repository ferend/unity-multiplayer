using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float verticalLookRotation;
    public float mouseSensitivity;
    public Camera playerCam;
    public static PlayerController _instance;
    public float sprintSpeed;
    public float walkSpeed;
    public float smoothTime;
    private Vector3 moveAmount;
    private bool grounded;
    private Vector3 smoothMoveVelocity;
    private Rigidbody rb;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    private void Update()
    {
        PlayerRotation();
        PlayerMovement();
    }
    public void PlayerRotation()
    {
        //var mousePos = playerCam.ScreenToWorldPoint(Input.mousePosition);
         transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        
        playerCam.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    public void PlayerMovement()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
         moveAmount = Vector3.SmoothDamp(moveAmount,moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed),ref
            smoothMoveVelocity , smoothTime);

         if (Input.GetKeyDown(KeyCode.Space) && grounded)
         {
             rb.AddForce(transform.up * 200 );
         }
    }

    public void JumpState(bool _grounded)
    {
        grounded = _grounded;
    }

}
