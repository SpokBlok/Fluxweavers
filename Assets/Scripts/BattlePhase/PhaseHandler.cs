using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public Dictionary<PlayerObject, Vector2Int> enemyPositions;
    public HashSet<PlayerObject> enemies;

    public PlayerObject selectedEnemy;
    public HashSet<Vector2Int> enemiesInRange = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> alliesInRange = new HashSet<Vector2Int>(); 

    public ResourceScript rs;

    //This text field is meant only to test the funcitonality of the state machine
    [SerializeField] public TextMeshProUGUI stateText;
    
    void Start()
    {
        playerPositions = new Dictionary<PlayerObject, Vector2Int>();

        players = new HashSet<PlayerObject>();
        foreach(GameObject aspirant in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(aspirant.GetComponent<PlayerObject>());

            AspirantMovement aspirantIndices = aspirant.GetComponent<AspirantMovement>();
            int y = aspirantIndices.currentYIndex;
            int x = aspirantIndices.currentXIndex;

            playerPositions[aspirant.GetComponent<PlayerObject>()] = new Vector2Int(y, x);
        }

        enemyPositions = new Dictionary<PlayerObject, Vector2Int>();

        enemies = new HashSet<PlayerObject>();
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(enemy.GetComponent<PlayerObject>());

            AiMovementLogic enemyIndices = enemy.GetComponent<AiMovementLogic>();
            int y = enemyIndices.GetYIndex();
            int x = enemyIndices.GetXIndex();

            enemyPositions[enemy.GetComponent<PlayerObject>()] = new Vector2Int(y, x);
        }

        rs = GameObject.FindObjectOfType<ResourceScript>();
        
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
}
