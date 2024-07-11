using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using FluxNamespace;

public class Hex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    public SpriteRenderer hexSprite;   
    private FluxInterface fi;
    public int terrainDuration;
    public TerrainNames currentTerrain;
    void Start()
    {
        currentTerrain = TerrainNames.None;
        fi = GameObject.Find("FluxInterface").GetComponent<FluxInterface>();        
        hexSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFluxPlaced() {
        //hexSprite.color = Color.green;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        hexSprite.color = Color.red;
    }

    public void OnPointerExit(PointerEventData eventData) {
        hexSprite.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData) {
        if(eventData.pointerDrag != null) {
            OnFluxPlaced();
            fi.FluxPlaced(eventData.pointerDrag, this);
        }
    }
}
