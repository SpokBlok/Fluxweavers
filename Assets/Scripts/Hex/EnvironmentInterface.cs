using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluxNamespace;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Unity.Collections;
using System.Linq.Expressions;
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
    private Sprite currentFluxSprite;
    private List<Hex> castedHexes;

    [SerializeField] FluxInterface fi;
    public bool castDisplaceThisRound; //If gust/tornado has been used

    private Animator currentFluxAnimator;
    private Animator hexAnimator;


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

            currentFluxSprite = currentFlux.gameObject.GetComponent<Image>().sprite;
            if (flux.fluxCode == FluxNames.Sandstorm)
            {
                foreach (Hex adjHex in GetSandstormHex(hex, true))
                {
                    adjHex.hexSprite.sprite = currentFluxSprite;

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
                hex.hexSprite.sprite = currentFluxSprite;

                currentFluxAnimator = currentFlux.GetComponent<Animator>();
                hexAnimator = hex.GetComponent<Animator>();
                hexAnimator.runtimeAnimatorController = currentFluxAnimator.runtimeAnimatorController;
                hexAnimator.applyRootMotion = currentFluxAnimator.applyRootMotion;
                hexAnimator.updateMode = currentFluxAnimator.updateMode;
                hexAnimator.cullingMode = currentFluxAnimator.cullingMode;

                hex.terrainDuration = flux.duration;
                hex.currentFlux = flux.fluxCode;
            }
        } else {
            flux.SpellCast(hex);
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
        Debug.Log("Wtf1");
        if (currentFlux.type == Flux.Type.Environment) {
            hex.hexSprite.sprite = currentFluxSprite;

            foreach (GameObject fluxObject in fi.fluxes)
            {
                Flux flux = fluxObject.GetComponent<Flux>();
                Debug.Log(flux.name); 
                if (currentFlux.fluxCode.ToString() == flux.name)
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

            hex.terrainDuration = currentFlux.duration;
            hex.currentFlux = currentFlux.fluxCode;
            currentFlux = oldFlux;
        } 

        if(currentFlux.fluxCode == FluxNames.Gust || currentFlux.fluxCode == FluxNames.Tornado || currentFlux.fluxCode == FluxNames.ScorchingWinds)
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
            Debug.Log("Wtf2");
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
}

