using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private AiMovementLogic[] aiEntities;
    private Vector2Int[] aiComrades;

    [SerializeField] private AspirantMovement aspirant;
    [SerializeField] private ResourceScript rs;

    public Dictionary<AiMovementLogic, bool> turnCheck;

    void Start()
    {
        aiEntities = gameObject.GetComponentsInChildren<AiMovementLogic>();
        aiComrades = new Vector2Int[aiEntities.Count()];
        turnCheck = new Dictionary<AiMovementLogic, bool>();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.C)) {
        //     // foreach (AiMovementLogic ai in aiEntities) {
        //     //     ai.enabled = true;
        //     //     ai.Move(obstacles);
        //     //     UpdateObstacles();
        //     // }

        //     StartCoroutine(nameof(MoveAi));
        // }
            
    }

    public IEnumerator MoveAi () {
        Vector2Int target = new(aspirant.currentXIndex, aspirant.currentYIndex);
        PlayerObject aspirantStats = aspirant.gameObject.GetComponent<PlayerObject>();
        Raccoon[] AiWithEnemyInRange = new Raccoon[]{};
        // int attackCounter = 0;

        
        foreach (AiMovementLogic ai in aiEntities) {

            // Move Ai First
            ai.Move(target, aiComrades);
            yield return new WaitUntil(() => ai.enabled == false);
            // yield return StartCoroutine(ai.Move(obstacles));
            UpdateObstacles();

            // 


            // Then Attack
            Raccoon raccoonComponent = ai.gameObject.GetComponent<Raccoon>();
            HashSet<Vector2Int> neighbors = ai.GetAdjacentTiles(raccoonComponent.control);

            if (neighbors.Contains(new(target.y, target.x))) {
                AiWithEnemyInRange.Append(ai.gameObject.GetComponent<Raccoon>());

                // if (rs.enemyMana() - raccoonComponent.skillMana > raccoonComponent.basicAttackMana * aiEntities.Length)
                //     raccoonComponent.skillStatus(new HashSet<PlayerObject>(){raccoonComponent});
                
                // raccoonComponent.basicAttack(aspirantStats.armor, aspirantStats.health, aspirantStats.maxHealth);
            }

            // else {
            //     raccoonComponent.skillStatus(new HashSet<PlayerObject>(){raccoonComponent});
            // }
        }


        // Then Attack or cast abilities
        Raccoon[] raccoons = gameObject.GetComponentsInChildren<Raccoon>();
        foreach (Raccoon raccoon in raccoons) {
            if (
                AiWithEnemyInRange.Contains(raccoon) && 
                rs.enemyMana() - raccoon.skillMana > raccoon.basicAttackMana * AiWithEnemyInRange.Count()
                ) {
                
                raccoon.skillStatus(new HashSet<PlayerObject>(){raccoon});

                raccoon.basicAttack(aspirantStats.armor, aspirantStats.health, aspirantStats.maxHealth);
            }

            else {
                raccoon.skillStatus(new HashSet<PlayerObject>(){raccoon});
            }
        }
    }

    private void UpdateObstacles () {
        for (int i = 0; i < aiComrades.Count(); i++) {
            aiComrades[i] = new Vector2Int(aiEntities[i].GetXIndex(), aiEntities[i].GetYIndex());
        }
    }
}
