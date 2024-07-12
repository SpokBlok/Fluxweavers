using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluxNamespace;
using System;
using TMPro;
using Unity.VisualScripting;
public class EnvironmentInterface : MonoBehaviour
{
    public delegate void CastAdjacent();
    public static event CastAdjacent onCastAdjacentStart;
    [SerializeField] TextMeshProUGUI currentFluxNameText;
    [SerializeField] TextMeshProUGUI tilesLeftText;
    [SerializeField] Sprite waterSprite;
    [SerializeField] Sprite defaultSprite;
    private int tilesLeft;
    private Flux currentFlux;

    void Start() {
        tilesLeft = 0;
        currentFlux = null;
        currentFluxNameText.text = "";
        tilesLeftText.text = "";
    }

    public void SetEnvironment(Hex hex, Flux flux){
        currentFlux = flux;
        tilesLeft = flux.tileLength - 1; // Number of tiles the user can paint over

        UpdateText();

        hex.hexSprite.sprite = SetSprite(hex, flux);
        hex.terrainDuration = flux.duration;

        if(tilesLeft > 0)
            onCastAdjacentStart?.Invoke(); //Makes an event signalling to all hexes that a flux has been cast and click is enabled
    }
    private Sprite SetSprite(Hex hex, Flux flux){   
        if(CompareFluxName(flux, FluxNames.HighTide) || CompareFluxName(flux, FluxNames.Rivershape))
            return waterSprite;
        else 
            return hex.hexSprite.sprite;
    }

    public void HexClicked(Hex hex){
        hex.hexSprite.sprite = SetSprite(hex, currentFlux);
        hex.terrainDuration = currentFlux.duration;
        tilesLeft -= 1;
        if(tilesLeft == 0)
            onCastAdjacentStart?.Invoke();
        UpdateText();
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
}
