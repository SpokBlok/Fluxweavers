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
    public bool isInt;
    public HashSet<PlayerObject> targets;
    FieldInfo statField;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void instantiateAddIntEffect(string statusEffectName, float statusEffect, int duration, HashSet<PlayerObject> targets)
    {
        this.statusEffectName = statusEffectName;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.targets = targets;
        isAdditive = true;
        isInt = true;

        foreach (PlayerObject target in targets)
        {
            statField = target.GetType().GetField(statusEffectName);
            Debug.Log(statField.GetValue(target));
            float oldStat = (float)(int)statField.GetValue(target);
            Debug.Log("lmao");
            float newStat = oldStat + statusEffect;
            statField.SetValue(target, (int) newStat);
        }

    }

    public void instantiateAddFloatEffect(string statusEffectName, float statusEffect, int duration, HashSet<PlayerObject> targets)
    {
        this.statusEffectName = statusEffectName;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.targets = targets;
        isAdditive = true;
        isInt = false;

        foreach (PlayerObject target in targets)
        {
            statField = target.GetType().GetField(statusEffectName);
            Debug.Log(statField.GetValue(target));
            float oldStat = (float) statField.GetValue(target);
            Debug.Log("lmao");
            float newStat = oldStat + statusEffect;
            statField.SetValue(target, newStat);
        }

    }

    public void instantiateMultiIntEffect(string statusEffectName, float statusEffect, int duration, HashSet<PlayerObject> targets)
    {
        this.statusEffectName = statusEffectName;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.targets = targets;
        isAdditive = false;
        isInt = true;

        foreach (PlayerObject target in targets)
        {
            statField = target.GetType().GetField(statusEffectName);
            float oldStat = (float)(int)statField.GetValue(target);
            float newStat = oldStat * statusEffect;
            statField.SetValue(target, (int)newStat);
        }

    }

    public void instantiateMultiFloatEffect(string statusEffectName, float statusEffect, int duration, HashSet<PlayerObject> targets)
    {
        this.statusEffectName = statusEffectName;
        this.statusEffect = statusEffect;
        this.duration = duration;
        this.targets = targets;
        isAdditive = false;
        isInt = false;

        foreach (PlayerObject target in targets)
        {
            statField = target.GetType().GetField(statusEffectName);
            float oldStat = (float)statField.GetValue(target);
            float newStat = oldStat * statusEffect;
            statField.SetValue(target, newStat);
        }

    }

    public void revertEffect()
    {
        if (isAdditive)
        {
            if (isInt)
            {
                foreach (PlayerObject target in targets)
                {
                    statField = target.GetType().GetField(statusEffectName);
                    float oldStat = (float)(int)statField.GetValue(target);
                    float newStat = oldStat - statusEffect;
                    statField.SetValue(target, (int) newStat);
                }
            }
            else
            {
                foreach (PlayerObject target in targets)
                {
                    statField = target.GetType().GetField(statusEffectName);
                    float oldStat = (float)statField.GetValue(target);
                    float newStat = oldStat - statusEffect;
                    statField.SetValue(target, newStat);
                }
            }
        }
        else
        {
            if (isInt)
            {
                foreach (PlayerObject target in targets)
                {
                    statField = target.GetType().GetField(statusEffectName);
                    float oldStat = (float)(int)statField.GetValue(target);
                    float newStat = oldStat / statusEffect;
                    statField.SetValue(target, (int) newStat);
                }
            }
            else
            {
                foreach (PlayerObject target in targets)
                {
                    statField = target.GetType().GetField(statusEffectName);
                    float oldStat = (float)statField.GetValue(target);
                    float newStat = oldStat / statusEffect;
                    statField.SetValue(target, newStat);
                }
            }
        }
    }
}