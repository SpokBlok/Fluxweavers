using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hex : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private SpriteRenderer hexSprite;   
    private FluxInterface fi;
    void Start()
    {
        fi = GameObject.Find("FluxInterface").GetComponent<FluxInterface>();        
        hexSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFluxPlaced() {
        hexSprite.color = Color.green;
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
            fi.FluxPlaced(eventData.pointerDrag);
        }
    }
}
