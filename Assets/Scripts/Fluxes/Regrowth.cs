using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Regrowth : Flux
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
        fluxName = "Regrowth";
        fluxCode = FluxNamespace.FluxNames.Regrowth;
        type = Type.Environment;
        duration = 2;
        manaCost = 12;
        tileLength = 4;
        effectTiming = EffectTimings.OnCast;
        description = $"Create a Forest on {tileLength} adjacent tiles. Lasts {duration} Rounds. Traversing within Forests within does not consume Movement.";
    }
}
