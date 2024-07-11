using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluxNamespace;
using System;
public class EnvironmentInterface : MonoBehaviour
{
    [SerializeField] Sprite waterSprite;
    [SerializeField] Sprite defaultSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnvironment(Hex hex, Flux flux){
        Debug.Log(flux.name);
        hex.hexSprite.sprite = SetSprite(hex, flux);
    }

    Sprite SetSprite(Hex hex, Flux flux){   
        if(CompareFluxName(flux, FluxNames.HighTide) || CompareFluxName(flux, FluxNames.Rivershape))
            return waterSprite;
        else 
            return hex.hexSprite.sprite;
    }

    bool CompareFluxName(Flux flux, FluxNames fluxName) {
        return flux.fluxName.Replace(" ","") == fluxName.ToString();
    }
}
