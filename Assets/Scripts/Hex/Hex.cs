using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    public SpriteRenderer hexSprite;
    [SerializeField] FluxInterface fi;
    void Start()
    {
        hexSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFluxPlaced() {
        hexSprite.color = Color.green;
    }

    void OnMouseEnter() {
        hexSprite.color = Color.red;
    }

    void OnMouseExit() {
        hexSprite.color = Color.white;
    }

    void OnMouseUp() {
        OnFluxPlaced();
        fi.FluxPlaced();
    }
}
