using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : ScriptableObject
{

    public enum EffectTimings {
        OnCast,
        AspirantPhase,
        RoundEnd
    }

    public String spellName;
    public int duration;
    public EffectTimings effectTiming;

    public virtual void Effect() {

    }
}
