using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class WindsweptWoods : Flux
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
        fluxName = "WindsweptWoods";
        fluxCode = FluxNamespace.FluxNames.WindsweptWoods;
        type = Type.Environment;
        duration = 3;
        manaCost = 25;
        tileLength = 3;
        effectTiming = EffectTimings.RoundStart;
        description = $"Create a Forest with special propeties on {tileLength} adjacent tiles. " +
            $"Round Start: If an Aspirant is on this environment, they have +1 Movement for that Round." +
            $"Lasts {duration} Rounds.";
    }

    public override void EnvironmentEffectRoundStart(PlayerObject aspirant)
    {
        StatusEffect effect = new StatusEffect();
        HashSet<PlayerObject> targets = new HashSet<PlayerObject>();
        targets.Add(aspirant);
        effect.instantiateAddIntEffect("movement", 1, 1, targets);

        StatusEffectHandlerScript Handler = GameObject.FindGameObjectWithTag("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
        Handler.addStatusEffect(effect);

    }
}
