﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum transformations
{
    GIGANT,
    NORMAL,
    SMALL

}

public enum Comport
{
    IDDLE,
    AIR,
    CARGANDO

}

public class HeroBody : MonoBehaviour
{

    public float walkSpeed ;
    public float runSpeed ;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;

    Animator animator;
    Transform cameraT;
    public transformations state = transformations.NORMAL;
    public Comport actions = Comport.IDDLE;
    public Rigidbody rb;
    private Vector3 initcale;
    public Vector3 inputDir;
    public float jumpForce = 5;
    private BoxScript bxSript;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        initcale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Big()
    {
        if (state != transformations.GIGANT)
        {
            transform.localScale = initcale;
            transform.localScale += new Vector3(0.8F, 0.8F, 0.8F);
            state = transformations.GIGANT;
        }
    }

    public void Small()
    {
        if (state != transformations.SMALL)
        {
            transform.localScale = initcale;
            transform.localScale -= new Vector3(0.8F, 0.8F, 0.8F);
            state = transformations.SMALL;

        }
    }

    public void Normal()
    {
        if(state != transformations.NORMAL) {
            transform.localScale = initcale;
            state = transformations.NORMAL;
        }

    }
    

    public void Jump()
    {
        if(state == transformations.GIGANT) return;
        if(actions == Comport.AIR) return;
        if(actions == Comport.CARGANDO) return;
        
        
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        actions = Comport.AIR;
    }

    public void Move(Vector3 dir) {


        inputDir = dir.normalized;

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        //currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);
        currentSpeed = targetSpeed;

        //if (inputDir != Vector3.zero && dir.z > 0)
        //{
        //float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
        //transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);





        //    float animationSpeedPercent = ((running) ? 1 : .5f) * inputDir.magnitude;
        //}
        transform.position += dir * currentSpeed * Time.deltaTime;
        //transform.LookAt(target.transform);
        //animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);


    }
    public void Activate()
    {

        RaycastHit hit;
        //if (state == transformations.GIGANT) {
        if (actions == Comport.CARGANDO)
        {
            bxSript.Soltar();
            actions = Comport.IDDLE;
        }

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.yellow);
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10))
        {
            if (hit.transform.gameObject.layer == 10)
            {
                bxSript = hit.transform.GetComponent<BoxScript>();
                bxSript.Agarre();
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.red);
                Debug.Log("Did Hit");
                actions = Comport.CARGANDO;
            }
        }


        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            actions = Comport.IDDLE;

        }

    }
}
