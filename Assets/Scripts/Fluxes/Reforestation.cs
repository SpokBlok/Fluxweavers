using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Reforestation : Flux
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
        fluxName = "Reforestation";
        fluxCode = FluxNamespace.FluxNames.Reforestation;
        type = Type.Environment;
        duration = 3;
        manaCost = 12;  
        tileLength = 6;
        effectTiming = EffectTimings.OnCast;
        description = $"Create a Forest on {tileLength} adjacent tiles. Lasts {duration} Rounds. Traversing within Forests within does not consume Movement.";
    }
}
