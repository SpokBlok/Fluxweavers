using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class ScorchingWinds : Flux
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
        fluxName = "ScorchingWinds";
        fluxCode = FluxNamespace.FluxNames.ScorchingWinds;
        type = Type.Spell;
        duration = 0;
        manaCost = 18;
        damage = 50;
        tileLength = 2;
        effectTiming = EffectTimings.OnCast;
        description = $"Displace a unit two tiles in any direction and deal {damage} Magic damage to it.";
    }

    public override void SpellCast(Hex hex)
    {
        PlayerObject aspirant = hex.HexOccupant();
        if (aspirant != null)
            aspirant.IsAttacked(damage);
    }
}
