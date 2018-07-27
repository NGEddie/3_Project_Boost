using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour 
{

    Rigidbody rigidBody;
    AudioSource rocketSound;

    enum State { Alive, Dying, Levelling };

    State state = State.Alive;

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
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
	
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
        if(state != State.Alive)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.Levelling;
                Invoke("LoadNextScene",1f);
                break;
            default:
                state = State.Dying;
                Invoke("LoadStartScene",1f);
                break;
        }
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }
}
