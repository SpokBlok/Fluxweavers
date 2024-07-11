using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using FluxNamespace;
using UnityEngine.UIElements;

public class Hex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler, IPointerClickHandler
{
    public SpriteRenderer hexSprite;   
    private FluxInterface fi;
    public int terrainDuration;
    public TerrainNames currentTerrain;
    private bool clickToCast;
    [SerializeField] Sprite defaultSprite;
    private EnvironmentInterface ei;
    void Start()
    {
        PhaseRoundEnd.onRoundEnd += RoundEnd; //Subscribes each hex to the onRoundEnd event seen in PhaseRoundEnd.cs
        ei = GameObject.Find("EnvironmentInterface").GetComponent<EnvironmentInterface>();
        fi = GameObject.Find("FluxInterface").GetComponent<FluxInterface>();  
        EnvironmentInterface.onCastAdjacentStart += ClickToCastEnable;
        currentTerrain = TerrainNames.None;      
        hexSprite = gameObject.GetComponent<SpriteRenderer>();
        clickToCast = false;
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hexSprite.color = new Color(0.8f, 0.8f, 0.8f, 1);
    }

    public void OnPointerExit(PointerEventData eventData) {
        hexSprite.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag != null) {
            fi.FluxPlaced(eventData.pointerDrag, this); //sends the data to the flux interface
        }
    }
    public void OnPointerClick(PointerEventData eventData) {
        if(clickToCast){
            ei.HexClicked(this);
        }
    }

    //subtracts duration by 1 on round end
    private void RoundEnd() {
        if (terrainDuration > 0) {
            terrainDuration -= 1;
            if(terrainDuration == 0){
                hexSprite.sprite = defaultSprite;
                currentTerrain = TerrainNames.None;
            }
        }
    }

    private void ClickToCastEnable() {
        clickToCast = !clickToCast;
    }
}
