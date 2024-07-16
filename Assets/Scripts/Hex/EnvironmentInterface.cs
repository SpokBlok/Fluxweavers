using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluxNamespace;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using Unity.Collections;
using System.Linq.Expressions;
public class EnvironmentInterface : MonoBehaviour
{
    public delegate void DisableHexClick();
    public delegate void ToggleUI();
    public static event DisableHexClick onDisableHexClick;
    public static event ToggleUI onToggleUI;
    [SerializeField] TextMeshProUGUI currentFluxNameText;
    [SerializeField] TextMeshProUGUI tilesLeftText;
    
    [SerializeField] TilesCreationScript tcs;
    [SerializeField] Sprite waterSprite;
    [SerializeField] Sprite defaultSprite;
    private int tilesLeft;
    private Flux currentFlux;
    private List<Hex> castedHexes;

    void Start() {
        tilesLeft = 0;
        currentFlux = null;
        castedHexes = new List<Hex>();
        currentFluxNameText.text = "";
        tilesLeftText.text = "";
    }

    //Initial hex drop
    public void SetEnvironment(Hex hex, Flux flux){
        currentFlux = flux;
        castedHexes.Add(hex);
        tilesLeft = flux.tileLength - 1; // Number of tiles the user can paint over
        
        UpdateText();

        hex.hexSprite.sprite = SetSprite(hex, flux);
        hex.terrainDuration = flux.duration;
        
        foreach(Hex adjHex in GetAdjacentHex(hex)){
            adjHex.hexSprite.color = Color.yellow;
            adjHex.clickToCast = true;
        }

        if(tilesLeft > 0){
            onToggleUI?.Invoke();
        } else {
            castedHexes.Clear();
        }
    }

    // On adjacent hex click
    public void HexClicked(Hex hex){
        hex.hexSprite.sprite = SetSprite(hex, currentFlux);
        hex.terrainDuration = currentFlux.duration;
        castedHexes.Add(hex);
        onDisableHexClick?.Invoke();
        tilesLeft -= 1;
        if(tilesLeft > 0) {
            foreach(Hex adjHex in GetAdjacentHex(hex)){
                if(!castedHexes.Contains(adjHex)) {
                    adjHex.hexSprite.color = Color.yellow;
                    adjHex.clickToCast = true;
                }
            }
        } else {
            onToggleUI?.Invoke();
            castedHexes.Clear();
        }
        UpdateText();
    }

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
        currentFluxNameText.text = currentFlux.fluxName;
        tilesLeftText.text = tilesLeft.ToString();
        if(tilesLeft == 0){
            currentFluxNameText.text = "";
            tilesLeftText.text = "";
        }
    }

    //gets the adjacent hexes, REALLY INEFFICIENT
    private List<Hex> GetAdjacentHex(Hex initialHex) {
        int x = initialHex.x;
        int y = initialHex.y;
        List<Hex> adjacentHexes = new List<Hex>();
        List<(int, int)> coords = new List<(int, int)>{
            (x-1, y+1),
            (x  , y+1),
            (x-1, y  ),
            (x+1, y  ),
            (x  , y-1),
            (x+1, y-1),
        };

        foreach((int,int) pair in coords){
            try{
                Hex newHex = tcs.Tiles[pair.Item2,pair.Item1].GetComponent<Hex>();
                if(newHex != null) {
                    adjacentHexes.Add(newHex);
                    Debug.Log($"{newHex.x}, {newHex.y}"); 
                }    
            } catch {}       
        }
        return adjacentHexes;
    }
}

