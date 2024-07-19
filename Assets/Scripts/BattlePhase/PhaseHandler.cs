using System.Collections;
using System.Collections.Generic;
using System.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FluxNamespace;

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

    public PlayerObject selectedEnemy;
    public HashSet<Vector2Int> enemiesInRange = new HashSet<Vector2Int>();

    public ResourceScript rs;

    public HashSet<FluxNames> forestMakingFluxes;
    public HashSet<FluxNames> mountainMakingFluxes;

    public HashSet<FluxNames> aquaFluxes;
    public HashSet<FluxNames> foliaFluxes;
    public HashSet<FluxNames> terraFluxes;

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

        // fluxes with special effects on movement / range
        SetupForestMakingFluxes();
        SetupMountainMakingFluxes();

        // fluxes with affinity to aspirants (for their signature moves)
        SetupAquaFluxes();
        SetupFoliaFluxes();
        SetupTerraFluxes();
        
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
        forestMakingFluxes.Add(FluxNames.MountainSpires);
        forestMakingFluxes.Add(FluxNames.WindsweptWoods);
    }

    void SetupMountainMakingFluxes()
    {
        mountainMakingFluxes = new HashSet<FluxNames>();

        mountainMakingFluxes.Add(FluxNames.Waterfall);
        mountainMakingFluxes.Add(FluxNames.MountainSpires);
        mountainMakingFluxes.Add(FluxNames.EarthArise);
        mountainMakingFluxes.Add(FluxNames.SeismicWave);
    }

    void SetupAquaFluxes()
    {
        aquaFluxes = new HashSet<FluxNames>();

        aquaFluxes.Add(FluxNames.HighTide);
        aquaFluxes.Add(FluxNames.Rivershape);
        aquaFluxes.Add(FluxNames.Scald);
        aquaFluxes.Add(FluxNames.Swamp);
        aquaFluxes.Add(FluxNames.Waterfall);
        aquaFluxes.Add(FluxNames.Rain);
    }

    void SetupFoliaFluxes()
    {
        foliaFluxes = new HashSet<FluxNames>();

        // get the forest making fluxes (all are folia)
        foreach(FluxNames flux in forestMakingFluxes)
            foliaFluxes.Add(flux);

        foliaFluxes.Add(FluxNames.Wildfire);
    }

    void SetupTerraFluxes()
    {
        terraFluxes = new HashSet<FluxNames>();

        // get the mountain making fluxes (all are terra)
        foreach(FluxNames flux in mountainMakingFluxes)
            terraFluxes.Add(flux);

        terraFluxes.Add(FluxNames.CinderCone);
        terraFluxes.Add(FluxNames.Sandstorm);
    }
}
