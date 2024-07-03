using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{

    int manaCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void roundStart(int roundCount)
    {
        manaCount = 10 + roundCount*10;
        if (manaCount > 100)
        {
            manaCount = 100;
        }
    }

    public bool abilityUseCheck(int currentMana, int manaCost)
    {
        if (manaCost > manaCount)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int abilityUseManaUpdate(int currentMana, int manaCost)
    {
        return currentMana - manaCost;
    }
}
