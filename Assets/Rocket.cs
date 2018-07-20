﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour 
{

    Rigidbody rigidBody;
    AudioSource rocketSound;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        ProcessInput();    	
	}

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))  //Can thrust whilst rotating
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rocketSound.Play();
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            rocketSound.Stop();
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }
    }
}
