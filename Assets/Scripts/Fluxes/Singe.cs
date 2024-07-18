using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Singe : Flux
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
        fluxName = "Singe";
        fluxCode = FluxNamespace.FluxNames.Singe;
        type = Type.Spell;
        duration = 0;
        manaCost = 10;
        damage = 35;
        tileLength = 1;
        effectTiming = EffectTimings.OnCast;
        description = $"Casts a fireball at a target tile, dealing {damage} Magic DMG to units in it.";
    }
    public override void SpellCast(Hex hex) {
        PlayerObject aspirant = hex.HexOccupant();
        if(aspirant != null)
            aspirant.IsAttacked(damage);
    }
}
