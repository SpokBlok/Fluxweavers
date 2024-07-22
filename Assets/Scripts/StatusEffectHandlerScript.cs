using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatusEffectHandlerScript : MonoBehaviour
{
    [SerializeField]
    public List<StatusEffect> effectList;
    // Start is called before the first frame update
    void Start()
    {
        PhaseRoundEnd.onRoundEnd += roundUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addStatusEffect(StatusEffect effect)
    {
        effectList.Add(effect); //the effect is added to the Status Effect List
        Debug.Log("Added"); //Debug message
    }

    public void roundUpdate()
    {
        //Start from the last element so no errors even if an element is removed
        for (int i = effectList.Count - 1; i >= 0; i--)
        {
            effectList[i].duration--; // The duration variable of each effect in the list is reduced
            if (effectList[i].duration <= 0)
            {
                effectList[i].revertEffect(); // Status effect is reverted
                effectList.RemoveAt(i); // Effect is removed from the list
            }
        }
    }
}
