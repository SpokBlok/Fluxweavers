using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Swamp : Flux
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
        fluxName = "Swamp";
        fluxCode = FluxNamespace.FluxNames.Swamp;
        type = Type.Environment;
        duration = 3;
        manaCost = 18;
        tileLength = 4;
        effectTiming = EffectTimings.RoundEnd;
        description = $"Create a forest with special properties on four adjacent tiles.\r\n" +
            $"Round End: Heal units here for 5% of their Max HP.\r\n" +
            $"Round Start: If a unit is here, gain 5 extra Mana this Round.\r\n\r\n" +
            $"Lasts {duration} Rounds.";
    }

    public override void EnvironmentEffectRoundStart(PlayerObject aspirant)
    {
        aspirant.phaseHandler.rs.playerManaCount += 5;
    }

    public override void EnvironmentEffectRoundEnd(PlayerObject aspirant)
    {
        float healPercent = 0.05f;
        aspirant.AddHealth(aspirant.maxHealth * healPercent);
    }
}
