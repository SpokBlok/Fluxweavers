using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Wildfire : Flux
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
        fluxName = "Wildfire";
        fluxCode = FluxNamespace.FluxNames.Wildfire;
        type = Type.Environment;
        duration = 2;
        manaCost = 20;
        tileLength = 4;
        effectTiming = EffectTimings.RoundEnd;
        description = $"Spread a forest fire and Scorch {tileLength} adjacent tiles. Lasts {duration} Rounds.";
    }

    public override void EnvironmentEffectRoundEnd(PlayerObject aspirant)
    {
        float tileDamage = 90;
        aspirant.IsAttacked(tileDamage);
    }
}
