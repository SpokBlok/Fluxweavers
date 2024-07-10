using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private AiMovementLogic[] aiEntities;
    private Vector2Int[] obstacles;

    public Dictionary<AiMovementLogic, bool> turnCheck;

    void Start()
    {
        aiEntities = gameObject.GetComponentsInChildren<AiMovementLogic>();
        obstacles = new Vector2Int[aiEntities.Count()];
        turnCheck = new Dictionary<AiMovementLogic, bool>();

        // foreach (AiMovementLogic ai in aiEntities) {
        //     ai.enabled = false;
        // }

        // foreach(AiMovementLogic ai in aiEntities) {
        //     obstacles.Add(new Vector2Int(ai.GetXIndex(), ai.GetYIndex()));
        // }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            // foreach (AiMovementLogic ai in aiEntities) {
            //     ai.enabled = true;
            //     ai.Move(obstacles);
            //     UpdateObstacles();
            // }

            StartCoroutine(nameof(MoveAi));
        }
            
    }

    private IEnumerator MoveAi () {
        foreach (AiMovementLogic ai in aiEntities) {
            ai.Move(obstacles);
            yield return new WaitForSeconds(1);
            // yield return StartCoroutine(ai.Move(obstacles));
            UpdateObstacles();
        }
    }

    private void UpdateObstacles () {
        for (int i = 0; i < obstacles.Count(); i++) {
            obstacles[i] = new Vector2Int(aiEntities[i].GetXIndex(), aiEntities[i].GetYIndex());
        }
    }
}
