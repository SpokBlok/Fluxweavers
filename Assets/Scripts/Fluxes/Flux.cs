using UnityEngine.UIElements;

/*

    THE WAY THIS CLASS IS ABSTRACTED IS STILL SUBJDCT TO CHANGE!@iu!n!in! -ej :>

*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class Flux : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler 
{
    public enum Type {
        Spell,
        Environment
    };

    public enum EffectTimings {
        OnCast,
        AspirantPhase,
        RoundEnd
    };


    protected String fluxName;
    protected Type type;
    protected int duration;
    protected int manaCost;
    protected EffectTimings effectTiming;
    protected String description;
    private Vector3 initialPosition;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        Debug.Log(transform.position);
        initialPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData){
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
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
}
