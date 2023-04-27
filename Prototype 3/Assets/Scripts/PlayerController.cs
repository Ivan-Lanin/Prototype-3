using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour {

    public static event Action OnDashModeEnter;
    public static event Action OnDashModeExit;
    public static event Action OnGameStarts;

    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    private bool isOnGround = true;
    private bool gameOver = false;
    private int jumpCount = 0;
    private int score = 0;
    private bool dashMode = false;
    private bool gameStarted = false;

    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityModifier;
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private ParticleSystem dirtParticle;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip crashSound;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateScore", 1, 1);
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update() 
    {
        if (!gameStarted) {
            transform.Translate(Vector3.forward * Time.deltaTime * 2);
            if (transform.position.x >= 0) {
                gameStarted = true;
                OnGameStarts?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && CanJump()) {
            jumpCount++;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isOnGround = false;
            playerAnim.SetTrigger("Jump_trig");
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }

        if (Input.GetKeyDown(KeyCode.W) && CanDash()) {
            OnDashModeEnter?.Invoke();
            dashMode = true;
        }

        if (Input.GetKeyUp(KeyCode.W)) {
            OnDashModeExit?.Invoke();
            dashMode = false;
        }
    }

    private bool CanJump() {
        if (gameOver) {
            return false;
        }
        if (!isOnGround) {
            return false;
        }
        if (jumpCount >= 2) {
            return false;
        }
        if (!gameStarted) {
            return false;
        }
        return true;
    }

    private bool CanDash() {
        if (gameOver) {
            return false;
        }
        if (!isOnGround) {
            return false;
        }
        if (!gameStarted) {
            return false;
        }
        return true;
    }


    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            isOnGround = true;
            jumpCount = 0;
            dirtParticle.Play();
        }
        else if (collision.gameObject.CompareTag("Obstacle")) {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
        }
    }

    public bool GetGameOver() {
        return gameOver;
    }

    private void UpdateScore() {
        score++;
        if (dashMode) {
            score++;
        }
        Debug.Log(score);
    }
}
