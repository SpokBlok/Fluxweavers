using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[System.Serializable]
public class StatusEffect
{
    public string statusEffectName;
    public float statusEffect;
    public int duration;
    public PlayerObject[] targets;
    public FieldInfo statField;
    public float newStat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateEffect(string statusEffectName, float statusEffect, int duration, PlayerObject[] targets)
    {
        this.statusEffectName = statusEffectName;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.targets = targets;

        foreach (PlayerObject target in targets)
        {
            statField = target.GetType().GetField(statusEffectName);
            float oldStat = (float)statField.GetValue(target);
            newStat = oldStat * statusEffect;
            statField.SetValue(target, newStat);
        }

    }

    public void revertEffect()
    {
        foreach (PlayerObject target in targets)
        {
            statField = target.GetType().GetField(statusEffectName);
            float oldStat = (float)statField.GetValue(target);
            newStat = oldStat / statusEffect;
            statField.SetValue(target, newStat);
        }
    }
}
