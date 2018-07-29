using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScript : MonoBehaviour {


    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] AudioClip explosionSound;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDeath(Vector3 deathPosition)
    {
        gameObject.transform.position = deathPosition;
        explosionParticles.Play();
        audioSource.PlayOneShot(explosionSound);

        Invoke("LoadStartScene", 1f);
    }

    private void LoadStartScene()
    {
        SceneManager.LoadScene(0);
    }
}
