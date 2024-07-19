using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class CinderCone : Flux
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
        fluxName = "Cinder Cone";
        fluxCode = FluxNamespace.FluxNames.CinderCone;
        type = Type.Environment;
        duration = 3;
        manaCost = 40;
        tileLength = 1;
        effectTiming = EffectTimings.OnCast;
        description = "Create a volcano that takes up three adjacent tiles. At the start of every Round, the Volcano erupts and Scorches 2 random tiles within its control radius for that round. When a volcano expires, it Desecrates the tiles it was placed on for 2 Rounds. Tiles that Volcanoes are placed on are considered to be Scorched.";
    }
    public override void EnvironmentEffectRoundEnd(PlayerObject aspirant)
    {
        float tileDamage = 90;
        aspirant.IsAttacked(tileDamage);
    }
}   
