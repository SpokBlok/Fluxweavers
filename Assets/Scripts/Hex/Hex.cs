using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using FluxNamespace;
using UnityEngine.UIElements;

public class Hex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerDownHandler
{
    public SpriteRenderer hexSprite;   
    private FluxInterface fi;
    public int terrainDuration;
    public FluxNames currentFlux;
    public bool clickToCast;
    [SerializeField] Sprite defaultSprite;
    private EnvironmentInterface ei;
    public int layer = 0; // default layer is ground (0)
    public int y;         // y-index in 2d array
    public int x;         // x-index in 2d array
    private Color currentColor;

    void Start()
    {
        PhaseRoundEnd.onRoundEnd += RoundEnd; //Subscribes each hex to the onRoundEnd event seen in PhaseRoundEnd.cs
        ei = GameObject.Find("EnvironmentInterface").GetComponent<EnvironmentInterface>();
        fi = GameObject.Find("FluxInterface").GetComponent<FluxInterface>();  
        EnvironmentInterface.onDisableHexClick += ClickToCastDisable;
        currentFlux = FluxNames.None;      
        hexSprite = gameObject.GetComponent<SpriteRenderer>();
        clickToCast = false;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        currentColor = hexSprite.color;
        hexSprite.color = new Color(0.8f,0.8f,0.8f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        hexSprite.color = currentColor;
    }

    public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag != null) {
            fi.FluxPlaced(eventData.pointerDrag, this); //sends the data to the flux interface
        }
    }
    public void OnPointerDown(PointerEventData eventData) {
        if(clickToCast){
            ei.HexClicked(this);
            currentColor = Color.white;
        }
    }

    //subtracts duration by 1 on round end
    private void RoundEnd() {
        if (terrainDuration > 0) {
            terrainDuration -= 1;
            if(terrainDuration == 0){
                hexSprite.sprite = defaultSprite;
                currentFlux = FluxNames.None;
            }
        }
    }

    private void ClickToCastDisable() {
        clickToCast = false;
        hexSprite.color = Color.white;
    }

    //Next part of the hex code is activated on round end. passes it to the environment interface
    public void TerrainEffect(PlayerObject entity){
        ei.TerrainEffect(entity, currentFlux);
    }
}
