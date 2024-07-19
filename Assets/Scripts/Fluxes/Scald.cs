using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Scald : Flux
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
        fluxName = "Scald";
        fluxCode = FluxNamespace.FluxNames.Scald;
        type = Type.Environment;
        duration = 3;
        manaCost = 10;
        tileLength = 4;
        effectTiming = EffectTimings.OnCast;
        description = "Create a steamfield on four adacent tiles. Units here have their healing effects reduced by 65% and are Silenced. Lasts 3 Rounds.";
    }   
}
