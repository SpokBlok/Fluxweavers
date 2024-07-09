using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatusEffectHandlerScript : MonoBehaviour
{
    int currentRound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addStatusEffect(string statusEffectName, float statusEffect, int duration, PlayerObject[] targets)
    {
        FieldInfo stat = this.GetType().GetField(statusEffectName);
        foreach (PlayerObject target in targets) 
        {
        }
    }
}
