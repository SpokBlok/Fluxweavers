using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class StatusEffectHandlerScript : MonoBehaviour
{
    List<StatusEffect> effectList = new List<StatusEffect>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addStatusEffect(StatusEffect effect)
    {
        effectList.Add(effect); //the effect is added to the Status Effect List
    }

    public void roundUpdate()
    {
        foreach (StatusEffect effect in effectList)
        {
            effect.duration--; //The duration variable of each effect in the list is reduced
            if (effect.duration == 0)
            {
                effect.revertEffect(); //Status effect is reverted
                effectList.Remove(effect); //Effect is removed from the list
                Destroy(effect);//Effect object is destroyed
            }
        }
    }
}
