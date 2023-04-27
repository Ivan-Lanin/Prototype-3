using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveToTheLeft : MonoBehaviour
{

    private float speed = 20;
    private float leftBound = -15;
    private PlayerController playerControllerScript;
    private bool gameStarted = false;
    
    [SerializeField] bool isABackground;


    private void Start() {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        PlayerController.OnDashModeEnter += DashModeOn;
        PlayerController.OnDashModeExit += DashModeOff;
        PlayerController.OnGameStarts += GameStarts;
    }

    void Update()
    {
        if (!gameStarted && isABackground) {
            return;
        }

        if (!playerControllerScript.GetGameOver()) { 
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle")) {
            Destroy(gameObject);
        }
    }

    private void DashModeOn() {
        speed = 30;
    }

    private void DashModeOff() {
        speed = 20;
    }

    private void GameStarts() {
        gameStarted = true;
    }

    private void OnDisable() {
        PlayerController.OnDashModeEnter -= DashModeOn;
        PlayerController.OnDashModeExit -= DashModeOff;
        PlayerController.OnGameStarts -= GameStarts;
    }
}
