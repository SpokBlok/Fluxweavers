using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
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

    public PlayerObject selectedPlayer;
    public HashSet<PlayerObject> players;
    public PlayerObject enemy;
    public ResourceScript rs;

    //This text field is meant only to test the funcitonality of the state machine
    [SerializeField] public TextMeshProUGUI stateText;
    
    void Start()
    {
        players = new HashSet<PlayerObject>();
        foreach(GameObject aspirant in GameObject.FindGameObjectsWithTag("Player"))
            players.Add(aspirant.GetComponent<PlayerObject>());

        enemy = GameObject.FindGameObjectWithTag("Enemy").GetComponent<PlayerObject>();

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
