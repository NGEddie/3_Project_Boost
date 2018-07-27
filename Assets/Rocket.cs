using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour 
{

    Rigidbody rigidBody;
    AudioSource rocketSound;
    [SerializeField] float  rcsThrust = 250f;
    [SerializeField] float mainThrust = 15f;

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Thrust();
        Rotate();  	
	}

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))  //Can thrust whilst rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!rocketSound.isPlaying)
            {
                rocketSound.Play();
            }
        }
        else
        {
            rocketSound.Stop();
        }
    }

    private void Rotate()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Bump");
                break;
            default:
                print("You're Dead");
                break;
        }
    }

}
