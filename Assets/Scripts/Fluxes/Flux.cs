using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Flux", menuName = "Flux")]
public class Flux
{

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
    public virtual void Effect() {

    }
}
