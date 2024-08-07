using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluxNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Unity.Collections;
using System.Linq.Expressions;
using UnityEngine.EventSystems;

public class EnvironmentInterface : MonoBehaviour
{
    public delegate void DisableHexClick();
    public delegate void ToggleUI();
    public static event DisableHexClick onDisableHexClick;
    public static event ToggleUI onToggleUI;
    [SerializeField] PhaseHandler ph;
    [SerializeField] TextMeshProUGUI tilesLeftText;
    
    [SerializeField] TilesCreationScript tcs;
    [SerializeField] Sprite waterSprite;
    [SerializeField] Sprite defaultSprite;
    private int tilesLeft;
    public Flux oldFlux;

    public Flux currentFlux;
    public FluxNames currentFluxName;
    public int currentFluxDuration;
    private List<Hex> castedHexes;

    [SerializeField] FluxInterface fi;
    public bool castDisplaceThisRound; //If gust/tornado has been used

    private Animator currentFluxAnimator;
    private Animator hexAnimator;

    private RuntimeAnimatorController previousAnimatorController;


    void Start() {
        PhaseRoundEnd.onRoundEnd += RoundEnd; //Subscribes this script to on round end call
        tilesLeft = 0;
        currentFlux = null;
        castedHexes = new List<Hex>();
        tilesLeftText.text = "";
        castDisplaceThisRound = false;
    }

    //Initial hex drop
    public void SetFlux(Hex hex, Flux flux){
        
        currentFlux = flux;
        tilesLeft = flux.tileLength - 1;
        castedHexes.Add(hex);
        
        if(flux.type == Flux.Type.Environment) {

            if (flux.fluxCode == FluxNames.Sandstorm)
            {
                foreach (Hex adjHex in GetSandstormHex(hex, true))
                {
                    currentFluxAnimator = currentFlux.GetComponent<Animator>();
                    hexAnimator = adjHex.GetComponent<Animator>();
                    hexAnimator.runtimeAnimatorController = currentFluxAnimator.runtimeAnimatorController;
                    hexAnimator.applyRootMotion = currentFluxAnimator.applyRootMotion;
                    hexAnimator.updateMode = currentFluxAnimator.updateMode;
                    hexAnimator.cullingMode = currentFluxAnimator.cullingMode;

                    adjHex.terrainDuration = flux.duration;
                    adjHex.currentFlux = flux.fluxCode;
                }
            }
            else
            {
                currentFluxAnimator = currentFlux.GetComponent<Animator>();
                hexAnimator = hex.GetComponent<Animator>();
                hexAnimator.runtimeAnimatorController = currentFluxAnimator.runtimeAnimatorController;
                hexAnimator.applyRootMotion = currentFluxAnimator.applyRootMotion;
                hexAnimator.updateMode = currentFluxAnimator.updateMode;
                hexAnimator.cullingMode = currentFluxAnimator.cullingMode;

                hex.terrainDuration = currentFluxDuration = flux.duration;
                hex.currentFlux = currentFluxName = flux.fluxCode;
            }

            if(flux.fluxCode == FluxNames.CinderCone) {
                MakeVolcano(hex);
            }

        } else {
            flux.SpellCast(hex);
            currentFluxAnimator = currentFlux.GetComponent<Animator>();
            hexAnimator = hex.GetComponent<Animator>();
            
            GameObject clone = Instantiate(hex.gameObject);
            clone.transform.position = hex.gameObject.transform.position;
            clone.transform.rotation = hex.gameObject.transform.rotation;
            clone.transform.localScale = hex.gameObject.transform.localScale;
            clone.GetComponent<SpriteRenderer>().sortingOrder -= 1;

            previousAnimatorController = hexAnimator.runtimeAnimatorController;

            hexAnimator.runtimeAnimatorController = currentFluxAnimator.runtimeAnimatorController;

            StartCoroutine(RevertAnimatorControllerAfterAnimation(hexAnimator, previousAnimatorController, clone));

            currentFluxName = flux.fluxCode;
        }
        if (tilesLeft > 0){
            UpdateText();
            if(flux.fluxCode == FluxNames.Tornado) {
                foreach(Hex adjHex in GetAdjacentHex(hex, 3, true)){
                    adjHex.hexSprite.color = Color.yellow;
                    adjHex.clickToCast = true;
                }
            }
            else if (flux.fluxCode == FluxNames.ScorchingWinds)
            {
                if (GetHexOccupant(hex))
                {
                    foreach (Hex adjHex in GetAdjacentHex(hex, 2, true))
                    {
                        adjHex.hexSprite.color = Color.yellow;
                        adjHex.clickToCast = true;
                    }
                }
                else
                {
                    tilesLeft = 0;
                    onToggleUI?.Invoke();
                    UpdateText();
                }
            }
            else if (flux.fluxCode == FluxNames.Gust) {
                foreach(Hex adjHex in GetAdjacentHex(hex, 1, true)){
                    adjHex.hexSprite.color = Color.yellow;
                    adjHex.clickToCast = true;
                }
            } else {
                foreach(Hex adjHex in GetAdjacentHex(hex, 1, false)){
                    adjHex.hexSprite.color = Color.yellow;
                    adjHex.clickToCast = true;
                }
            }
            onToggleUI?.Invoke();
        } else {
            castedHexes.Clear();
        }
    }

