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

    public bool abilityUse(int manaCost)
    {
        if (manaCost > manaCount)
        {
            return false;
        }
        else
        {
            manaCount -= manaCost;
            return true;
        }
    }
}
