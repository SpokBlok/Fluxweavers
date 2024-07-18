using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AspirantInterface : MonoBehaviour
{
    private GameObject uiObject;
    private PhaseHandler phaseHandler;

    // SERIALIZED IS JUST FOR CHECKING
    [SerializeField] private PlayerObject currentPlayer;

    [SerializeField] private List<string> actionsInOrder = new List<string>
    {
        "BasicAttack", "Skill", "SignatureMove"
    };

    // BUTTONS
    [SerializeField] private Button lastClickedButton;
    [SerializeField] private List<Button> actionButtons;
    private List<Sprite> currentButtons; // for current list of button sprites

    // IMAGE
    [SerializeField] private Image aspirantImage;

    // SPRITES
    // for Action Buttons
    // traverse
    public Sprite traverseUnclicked;
    public Sprite traverseClicked;

    // Maiko's actions
    public List<Sprite> maikoButtons;

    // Dedra's actions
    public List<Sprite> dedraButtons;

    // Citrine's actions
    public List<Sprite> citrineButtons;

    // Signature Move Inactive
    public Sprite sigInactive;

    // for Aspirant Images
    public Sprite maikoPfp;
    public Sprite dedraPfp;
    public Sprite citrinePfp;

    // ==== end of sprites section ====


    // Start is called before the first frame update
    void Start()
    {
        uiObject = GameObject.Find("AspirantPhaseUI");
        phaseHandler = GameObject.Find("PhaseHandler").GetComponent<PhaseHandler>();

        SetActionButtons();

        aspirantImage = GameObject.Find("AspirantImage").GetComponent<Image>();

        uiObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiObject.activeSelf && phaseHandler.selectedPlayer != null)
        {
            uiObject.SetActive(true);
            SetupButtonsAndImages();
            currentPlayer = phaseHandler.selectedPlayer;
        }

        else if (uiObject.activeSelf && phaseHandler.selectedPlayer == null)
            uiObject.SetActive(false);
    }

    void SetActionButtons()
    {
        foreach(string action in actionsInOrder)
        {
            Button button = GameObject.Find(action).GetComponent<Button>();

            if(!actionButtons.Contains(button))
                actionButtons.Add(button);
        }
    }

    public void SetupButtonsAndImages()
    {
        string name = phaseHandler.selectedPlayer.name;

        if (name.Equals("Maiko"))
        {
            currentButtons = maikoButtons;
            aspirantImage.sprite = maikoPfp;
        }
        else if (name.Equals("Dedra"))
        {
            currentButtons = dedraButtons;
            aspirantImage.sprite = dedraPfp;
        }
        else if (name.Equals("Citrine"))
        {
            currentButtons = citrineButtons;
            aspirantImage.sprite = citrinePfp;
        }
        
        for(int i = 0; i < actionButtons.Count-1; i++)
            actionButtons[i].GetComponent<Image>().sprite = currentButtons[i*2];

        // set sig to inactive
        actionButtons[actionButtons.Count-1].GetComponent<Image>().sprite = sigInactive;
    }

    public void ButtonClicked(Button button)
    {
        // reset ranges
        phaseHandler.playerAspirant.ResetForRangeCalculations(phaseHandler);
        
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
                // make sprite back to unclicked state
                ToggleAbilityButton(false);
            }

            else
            {
                // Unclick previous ability if there is one
                if (currentAbility != "none")
                    ToggleAbilityButton(false);

                // make new button selected
                lastClickedButton = button;
                ToggleAbilityButton(true);

                // select ability
                phaseHandler.playerAspirant.selectedAbility = button.name;
            }
        }
    }

    public void ToggleAbilityButton(bool isSelected)
    {;

        if(isSelected)
        {
            if (lastClickedButton.name.Equals("Traverse"))
                lastClickedButton.GetComponent<Image>().sprite = traverseClicked;
            else
            {
                int index = actionsInOrder.IndexOf(lastClickedButton.name);

                lastClickedButton.GetComponent<Image>().sprite = currentButtons[index * 2 + 1];
            }
        }
        else
        {
            if (lastClickedButton.name.Equals("Traverse"))
                lastClickedButton.GetComponent<Image>().sprite = traverseUnclicked;
            else
            {
                int index = actionsInOrder.IndexOf(lastClickedButton.name);

                lastClickedButton.GetComponent<Image>().sprite = currentButtons[index * 2];
            }

            lastClickedButton = null;

            // deselect ability
            phaseHandler.playerAspirant.selectedAbility = "none";
        }
    }
}
