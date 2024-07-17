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
        damage = 20;
        tileLength = 1;
        effectTiming = EffectTimings.OnCast;
        description = String.Format("Deals %2.0d damage to an opponent on the tile cast.", damage);
    }
    public override void SpellCast(Hex hex) {
        PlayerObject aspirant = hex.HexOccupant();
        aspirant.IsAttacked(damage);
    }
}
