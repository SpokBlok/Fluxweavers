using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Gust : Flux
{
    /* 
    public enum Type {
        Spell,
        Environment
    };

    public enum EffectTimings {
        OnCast,
        AspirantPhase,
        RoundEnd
    }; 

    public String fluxName;
    public Type type;
    public int duration;
    public int manaCost;
    public EffectTimings effectTiming;
    public String description;

    */

    void Awake() {
        fluxName = "Gust";
        fluxCode = FluxNamespace.FluxNames.Gust;
        type = Type.Spell;
        duration = 0;
        manaCost = 12;
        tileLength = 2;
        effectTiming = EffectTimings.OnCast;
        description = "Displace a unit one tile towards any direction. This Flux can only be used once per unit per Phase.";
    }

    public override void SpellCast(Hex hex){
        
    }
}
