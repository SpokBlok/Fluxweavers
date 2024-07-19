using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using FluxNamespace;
using System;
using UnityEngine.Device;

public class PhaseHandler : MonoBehaviour
{
    public PhaseBase currentState;
    public PhaseEnemyAspirant enemyAspirant = new PhaseEnemyAspirant();
    public PhasePlayerAspirant playerAspirant = new PhasePlayerAspirant();
    public PhaseEnemyFlux enemyFlux = new PhaseEnemyFlux();
    public PhasePlayerFlux playerFlux = new PhasePlayerFlux();
    public PhaseMatchStart matchStart = new PhaseMatchStart();
    public PhaseRoundEnd roundEnd = new PhaseRoundEnd();
    public int currentRound;
    public int playerManaCount;
    public int enemyManaCount;

    public Dictionary<PlayerObject, Vector2Int> playerPositions;
    public HashSet<PlayerObject> players;

    public PlayerObject selectedPlayer;
    public HashSet<Vector2Int> alliesInRange = new HashSet<Vector2Int>(); 

    public Dictionary<PlayerObject, Vector2Int> enemyPositions;
    public HashSet<PlayerObject> enemies;
    public AiHandler aiHandler;

    public PlayerObject selectedEnemy;
    public HashSet<Vector2Int> enemiesInRange = new HashSet<Vector2Int>();

    public ResourceScript rs;
    

    public HashSet<FluxNames> forestMakingFluxes;

    // This is for position detection. REMEMBER ITS (PlayerObject, Y, X)
    public List<(PlayerObject, int,int)> entityPositions;
    [SerializeField] public TilesCreationScript tcs;
    
    //This text field is meant only to test the funcitonality of the state machine
    [SerializeField] public TextMeshProUGUI stateText;
    
    void Start()
    {
        playerPositions = new Dictionary<PlayerObject, Vector2Int>();

        players = new HashSet<PlayerObject>();
        foreach(GameObject aspirant in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(aspirant.GetComponent<PlayerObject>());

            int x = 0;
            int y = 0;

            try
            {
                AspirantMovement aspirantIndices = aspirant.GetComponent<AspirantMovement>();
                y = aspirantIndices.currentYIndex;
                x = aspirantIndices.currentXIndex;
            }
                catch (Exception)
            {
                y = aspirant.GetComponent<PlayerNexus>().y;
                x = aspirant.GetComponent<PlayerNexus>().x;
            }

            playerPositions[aspirant.GetComponent<PlayerObject>()] = new Vector2Int(y, x);
        }

        enemyPositions = new Dictionary<PlayerObject, Vector2Int>();

        enemies = new HashSet<PlayerObject>();
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy.GetComponent<PlayerObject>());
            int x = 0;
            int y = 0;

            try
            {
                AiMovementLogic enemyIndices = enemy.GetComponent<AiMovementLogic>();
                y = enemyIndices.GetYIndex();
                x = enemyIndices.GetXIndex();
            }
            catch (Exception)
            {
                y = enemy.GetComponent<Nexus>().y;
                x = enemy.GetComponent<Nexus>().x;
            }

            enemyPositions[enemy.GetComponent<PlayerObject>()] = new Vector2Int(y, x);
        }

        rs = GameObject.FindObjectOfType<ResourceScript>();

        SetupForestMakingFluxes();
        
        currentRound = 1;
        rs.roundStart(currentRound);
        currentState = matchStart;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState (PhaseBase state) {
        currentState = state;
        state.EnterState(this);
    }

    void SetupForestMakingFluxes()
    {
        forestMakingFluxes = new HashSet<FluxNames>();

        forestMakingFluxes.Add(FluxNames.Swamp);
        forestMakingFluxes.Add(FluxNames.Regrowth);
        forestMakingFluxes.Add(FluxNames.Reforestation);
        forestMakingFluxes.Add(FluxNames.WindsweptWoods);
    }
}
