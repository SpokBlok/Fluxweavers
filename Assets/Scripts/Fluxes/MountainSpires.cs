using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MountainSpires : Flux
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
        fluxName = "MountainSpires";
        fluxCode = FluxNamespace.FluxNames.MountainSpires;
        type = Type.Environment;
        duration = 2;
        manaCost = 30;
        tileLength = 3;
        effectTiming = EffectTimings.RoundEnd;
        description = $"Create mountain spires on three adjacent tiles.\r\n\r\n" +
            $"Mountain Spires grant +1 Control and +1 Range to all skills to units on top of them.\r\n\r\n" +
            $"Traversing into the Spires costs 2 Movement. Traversing within spires does not consume\r\n\r\n" +
            $"Movement. Traversing into and out of a Spires costs 2 Movement.\r\n\r\n" +
            $"Lasts 2 Rounds.";
    }
}
