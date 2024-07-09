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
    private Transform parentAfterDrag;
    private Vector3 initialPosition;

    private void Start() {
        parentAfterDrag = transform.parent;
        GetComponent<SpriteRenderer>().sortingOrder = 100;
        initialPosition = transform.position;
    }

    public void OnBeginDrag(PointerEventData eventData){
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        GetComponent<PolygonCollider2D>().enabled = false;
    }
    public void OnDrag(PointerEventData eventData) {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.SetParent(parentAfterDrag);
        GetComponent<PolygonCollider2D>().enabled = true;
        transform.position = initialPosition;
    }

    public void Destroy() {
        Destroy(gameObject);
    }
}
