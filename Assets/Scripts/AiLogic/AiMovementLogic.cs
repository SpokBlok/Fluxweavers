using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiMovementLogic : MonoBehaviour
{

    [SerializeField] Transform aspirant; // Should be a list to accomodate more aspirant
    [SerializeField] int movement;
    [SerializeField] int moveSpeed;

    [SerializeField] private int currentYIndex;
    [SerializeField] private int currentXIndex;

    private Vector3 moveX;
    private Vector3 moveY;
    [SerializeField] private int remainingMoves; // For debugging. Don't remove me yet >_>
    private Vector3[] adjacentTiles;

    public AspirantMovement movementScript;

    // Start is called before the first frame update
    void Start()
    {
        moveX = new Vector3(1f, 0f, 0f); // Change depending on hex size and dimensions
        moveY = new Vector3(0.5f, 0.9f, 0f); // Change depending on hex size and dimensions
        remainingMoves = 0; // Don't move at start

        adjacentTiles = new Vector3[] { // Arranged in counter-clockwise order
            moveX, moveY, -moveX + moveY, -moveX, -moveY, moveX - moveY
        };
    }

    // Update is called once per frame
    void Update()
    {

        if (AdjancentTo(aspirant)) { // Change aspirant depending on AI targeting priority
            remainingMoves = 0; // Should be changed to simply disallow movement to aspirant tile // <- TODO
        }

        if (remainingMoves > 0) { 
            // StartCoroutine(Move(moveSpeed * Time.deltaTime));
            Move(moveSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && remainingMoves < 1) {
            remainingMoves = movement;
        }
    }


    // Move should be updated eventually to take in ANY set of position vectors as parameter // <- TODO
    private void Move(float speed) {
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

        remainingMoves--;
        Debug.Log("moving...");
        // aspirant.transform.position;

        // yield return new WaitForSeconds(1);
    }

    private bool AdjancentTo (Transform targetAspirant) {
        /*foreach (Vector3 tile in movementScript.GetAdjacentTiles(movementScript.Tiles.)) {
            if (targetAspirant.position == gameObject.transform.position + tile) {
                return true;
            }
        }*/

        return false;
    }
}