    // On adjacent hex click
    public void HexClicked(Hex hex){
        if(currentFlux.type == Flux.Type.Environment) {
            foreach (GameObject fluxObject in fi.fluxes)
            {
                Flux flux = fluxObject.GetComponent<Flux>();
                if (currentFluxName.ToString() == flux.name)
                {
                    oldFlux = currentFlux;
                    currentFlux = flux;
                    currentFluxAnimator = currentFlux.GetComponent<Animator>();
                    break;
                }
            }
            
            hexAnimator = hex.GetComponent<Animator>();
            hexAnimator.runtimeAnimatorController = currentFluxAnimator.runtimeAnimatorController;
            hexAnimator.applyRootMotion = currentFluxAnimator.applyRootMotion;
            hexAnimator.updateMode = currentFluxAnimator.updateMode;
            hexAnimator.cullingMode = currentFluxAnimator.cullingMode;

            hex.terrainDuration = currentFluxDuration;
            hex.currentFlux = currentFluxName;
            currentFlux = oldFlux;
        }
        else
        {
            foreach(GameObject fluxObject in fi.fluxes)
            {
                Flux flux = fluxObject.GetComponent<Flux>();
                if (currentFluxName.ToString() == flux.name)
                {
                    oldFlux = currentFlux;
                    currentFlux = flux;
                    currentFluxAnimator = currentFlux.GetComponent<Animator>();
                    break;
                }
            }
            hexAnimator = hex.GetComponent<Animator>();

            GameObject clone = Instantiate(hex.gameObject);
            clone.transform.position = hex.gameObject.transform.position;
            clone.transform.rotation = hex.gameObject.transform.rotation;
            clone.transform.localScale = hex.gameObject.transform.localScale;
            clone.GetComponent<SpriteRenderer>().sortingOrder -= 1;

            previousAnimatorController = hexAnimator.runtimeAnimatorController;

            hexAnimator.runtimeAnimatorController = currentFluxAnimator.runtimeAnimatorController;

            StartCoroutine(RevertAnimatorControllerAfterAnimation(hexAnimator, previousAnimatorController, clone));

            currentFlux = oldFlux;
        }

        if (currentFlux.fluxCode == FluxNames.Gust || currentFlux.fluxCode == FluxNames.Tornado || currentFlux.fluxCode == FluxNames.ScorchingWinds)
        {
            Displace(hex);
        }

        if(currentFlux.fluxCode == FluxNames.Blaze){
            currentFlux.SpellCast(hex);
        }

        castedHexes.Add(hex); // so that we cant cast on an already casted tile
        onDisableHexClick?.Invoke(); // sets all hexes to be unclickable (temporarily)
        tilesLeft -= 1;
        if(tilesLeft > 0) {
            //Highlights adjacent hexes
            foreach (Hex adjHex in GetAdjacentHex(hex, 1, false)){
                if(!castedHexes.Contains(adjHex)) {
                    adjHex.hexSprite.color = Color.yellow;
                    adjHex.clickToCast = true;
                }
            }
        } else {
            //if there are no tiles left, enables the ui
            onToggleUI?.Invoke();
            castedHexes.Clear();
        }
        UpdateText();
    }

    public void TerrainEffectRoundStart(PlayerObject entity, FluxNames fluxName)
    {
        switch (fluxName)
        {
            case FluxNames.WindsweptWoods:
                fi.windsweptWoods.GetComponent<WindsweptWoods>().EnvironmentEffectRoundStart(entity);
                break;
            default:
                break;
        }
    }

