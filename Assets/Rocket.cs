﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour 
{
    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 15f;
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip levelingSound;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem levelingParticles;

    

    Rigidbody rigidBody;
    AudioSource rocketSound;

    int currentLevel;
    int maxLevel;
    bool isTesting = false;

    enum State { Alive, Dying, Levelling };
    State state = State.Alive;


    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rocketSound = GetComponent<AudioSource>();
        currentLevel = SceneManager.GetActiveScene().buildIndex;
        maxLevel = SceneManager.sceneCountInBuildSettings -1;
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        RespondToDebugKeys();

        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    


    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))  //Can thrust whilst rotating
        {
            ApplyThrust();
        }
        else
        {
            rocketSound.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        if (!rocketSound.isPlaying)
        {
            rocketSound.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
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

    private void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCollisions();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || isTesting) {return;}

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void ToggleCollisions()
    {
        if (isTesting)
        {
            isTesting = !isTesting;
            print("Collisons On - Game Mode");
        }
        else
        {
            isTesting = !isTesting;
            print("Collisions Off - Test Mode");
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Levelling;
        rocketSound.Stop();
        rocketSound.PlayOneShot(levelingSound);
        levelingParticles.Play();
        Invoke("LoadNextScene", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        FindObjectOfType<DeathScript>().StartDeath(gameObject.transform.position,levelLoadDelay);
        Destroy(gameObject);
     }

    private void LoadNextScene()
    {
        if (currentLevel >= maxLevel)
        {
            SceneManager.LoadScene(0);
        }         
        else
        {
            SceneManager.LoadScene(currentLevel + 1);
        }
    }
}
