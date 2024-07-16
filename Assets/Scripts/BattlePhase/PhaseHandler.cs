using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhaseHandler : MonoBehaviour
{
    PhaseBase currentState;
    public PhaseEnemyAspirant enemyAspirant = new PhaseEnemyAspirant();
    public PhasePlayerAspirant playerAspirant = new PhasePlayerAspirant();
    public PhaseEnemyFlux enemyFlux = new PhaseEnemyFlux();
    public PhasePlayerFlux playerFlux = new PhasePlayerFlux();
    public PhaseMatchStart matchStart = new PhaseMatchStart();
    public PhaseRoundEnd roundEnd = new PhaseRoundEnd();
    public int currentRound;
    public int playerManaCount;
    public int enemyManaCount;

    public PlayerObject player;
    public PlayerObject enemy;
    public ResourceScript rs;
    

    // This is for position detection. REMEMBER ITS (PlayerObject, Y, X)
    public List<(PlayerObject, int,int)> entityPositions;
    [SerializeField] public TilesCreationScript tcs;
    //This text field is meant only to test the funcitonality of the state machine
    [SerializeField] public TextMeshProUGUI stateText;
    
    void Start()
    {
        
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerObject>();
        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<PlayerObject>(); 
        rs = GameObject.FindObjectOfType<ResourceScript>();
        
        //Stores the position of the entities for hex effects
        AspirantMovement playerMovement = player.gameObject.GetComponent<AspirantMovement>();
        AiMovementLogic enemyMovement = enemy.gameObject.GetComponent<AiMovementLogic>(); 
        entityPositions = new List<(PlayerObject, int, int)>(){
            (player, playerMovement.currentYIndex, playerMovement.currentXIndex),
            (enemy, enemyMovement.currentYIndex, enemyMovement.currentXIndex)
        };

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