    //self explanatory method for affecting a player on a terrain
    public void TerrainEffectRoundEnd(PlayerObject entity, FluxNames fluxName) {
        switch(fluxName) {
            case FluxNames.HighTide:
                fi.highTide.GetComponent<HighTide>().EnvironmentEffectRoundEnd(entity);
                break;
            case FluxNames.Rivershape:
                fi.rivershape.GetComponent<Rivershape>().EnvironmentEffectRoundEnd(entity);
                break;
            case FluxNames.Swamp:
                fi.swamp.GetComponent<Swamp>().EnvironmentEffectRoundEnd(entity);
                break;
            case FluxNames.Waterfall:
                fi.waterfall.GetComponent<Waterfall>().EnvironmentEffectRoundEnd(entity);
                break;
            case FluxNames.Wildfire:
                fi.wildfire.GetComponent<Wildfire>().EnvironmentEffectRoundEnd(entity);
                break;
            case FluxNames.CinderCone:
                fi.cinderCone.GetComponent<CinderCone>().EnvironmentEffectRoundEnd(entity);
                break;
            default:
                break;
        }
    }

    public PlayerObject GetHexOccupant(Hex hex){
        Vector2Int pos = new Vector2Int(hex.y, hex.x);
        foreach(KeyValuePair<PlayerObject, Vector2Int> pair in ph.playerPositions){
            if(pair.Value == pos){
                return pair.Key;
            }
        }
        foreach(KeyValuePair<PlayerObject, Vector2Int> pair in ph.enemyPositions){
            if(pair.Value == pos){
                return pair.Key;
            }
        }
        return null;
    }

    //Helper method for sprite loading
    private Sprite SetSprite(Hex hex, Flux flux){   
        if(CompareFluxName(flux, FluxNames.HighTide) || CompareFluxName(flux, FluxNames.Rivershape))
            return waterSprite;
        else 
            return hex.hexSprite.sprite;
    }

    //helper method for name comparison
    private bool CompareFluxName(Flux flux, FluxNames fluxName) {
        return flux.fluxName.Replace(" ","") == fluxName.ToString();
    }
    
    private void UpdateText() {
        tilesLeftText.text = tilesLeft.ToString();
        if(tilesLeft == 0){
            tilesLeftText.text = "";
        }
    }

    //gets the adjacent hexes
    private List<Hex> GetAdjacentHex(Hex initialHex, int range, bool aspirantCheck) {
        int x = initialHex.x;
        int y = initialHex.y;
        List<Hex> adjacentHexes = new List<Hex>();
        List<Vector2Int> coords = new List<Vector2Int>();

        //magical formula
        for (int q = -range; q <= range; q++)
        {
            for (int r = Mathf.Max(-range, -q - range); r <= Mathf.Min(range, -q + range); r++)
            {
                if(!(q==0 && r==0))
                    coords.Add(new Vector2Int(y+r, x+q));
            }
        }
        

        if(aspirantCheck) {
            foreach(KeyValuePair<PlayerObject, Vector2Int> pair in ph.playerPositions){
                coords.Remove(pair.Value);
            }
            foreach(KeyValuePair<PlayerObject, Vector2Int> pair in ph.enemyPositions){
                coords.Remove(pair.Value);
            }
        }

        foreach(Vector2Int pair in coords){
            try{
                Hex newHex = tcs.Tiles[pair.x,pair.y].GetComponent<Hex>();
                if(newHex != null) {
                    adjacentHexes.Add(newHex);
                }    
            } catch {}       
        }
        return adjacentHexes;


    }

    private List<Hex> GetSandstormHex(Hex initialHex, bool aspirantCheck)
    {
        int x = initialHex.x;
        int y = initialHex.y;
        List<Hex> adjacentHexes = new List<Hex>();
        List<Vector2Int> coords = new List<Vector2Int>();

        for(int i = 0; i < 4; i++)
        {
            coords.Add(new Vector2Int(y, x + i));
            coords.Add(new Vector2Int(y - 1, x + 1 + i));
        }

        if (aspirantCheck)
        {
            foreach (KeyValuePair<PlayerObject, Vector2Int> pair in ph.playerPositions)
            {
                coords.Remove(pair.Value);
            }
            foreach (KeyValuePair<PlayerObject, Vector2Int> pair in ph.enemyPositions)
            {
                coords.Remove(pair.Value);
            }
        }

        foreach (Vector2Int pair in coords)
        {
            try
            {
                Hex newHex = tcs.Tiles[pair.x, pair.y].GetComponent<Hex>();
                if (newHex != null)
                {
                    adjacentHexes.Add(newHex);
                }
            }
            catch { }
        }
        return adjacentHexes;


    }

