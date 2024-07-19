using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AiHandler : MonoBehaviour
{
    // Start is called before the first frame update

    private AiMovementLogic[] aiEntities;
    private Vector2Int[] aiComrades;

    private GameObject[] aspirants;
    [SerializeField] private ResourceScript rs;

    public Dictionary<AiMovementLogic, bool> turnCheck;

    void Start()
    {
        aiEntities = gameObject.GetComponentsInChildren<AiMovementLogic>();
        aiComrades = new Vector2Int[aiEntities.Count()];
        // aspirants = GameObject.FindGameObjectsWithTag("Player");
        // Debug.Log(aspirants.Length);
        
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

        //     //StartCoroutine(nameof(MoveAi)); 
        //     aiEntities[2].Move(new(16,4), new Vector2Int[]{new(4,13), new(5,13)});
        // }
            
    }

    public IEnumerator MoveAi (HashSet<PlayerObject> aspirants) {
        // AspirantMovement aspirant = aspirants[0].GetComponent<AspirantMovement>();
        
        AspirantMovement target = null;
        PlayerObject aspirantStats;
        HashSet<Raccoon> AiWithEnemyInRange = new();
        // int attackCounter = 0;

        
        foreach (AiMovementLogic ai in aiEntities) {
            
            UpdateObstacles();
            target = GetClosestAspirant(ai, aspirants).gameObject.GetComponent<AspirantMovement>();
            Vector2Int targetPosition = new(target.currentXIndex, target.currentYIndex);

            // Move Ai First
            ai.Move(targetPosition, aiComrades);
            yield return new WaitUntil(() => ai.enabled == false);
            // yield return StartCoroutine(ai.Move(obstacles));

            // 

            Raccoon raccoonComponent = ai.gameObject.GetComponent<Raccoon>();
            HashSet<Vector2Int> neighbors = ai.GetAdjacentTiles((int) raccoonComponent.basicAttackRange);
     
            if (neighbors.Contains(new Vector2Int(targetPosition.y, targetPosition.x))) {
                AiWithEnemyInRange.Add(raccoonComponent);
            }
        }

        // Then Attack or cast abilities
        HashSet<PlayerObject> raccoons = new(gameObject.GetComponentsInChildren<Raccoon>());

        // POINT FOR UPDATE: Could be better, use a dictionary
        int manaPerRaccon = (int) Mathf.Ceil((float) rs.enemyMana() / aiEntities.Length); 

        float damageDealt = 0;
        // Debug.Log(raccoons.Length);

        if (AiWithEnemyInRange.Count == 0) {
            foreach (Raccoon raccoon in raccoons) {
                AiMovementLogic raccoonMovement = raccoon.GetComponent<AiMovementLogic>();
                int manaAllocated = manaPerRaccon; // Important
                
                // Keep basic attacking as long as mana allotted still allows for it
                while (manaAllocated >= raccoon.skillMana) {
                    raccoon.skillStatus(raccoons);
                    manaAllocated -= raccoon.skillMana;
                }
            }   
        }
        else { // Meaning there's something that can be attacked
            manaPerRaccon = rs.enemyMana() / AiWithEnemyInRange.Count;
        }

        foreach (Raccoon raccoon in AiWithEnemyInRange) {
            AiMovementLogic raccoonMovement = raccoon.GetComponent<AiMovementLogic>();
            int manaAllocated = manaPerRaccon; // Important
            target = GetClosestAspirant(raccoonMovement, aspirants).gameObject.GetComponent<AspirantMovement>();
            aspirantStats = target.gameObject.GetComponent<PlayerObject>();
            // Debug.Log(aspirantStats.gameObject.name);
            
            // Debug.Log(manaPerRaccon);
            // Better to attack first instead of buff when able to attack
            if (manaAllocated - raccoon.skillMana >= raccoon.basicAttackMana) {
                raccoon.skillStatus(raccoons);
                manaAllocated -= raccoon.skillMana;
            }
            
            // Keep basic attacking as long as mana allotted still allows for it
            while (manaAllocated >= raccoon.basicAttackMana) {
                damageDealt = raccoon.basicAttack(aspirantStats.armor, aspirantStats.health, aspirantStats.maxHealth);
                aspirantStats.IsAttacked(damageDealt);
                manaAllocated -= raccoon.basicAttackMana;
            }
        }
    }

    // POINT FOR UPDATE: Add in what happens when aspirantPositions == 0 (i.e when only the nexus is present)
    private PlayerObject GetClosestAspirant(AiMovementLogic enemyPosition, HashSet<PlayerObject> aspirantPositions) {
        int currentMinimum = 10_000; // Some big number
        Vector2Int enemy = new(enemyPosition.GetXIndex(), enemyPosition.GetYIndex());
        PlayerObject closestAspirant = null;

        foreach(PlayerObject aspirant in aspirantPositions) {

            AspirantMovement movementScript = aspirant.GetComponent<AspirantMovement>();
            Vector2Int aspirantPosition = new(movementScript.currentXIndex, movementScript.currentYIndex);

            
            Vector2Int[] paths = enemyPosition.CreatePathToTarget(aspirantPosition, aiComrades);

            if (paths.Length < currentMinimum) {
                currentMinimum = paths.Length;
                closestAspirant = aspirant;
            }
        }

        // If null?
        return closestAspirant;
    }

    private void UpdateObstacles () {
        // Debug.Log(aiComrades.Count());
        for (int i = 0; i < aiComrades.Count(); i++) {
            aiComrades[i] = new Vector2Int(aiEntities[i].GetYIndex(), aiEntities[i].GetXIndex());
            // Debug.Log("Updating: " + aiEntities[i].GetYIndex() + " " + aiEntities[i].GetXIndex());
        }
    }
}
