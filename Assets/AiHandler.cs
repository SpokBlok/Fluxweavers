using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private AiMovementLogic[] aiEntities;

    void Start()
    {
        aiEntities = gameObject.GetComponentsInChildren<AiMovementLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            foreach (AiMovementLogic ai in aiEntities) {
                ai.move();
            }
        }
            
    }
}
