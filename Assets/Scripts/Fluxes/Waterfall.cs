using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Waterfall : Flux
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
        fluxName = "Waterfall";
        fluxCode = FluxNamespace.FluxNames.Waterfall;
        type = Type.Environment;
        duration = 2;
        manaCost = 22;
        tileLength = 2;
        effectTiming = EffectTimings.RoundEnd;
        description = $"Create a mountain with special properties on two adjacent tiles.\r\n" +
            $"Round End: Heal units here for 8% of their Max HP.\r\n" +
            $"Round Start: If a unit is here, gain 8 extra Mana this Round.\r\n\r\n" +
            $"Lasts {duration} Rounds.";
    }

    public override void EnvironmentEffectRoundStart(PlayerObject aspirant)
    {
        aspirant.phaseHandler.rs.playerManaCount += 8;
    }

    public override void EnvironmentEffectRoundEnd(PlayerObject aspirant)
    {
        float healPercent = 0.08f;
        aspirant.AddHealth(aspirant.maxHealth * healPercent);
    }
}
