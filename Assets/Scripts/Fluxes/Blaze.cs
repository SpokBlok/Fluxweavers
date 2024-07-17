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
        description = String.Format("Deals %2.0d damage to an opponent on the tile cast.", damage);
    }

    public override void SpellCast(Hex hex) {
        hex.AugmentDuration(-1);
        PlayerObject aspirant = hex.HexOccupant();
        aspirant.IsAttacked(damage);
    }
}
