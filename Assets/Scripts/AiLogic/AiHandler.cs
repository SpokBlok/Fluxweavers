using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private AiMovementLogic[] aiEntities;
    private Vector2Int[] aiComrades;

    [SerializeField] private List<Vector2Int> obstacles;

    public Dictionary<AiMovementLogic, bool> turnCheck;

    void Start()
    {
        aiEntities = gameObject.GetComponentsInChildren<AiMovementLogic>();
        aiComrades = new Vector2Int[aiEntities.Count()];
        turnCheck = new Dictionary<AiMovementLogic, bool>();

        obstacles = new List<Vector2Int>
        {
            new(9, 4),
            new(10, 4),
            new(11, 5)
        };

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
            ai.Move(obstacles, aiComrades);
            yield return new WaitUntil(() => ai.enabled == false);
            // yield return StartCoroutine(ai.Move(obstacles));
            UpdateObstacles();
        }

        Raccoon[] raccoons = gameObject.GetComponentsInChildren<Raccoon>();

        foreach (Raccoon raccoon in raccoons) {
            raccoon.skillStatus(new HashSet<PlayerObject>(){raccoon});
        }
    }

    private void UpdateObstacles () {
        for (int i = 0; i < aiComrades.Count(); i++) {
            aiComrades[i] = new Vector2Int(aiEntities[i].GetXIndex(), aiEntities[i].GetYIndex());
        }
    }
}
