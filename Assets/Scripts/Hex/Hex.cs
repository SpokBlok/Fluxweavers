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
    public enum HexEffects{
        Scorched,
        Desecrated
    }
    public SpriteRenderer hexSprite;   
    private FluxInterface fi;
    public int terrainDuration;
    public FluxNames currentFlux;
    public int effectDuration;
    public HexEffects currentEffect;
    public bool clickToCast;
    [SerializeField] Sprite defaultSprite;
    private EnvironmentInterface ei;
    public int layer = 0; // default layer is ground (0)
    public int y;         // y-index in 2d array
    public int x;         // x-index in 2d array
    private Color currentColor;
    PolygonCollider2D polygonCollider2D;
    Vector2[] polygonPoints;

    private Animator defaultAnimator;
    private RuntimeAnimatorController previousAnimatorController;
    void Start()
    {
        defaultAnimator = GetComponent<Animator>();
        previousAnimatorController = defaultAnimator.runtimeAnimatorController;
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        polygonPoints = polygonCollider2D.points;
        PhaseRoundEnd.onRoundEnd += RoundEnd; //Subscribes each hex to the onRoundEnd event seen in PhaseRoundEnd.cs
        ei = GameObject.Find("EnvironmentInterface").GetComponent<EnvironmentInterface>();
        fi = GameObject.Find("FluxInterface").GetComponent<FluxInterface>();  
        EnvironmentInterface.onDisableHexClick += ClickToCastDisable;
        //currentFlux = FluxNames.None;      
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
    public void RoundEnd() {
        AugmentDuration(-1);
        if(currentFlux == FluxNames.CinderCone)
            ei.BurnSurroundingTiles(this);
    }

    public void AugmentDuration(int length){
        if (terrainDuration > 0) {
            terrainDuration += length;
            if(terrainDuration <= 0){
                defaultAnimator.runtimeAnimatorController = previousAnimatorController;
                if(currentFlux == FluxNames.CinderCone) {
                    ei.VolcanoRemnant(this);
                }
                else {
                    currentFlux = FluxNames.None;
                }
                polygonCollider2D.points = polygonPoints;
                transform.localScale = new Vector3(1,1,1);


            }
        }
    }
    public void ClickToCastDisable() {
        clickToCast = false;
        hexSprite.color = Color.white;
    }

    public void TerrainEffectRoundStart(PlayerObject entity)
    {
        ei.TerrainEffectRoundStart(entity, currentFlux);
    }

    //Next part of the hex code is activated on round end. passes it to the environment interface
    public void TerrainEffectRoundEnd(PlayerObject entity){
        ei.TerrainEffectRoundEnd(entity, currentFlux);
    }

    public PlayerObject HexOccupant(){
        return ei.GetHexOccupant(this);
    }
}