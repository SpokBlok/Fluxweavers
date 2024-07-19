using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using FluxNamespace;

public class Flux : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler 
{
    public FluxNames FluxNames;
    public enum Type {
        Spell,
        Environment
    };

    public enum EffectTimings {
        OnCast,
        AspirantPhase,
        RoundStart,
        RoundEnd
    };

    public String fluxName;
    public FluxNames fluxCode;
    public Type type;
    public int duration;
    public int tileLength;
    public int manaCost;
    protected EffectTimings effectTiming;
    protected String description;
    private Vector3 initialPosition;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    /*
        The code does the following:

        - Allows fluxes to be dragged.
        - If it doesnt dorp on a hex, it returns to its original position
        - on dragging, it blocks raycasts so the mouse pointer can pick up other event data
        - we also change it to be the end of the canvas so it appears in front of everything

    */
    
    private void Start() {
        //fluxCode = FluxNames.None; //TEMPORARY flux code
        //tileLength = 4; //TEMPORARY(?) TILE LENGTH
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        initialPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData){
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.75f;
    }
    public void OnDrag(PointerEventData eventData) {
        Vector3 mousePos = Input.mousePosition;
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.position = initialPosition;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    public virtual void EnvironmentEffectRoundStart(PlayerObject aspirant){
        
    }

    public virtual void EnvironmentEffectRoundEnd(PlayerObject aspirant)
    {

    }

    public virtual void SpellCast(Hex hex) {
        
    }
}
