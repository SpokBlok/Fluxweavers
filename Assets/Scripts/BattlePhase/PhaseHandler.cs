using System.Collections;
using System.Collections.Generic;
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


    //This text field is meant only to test the funcitonality of the state machine
    [SerializeField] public TextMeshProUGUI stateText;
    
    void Start()
    {
        currentRound = 0;
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
