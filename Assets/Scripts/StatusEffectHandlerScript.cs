using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class StatusEffectHandlerScript : MonoBehaviour
{
    [SerializeField]
    List<StatusEffect> effectList;
    public PlayerObject[] playerObject;
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
        Debug.Log("Added");
        foreach (StatusEffect effectss in effectList)
        {
            Debug.Log(effectss.statusEffectName);
            Debug.Log(effectss.statusEffect);
            Debug.Log(effectss.duration);
            Debug.Log(effectss.targets);

        }
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
            }
        }
    }

    private void OnMouseDown()
    {
        StatusEffect effect = new StatusEffect();
        effect.instantiateEffect("armor", 0.7f, 2, playerObject);
        addStatusEffect(effect);
    }
}
