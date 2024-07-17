using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Rivershape : Flux
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
        fluxName = "Rivershape";
        fluxCode = FluxNamespace.FluxNames.Rivershape;
        type = Type.Environment;
        duration = 5;
        manaCost = 7;
        tileLength = 2;
        effectTiming = EffectTimings.RoundEnd;
        description = String.Format("Creates a small body of water on two adjacent tiles. Round End: Heal units here for 7.5% of their Max HP. Lasts 5 rounds.");
    }

    public override void EnvironmentEffect(PlayerObject aspirant) {
        float healPercent = 0.075f;
        aspirant.AddHealth(aspirant.maxHealth * healPercent);
    }

}
