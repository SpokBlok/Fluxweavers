using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AspirantInterface : MonoBehaviour
{
    private GameObject uiObject;
    private PhaseHandler phaseHandler;

    // Start is called before the first frame update
    void Start()
    {
        uiObject = GameObject.Find("AspirantPhaseUI");
        phaseHandler = GameObject.Find("PhaseHandler").GetComponent<PhaseHandler>();

        uiObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiObject.activeSelf && phaseHandler.selectedPlayer != null)
            uiObject.SetActive(true);

        else if (uiObject.activeSelf && phaseHandler.selectedPlayer == null)
            uiObject.SetActive(false);
    }

    public void ButtonClicked(Button button)
    {
        if (button.name.Equals("Cancel"))
        {
            // do stuff for cancel button
        }

        else if (button.name.Equals("Info"))
        {
            // do stuff for info button
        }

        else
        {
            string currentAbility = phaseHandler.playerAspirant.selectedAbility;

            // if the current ability is the same with what was clicked
            if (currentAbility.Equals(button.name))
            {
                // deselect ability
                phaseHandler.playerAspirant.selectedAbility = "none";

                // make sprite back to unclicked state
            }

            else
            {
                // select ability
                phaseHandler.playerAspirant.selectedAbility = button.name;

                // make sprite to clicked state
            }
        }
    }
}
