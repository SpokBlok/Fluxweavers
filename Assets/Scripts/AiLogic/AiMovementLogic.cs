using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMovementLogic : MonoBehaviour
{

    [SerializeField] Transform aspirant; // Should be a list to accomodate more aspirant
    [SerializeField] int movement;
    [SerializeField] int moveSpeed;

    private Vector3 moveX;
    private Vector3 moveY;
    [SerializeField] private int remainingMoves;
    private Vector3[] adjancenctTiles;

    // Start is called before the first frame update
    void Start()
    {
        moveX = new Vector3(1f, 0f, 0f); // Change depending on hex size and dimensions
        moveY = new Vector3(0.5f, 0.9f, 0f); // Change depending on hex size and dimensions
        remainingMoves = movement;

        adjancenctTiles = new Vector3[6];

        adjancenctTiles[0] = moveX;
        adjancenctTiles[1] = moveY;
        adjancenctTiles[2] = -moveX + moveY;
        adjancenctTiles[3] = -moveX;
        adjancenctTiles[4] = -moveY;
        adjancenctTiles[5] = moveX - moveY;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Move(moveSpeed * Time.deltaTime);
        }

        // if ( && remainingMoves < 1) {
        //     remainingMoves = movement;
        // }
    }

    void Move(float speed) {
        Vector3 enemy_position = gameObject.transform.position;

        if (aspirant.position.y > enemy_position.y) {
            transform.position += moveY;

            if (aspirant.position.x < enemy_position.x) {
                transform.position -= moveX;
            }
        }

        else if (aspirant.position.y < enemy_position.y) {
            transform.position -= moveY;

            if (aspirant.position.x > enemy_position.x) {
                transform.position += moveX;
            }
        }

        else if (aspirant.position.x > enemy_position.x) {
            transform.position += moveX;
        }

        else if (aspirant.position.x < enemy_position.x) {
            transform.position -= moveX;
        }


        Debug.Log("moving...");
        // aspirant.transform.position;
    }

}
