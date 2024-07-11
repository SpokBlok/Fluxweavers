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
    public bool isAdditive;
    public PlayerObject[] targets;
    FieldInfo statField;
    float newStat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateAddEffect(string statusEffectName, float statusEffect, int duration, PlayerObject[] targets)
    {
        this.statusEffectName = statusEffectName;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.targets = targets;
        isAdditive = true;

        foreach (PlayerObject target in targets)
        {
            statField = target.GetType().GetField(statusEffectName);
            float oldStat = (float)statField.GetValue(target);
            newStat = oldStat - statusEffect;
            statField.SetValue(target, newStat);
        }

    }

    public void instantiateMultiEffect(string statusEffectName, float statusEffect, int duration, PlayerObject[] targets)
    {
        this.statusEffectName = statusEffectName;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.targets = targets;
        isAdditive = false;

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
        if (isAdditive)
        {
            foreach (PlayerObject target in targets)
            {
                statField = target.GetType().GetField(statusEffectName);
                float oldStat = (float)statField.GetValue(target);
                newStat = oldStat / statusEffect;
                statField.SetValue(target, newStat);
            }
        }
        else
        {
            foreach (PlayerObject target in targets)
            {
                statField = target.GetType().GetField(statusEffectName);
                float oldStat = (float)statField.GetValue(target);
                newStat = oldStat + statusEffect;
                statField.SetValue(target, newStat);
            }
        }
    }
}
