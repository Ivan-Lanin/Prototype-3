using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField] private GameObject[] obstaclePrefabs;
    private Vector3 spawnPos = new Vector3(25,0,0);
    private float startDelay = 2;
    private float repeatRate = 2;
    private PlayerController playerControllerScript;
    private int prefabN;
    private bool gameStarted = false;

    private void Start()
    {
        InvokeRepeating("SpawnObstacle", startDelay, repeatRate);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        PlayerController.OnGameStarts += GameStarts;
    }

    private void Update()
    {

    }

    private void SpawnObstacle() {
        if (playerControllerScript.GetGameOver() == false && gameStarted) {
            prefabN = Random.Range(0, 3);
            Instantiate(obstaclePrefabs[prefabN], spawnPos, obstaclePrefabs[prefabN].transform.rotation);
        }
    }

    private void GameStarts() {
        gameStarted = true;
    }

    private void OnDisable() {
        PlayerController.OnGameStarts -= GameStarts;
    }
}
