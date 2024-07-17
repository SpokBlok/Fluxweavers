using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Blaze : Flux
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
        fluxName = "Blaze";
        fluxCode = FluxNamespace.FluxNames.Blaze;
        type = Type.Spell;
        duration = 0;
        manaCost = 22;
        damage = 40;
        tileLength = 3;
        effectTiming = EffectTimings.OnCast;
        description = $"Rains a volley of fireballs at three adjacent tiles, dealing {damage} Magic DMG to units and reduces the duration of environments by 1 Round.";
    }

    public override void SpellCast(Hex hex) {
        hex.AugmentDuration(-1);
        PlayerObject aspirant = hex.HexOccupant();
        if(aspirant != null)
            aspirant.IsAttacked(damage);
    }
}
