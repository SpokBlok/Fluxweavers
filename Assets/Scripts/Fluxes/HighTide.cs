using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using FluxNamespace;
public class HighTide : Flux
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
    public int tileLength;
    */
    

    void Awake() {
        fluxName = "High Tide";
        fluxCode = FluxNames.HighTide;
        type = Type.Environment;
        
        duration = 3;
        manaCost = 4;
        tileLength = 2;
        effectTiming = EffectTimings.OnCast;
        description = String.Join(
                        "Creates a small body of water on two adjacent tiles.",
                        " Round End: Heal units here for 7.5% of their Max HP.",
                        " Lasts 3 rounds."
                    );
    }

    public void EnvironmentEffect(PlayerObject hexOccupant) {
        float healPercent = 0.075f;
        hexOccupant.AddHealth(hexOccupant.maxHealth * healPercent);
    }
}
