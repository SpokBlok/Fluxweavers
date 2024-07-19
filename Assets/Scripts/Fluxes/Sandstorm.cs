using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sandstorm : Flux
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
        fluxName = "Sandstorm";
        fluxCode = FluxNamespace.FluxNames.Sandstorm;
        type = Type.Environment;
        duration = 2;
        manaCost = 25;
        effectTiming = EffectTimings.OnCast;
        description = $"Send forward a Sandstorm, spanning two rows wide and traverses four tiles forward.\r\r\n " +
            $"Units caught in the Sandstorm have their Control and Range stat of skills reduced by 1 for {duration} Round.";
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.75f;
    }
}
