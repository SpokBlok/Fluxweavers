using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SeismicWave : Flux
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
        fluxName = "Earth Arise";
        fluxCode = FluxNamespace.FluxNames.EarthArise;
        type = Type.Environment;
        duration = 4;
        manaCost = 25;
        tileLength = 6;
        effectTiming = EffectTimings.OnCast;
        description = $"Create a Mountain on {tileLength} adjacent tiles. Lasts {duration} Rounds. Mountains are environments that grant extra +2 Control and +1 Range to all skills to units on top of them. Traversing into and out of a Mountain costs 2 Movement.";
    }
}
