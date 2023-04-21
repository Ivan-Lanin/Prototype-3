using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTheLeft : MonoBehaviour
{

    private float speed = 20;
    private float leftBound = -15;
    private PlayerController playerControllerScript;


    private void Start() {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!playerControllerScript.GetGameOver()) { 
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle")) {
            Destroy(gameObject);
        }
    }
}
