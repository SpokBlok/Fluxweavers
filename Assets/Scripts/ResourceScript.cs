using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{

    public int manaCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int roundStart(int roundCount)
    {
        manaCount = 10 + roundCount*10;
        if (manaCount > 100)
        {
            manaCount = 100;
        }
        return manaCount;
    }

    public bool abilityUseCheck(int currentMana, int manaCost)
    {
        return manaCost <= currentMana;
    }

    public int abilityUseManaUpdate(int currentMana, int manaCost)
    {
        return currentMana - manaCost;
    }
}
