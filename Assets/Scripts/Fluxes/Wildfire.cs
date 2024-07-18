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
    float damage;

    void Awake() {
        fluxName = "Wildfire";
        fluxCode = FluxNamespace.FluxNames.Wildfire;
        type = Type.Environment;
        duration = 2;
        manaCost = 20;
        tileLength = 4;
        damage = 60;
        effectTiming = EffectTimings.RoundEnd;
        description = $"Spread a forest fire and Scorch {tileLength} adjacent tiles. Lasts {duration} Rounds.";
    }

    public override void EnvironmentEffect(PlayerObject aspirant)
    {
        float tiledamage = 60;
        aspirant.IsAttacked(tiledamage);
    }
}