    private void MakeVolcano(Hex hex) {
        hex.transform.localScale = new Vector3(3,3,3);
        PolygonCollider2D polygonCollider2D = hex.GetComponent<PolygonCollider2D>();
        Vector2[] newPoints = polygonCollider2D.points;
        for(int i = 0; i < newPoints.Length; i++){
            newPoints[i] = Vector2.Scale(newPoints[i], new Vector2(1.0f/3.0f, 1.0f/3.0f));
        }
        polygonCollider2D.points = newPoints;

        List<Hex> volcanoHexes = new List<Hex>();
        List<Vector2Int> volcanoCoords = new List<Vector2Int>{
            new Vector2Int(hex.y, hex.x),
            new Vector2Int(hex.y-1, hex.x),
            new Vector2Int(hex.y-1, hex.x+1),
            new Vector2Int(hex.y-2, hex.x+1),
        };
        foreach(Vector2Int pair in volcanoCoords){
            try{
                Hex newHex = tcs.Tiles[pair.x,pair.y].GetComponent<Hex>();
                if(newHex != null) {
                    volcanoHexes.Add(newHex);
                }    
            } catch {}       
        }

        foreach(Hex volcano in volcanoHexes){
            if(volcano != hex) 
                volcano.hexSprite.sprite = defaultSprite;
            volcano.terrainDuration = hex.terrainDuration;
            volcano.currentFlux = FluxNames.CinderCone;
        }
    }

    public void BurnSurroundingTiles(Hex hex){
        List<Hex> adjacentHexes = GetAdjacentHex(hex, 1, false);
        List<Hex> adjacentVolcanoTiles = new List<Hex>(); 
        foreach(Hex surroundingTiles in adjacentHexes){
            if(surroundingTiles.currentFlux == FluxNames.CinderCone) {
                adjacentVolcanoTiles.Add(surroundingTiles);
            }
        }
        foreach(Hex volcanoTile in adjacentVolcanoTiles){
            if(adjacentHexes.Contains(volcanoTile))
                adjacentHexes.Remove(volcanoTile);
        }
        Hex burnTile = adjacentHexes[(int)(Random.Range(0,1.0f)*(float)adjacentHexes.Count)];
        burnTile.hexSprite.sprite = fi.wildfire.GetComponent<Image>().sprite;
        burnTile.currentFlux = FluxNames.Wildfire;
        burnTile.terrainDuration = 2;
    }

    public void VolcanoRemnant(Hex hex){
        hex.hexSprite.sprite = fi.wildfire.GetComponent<Image>().sprite;
        hex.currentFlux = FluxNames.Wildfire;
        hex.terrainDuration = 2;
    }

    private void RoundEnd(){
        castDisplaceThisRound = false;
    }

    public void Displace(Hex hex){
        Hex origHex = castedHexes[0];      
        try
        {
            AspirantMovement aspirant = GetHexOccupant(origHex).gameObject.GetComponent<AspirantMovement>();
            aspirant.currentYIndex = aspirant.originalYIndex = hex.x;
            aspirant.currentXIndex = aspirant.originalXIndex = hex.y;
            aspirant.transform.position = tcs.Tiles[hex.y, hex.x].transform.position + new Vector3(0.0f, 0.22f, 0.0f);
            ph.playerPositions[GetHexOccupant(origHex)] = new Vector2Int(hex.y, hex.x);

        }
        catch
        {
            AiMovementLogic aspirant = GetHexOccupant(origHex).gameObject.GetComponent<AiMovementLogic>();
            aspirant.currentXIndex = hex.y;
            aspirant.currentYIndex = hex.x;
            aspirant.transform.position = tcs.Tiles[hex.y, hex.x].transform.position + new Vector3(0.0f, 0.22f, 0.0f);
            ph.enemyPositions[GetHexOccupant(origHex)] = new Vector2Int(hex.y, hex.x);
        }
        castDisplaceThisRound = true;

    }

    private IEnumerator RevertAnimatorControllerAfterAnimation(Animator animator, RuntimeAnimatorController previousController, GameObject clone)
    {
        // Wait for the animation to complete
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        PhaseRoundEnd.onRoundEnd -= clone.GetComponent<Hex>().RoundEnd;
        EnvironmentInterface.onDisableHexClick -= clone.GetComponent<Hex>().ClickToCastDisable;
        Destroy(clone);
        // Revert the animator controller
        animator.runtimeAnimatorController = previousController;
    }
}

