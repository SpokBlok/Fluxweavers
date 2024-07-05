using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AiAttackLogic
{

    public bool canAttack;

    public AiAttackLogic() {
        canAttack = false;
    }

    public void attack() {
        if (canAttack) {
            Debug.Log("Attack!");
            canAttack = false;
        }
    }

    // Start is called before the first frame update
    // void Start()
    // {
    //     basisX = new Vector3(1f, 0f, 0f); // Change depending on hex size and dimensions
    //     basisY = new Vector3(0.5f, 0.9f, 0f); // Change depending on hex size and dimensions
    //     basisZ = basisY - basisX;

    //     tilesInRange = new Queue<Vector3>{};

    // }

    // Update is called once per frame
    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.V) && IsWithinRange(aspirant)) {
    //         Debug.Log("Enemy Hit!");
    //     }
    // }

    // private bool IsWithinRange (Transform target) {
    //     float dx = math.abs(target.position.x - transform.position.x) / (2 * range + 1); // So no division by zero errors
    //     float dy = math.abs(target.position.y - transform.position.y) / (2 * range + 1);
    //     float a = 0.25f * (float)math.sqrt(3.0);

    //     return (dy <= a) && (a*dx + 0.25*dy <= 0.5*a);
    // }
}
