using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Tornado : Flux
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
        fluxName = "Tornado";
        fluxCode = FluxNamespace.FluxNames.Tornado;
        type = Type.Spell;
        duration = 0;
        manaCost = 25;
        tileLength = 2;
        effectTiming = EffectTimings.OnCast;
        description = "Displace a unit one tile towards any direction. This Flux can only be used once per unit per Phase.";
    }

    public override void SpellCast(Hex hex){}
}
