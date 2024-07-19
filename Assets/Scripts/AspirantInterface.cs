using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AspirantInterface : MonoBehaviour
{
    private GameObject uiObject;
    [SerializeField] public GameObject tooltip;
    public GameObject aspirantStats;

    private PhaseHandler phaseHandler;

    // SERIALIZED IS JUST FOR CHECKING
    [SerializeField] private List<string> actionsInOrder;

    // BUTTONS
    [SerializeField] private Button lastClickedAbility;
    private List<Button> actionButtons;

    private Button traverseButton;
    private Button signatureMoveButton;
    private Button infoButton;

    private List<Sprite> currentButtons; // for current list of button sprites

    // IMAGE COMPONENT
    private Image aspirantImage;

    // SPRITES
    // for Action Buttons
    // traverse
    public Sprite traverseUnclicked;
    public Sprite traverseClicked;

    // for Aspirants' other buttons
    // (for each action: unclicked, then clicked, then next action)
    // Maiko's actions
    public List<Sprite> maikoButtons;

    // Dedra's actions
    public List<Sprite> dedraButtons;

    // Citrine's actions
    public List<Sprite> citrineButtons;

    // Signature Move Inactive
    public Sprite sigInactive;

    // info
    public Sprite infoUnclicked;
    public Sprite infoClicked;

    // for Aspirant Images
    public Sprite maikoPfp;
    public Sprite dedraPfp;
    public Sprite citrinePfp;

    // for Image Borders
    public Sprite inactiveBorder;
    public Sprite maikoBorder;
    public Sprite dedraBorder;
    public Sprite citrineBorder;

    private Sprite currentActiveBorder;

    // ===== end of sprites section =====

    // TEXT COMPONENTS
    [SerializeField] public TextMeshProUGUI headerText;
    [SerializeField] public TextMeshProUGUI bodyText;
    [SerializeField] public TextMeshProUGUI footerText;

    // TEXTS
    // for ability descriptions / definitions
    // traverse
    private string traverseAbilityDef;

    // for each aspirant's other abilities
    // Maiko's abilities
    private List<string> maikoAbilityDefs;

    // Dedra's abilities
    private List<string> dedraAbilityDefs;

    // Citrine's abilities
    private List<string> citrineAbilityDefs;

    // ===== end of texts section =====

    // Start is called before the first frame update
    void Start()
    {
        uiObject = GameObject.Find("AspirantPhaseUI");
        tooltip = GameObject.Find("TooltipContainer");
        aspirantStats = GameObject.Find("AspirantStats");

        phaseHandler = GameObject.Find("PhaseHandler").GetComponent<PhaseHandler>();

        actionsInOrder = new List<string>{"BasicAttack", "Skill", "SignatureMove"};

        SetActionButtons();
        SetTextObjects();

        aspirantImage = GameObject.Find("AspirantImage").GetComponent<Image>();

        traverseAbilityDef = "Traverse is the ability to move from one tile to another. The distance traveled (in tiles) is determined by the Aspirant’s Movement stat. Traversing can only be done once per round per Aspirant.";

        maikoAbilityDefs = new List<string>
        {
            "\nDeal Physical DMG equal to 5% of Maiko's Max HP + 60% of Maiko's Combined Armor and Magic Res to a target enemy.",
            "\nDeal Magic DMG equal to 10% of Maiko's Max HP + 100% of his ATK and Slow the target for 1 Round.",
            "Passive: While Maiko is in a Aqua environment, he regenerates 3% of his missing HP\nat Round End.\nMaiko gains +1 Movement this Round. Then, Maiko increases his Armor and Magic Res. by 35% and lowers enemy ATK by 20% in a 2 tile radius. Lasts 3 Rounds."
        };

        dedraAbilityDefs = new List<string>
        {
            "\nDeal Physical DMG equal to 100% of Dedra's ATK to a target enemy.",
            "\nDedra's next 3 Basic Attacks deal 185% of her ATK instead and she gains +14% Armor Pen. This bonus increases to 225% of ATK for targets below 35% of their Max HP. Lasts 3 Rounds.",
            "\nDedra's gains + 1 Control, and her next basic attack costs -2 Mana and has +1 Range. If the target dies, this effect is refreshed."
        };

        citrineAbilityDefs = new List<string>
        {
            "\nDeal Magic DMG equal to 120% of Citrine's ATK to a target enemy.",
            "\nIncreases all allies' Armor Pen. and Magic Res. Pen. by 12 for 2 Rounds.",
            "\nShield all allies negating the next instance of damage. Increase all allies ATK by 50% for 2 Rounds."
        };
        
        aspirantStats.SetActive(false);
        tooltip.SetActive(false);
        uiObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!uiObject.activeSelf && phaseHandler.selectedPlayer != null)
        {
            uiObject.SetActive(true);
            SetupButtonsAndImages();
            aspirantStats.SetActive(false);
            tooltip.SetActive(false);
        }

        else if (uiObject.activeSelf && phaseHandler.selectedPlayer == null)
            uiObject.SetActive(false);

        // to check flux affinity and update UI accordingly
        if (phaseHandler.selectedPlayer != null)
        {
            if (phaseHandler.selectedPlayer.isMeetingFluxAffinity())
                SignatureMoveSetActive(true);
            else
                SignatureMoveSetActive(false);
        }
    }

    void SetActionButtons()
    {
        actionButtons = new List<Button>();

        foreach(string action in actionsInOrder)
        {
            Button button = GameObject.Find(action).GetComponent<Button>();

            if(!actionButtons.Contains(button))
                actionButtons.Add(button);
        }

        traverseButton = GameObject.Find("Traverse").GetComponent<Button>();
        signatureMoveButton = GameObject.Find("SignatureMove").GetComponent<Button>();
        infoButton = GameObject.Find("Info").GetComponent<Button>();
    }

    void SetTextObjects()
    {
        headerText = GameObject.Find("HeaderText").GetComponent<TextMeshProUGUI>();
        bodyText = GameObject.Find("BodyText").GetComponent<TextMeshProUGUI>();
        footerText = GameObject.Find("FooterText").GetComponent<TextMeshProUGUI>();
    }

    public void SetupButtonsAndImages()
    {
        string name = phaseHandler.selectedPlayer.name;

        if (name.Equals("Maiko"))
        {
            currentButtons = maikoButtons;
            aspirantImage.sprite = maikoPfp;
            currentActiveBorder = maikoBorder;
        }
        else if (name.Equals("Dedra"))
        {
            currentButtons = dedraButtons;
            aspirantImage.sprite = dedraPfp;
            currentActiveBorder = dedraBorder;
        }
        else if (name.Equals("Citrine"))
        {
            currentButtons = citrineButtons;
            aspirantImage.sprite = citrinePfp;
            currentActiveBorder = citrineBorder;
        }
        
        // get unclicked buttons that contain aspirant's abilities
        for(int i = 0; i < actionButtons.Count-1; i++)
            actionButtons[i].GetComponent<Image>().sprite = currentButtons[i*2];

        // (by default) set sig to inactive
        actionButtons[actionButtons.Count-1].GetComponent<Image>().sprite = sigInactive;

        // set other buttons to unclicked
        traverseButton.GetComponent<Image>().sprite = traverseUnclicked;
        infoButton.GetComponent<Image>().sprite = infoUnclicked;

        // reset last clicked button to null
        lastClickedAbility = null;
    }

    void SignatureMoveSetActive(bool isActive)
    {
        signatureMoveButton.interactable = isActive;

        Image border = aspirantImage.gameObject.transform.GetChild(0).GetComponent<Image>();

        if (isActive)
        {
            signatureMoveButton.GetComponent<Image>().sprite = currentButtons[4];

            border.sprite = currentActiveBorder;
        }
        else
        {
            signatureMoveButton.GetComponent<Image>().sprite = sigInactive;
        
            border.sprite = inactiveBorder;
        }
    }

    public void ButtonClicked(Button button)
    {
        // reset ranges
        phaseHandler.playerAspirant.ResetForRangeCalculations(phaseHandler);
        
        if (button.name.Equals("Cancel"))
        {
            // Unclick previous ability if there is one
            if (!phaseHandler.playerAspirant.selectedAbility.Equals("none"))
            {
                ToggleAbilityButton(false);

                // reset tooltip accordingly
                SetupTooltip();
            }
        }

        else if (button == infoButton)
        {
            if (tooltip.activeSelf)
            {
                // make sprite back to unclicked state
                infoButton.GetComponent<Image>().sprite = infoUnclicked;

                // close tooltip
                tooltip.SetActive(false);
            }

            else
            {
                // setup tooltip
                SetupTooltip();

                // open tooltip
                OpenTooltip();
            }
        }

        else
        {
            string currentAbility = phaseHandler.playerAspirant.selectedAbility;

            // if the current selected ability is the same with what was clicked
            if (currentAbility.Equals(button.name))
            {
                // make sprite back to unclicked state
                ToggleAbilityButton(false);
            }

            else
            {
                // Unclick previously clicked button if there is one recorded
                if (lastClickedAbility != null)
                    ToggleAbilityButton(false);

                // make new button selected
                lastClickedAbility = button;
                ToggleAbilityButton(true);

                // select ability
                phaseHandler.playerAspirant.selectedAbility = button.name;
            }

            // setup tooltip
            SetupTooltip();
        }
    }

    public void ToggleAbilityButton(bool isSelected)
    {
        if(isSelected)
        {
            if (lastClickedAbility == traverseButton)
                lastClickedAbility.GetComponent<Image>().sprite = traverseClicked;
            else
            {
                int index = actionsInOrder.IndexOf(lastClickedAbility.name);

                lastClickedAbility.GetComponent<Image>().sprite = currentButtons[index * 2 + 1];
            }
        }
        else
        {
            if (lastClickedAbility == traverseButton)
                lastClickedAbility.GetComponent<Image>().sprite = traverseUnclicked;
            else
            {
                int index = actionsInOrder.IndexOf(lastClickedAbility.name);

                lastClickedAbility.GetComponent<Image>().sprite = currentButtons[index * 2];
            }

            lastClickedAbility = null;

            // deselect ability
            phaseHandler.playerAspirant.selectedAbility = "none";
        }
    }

    void SetupTooltip()
    {
        if (!phaseHandler.playerAspirant.selectedAbility.Equals("none"))
        {
            if (phaseHandler.playerAspirant.selectedAbility.Equals("BasicAttack"))
                headerText.text = "Basic Attack";
            else if (phaseHandler.playerAspirant.selectedAbility.Equals("SignatureMove"))
                headerText.text = "Signature Move";
            else
                headerText.text = phaseHandler.playerAspirant.selectedAbility;

            int index = actionsInOrder.IndexOf(phaseHandler.playerAspirant.selectedAbility);
            string name = phaseHandler.selectedPlayer.name;

            if (lastClickedAbility == traverseButton)
            {
                bodyText.text = traverseAbilityDef;

                // show movement stat of selected player
                footerText.text = "Movement stat: " + phaseHandler.selectedPlayer.movement;
            }

            else
            {
                int range = 0;

                if (name.Equals("Maiko"))
                {
                    if (index == 2)
                        bodyText.fontSize = 14; // sig text too long, so made font size smaller

                    bodyText.text = maikoAbilityDefs[index];
                }

                else if (name.Equals("Dedra"))
                    bodyText.text = dedraAbilityDefs[index];
                
                else if (name.Equals("Citrine"))
                    bodyText.text = citrineAbilityDefs[index];

                if (index == 0)
                    range = (int) phaseHandler.selectedPlayer.basicAttackRange;
                else if (index == 1)
                    range = (int) phaseHandler.selectedPlayer.skillRange;
                else if (index == 2)
                    range = (int) phaseHandler.selectedPlayer.signatureMoveRange;

                if (range > 0)
                {
                    range += phaseHandler.playerAspirant.additionalRange;

                    footerText.text = "Ability range: " + range;
                }
                else if (range == 0)
                    footerText.text = "Ability range: Self (0)";
                else
                    footerText.text = "Ability range: Global";
            }
        }

        else
        {
            headerText.text = phaseHandler.selectedPlayer.name;
            bodyText.text = "";
            footerText.text = "";

            List<float> stats = GetAspirantStats();

            for (int i = 0; i < stats.Count; i++)
            {
                TextMeshProUGUI textField = aspirantStats.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
                textField.text = (Math.Round(stats[i],1)).ToString();
            }

            aspirantStats.SetActive(true);
        }
    }

    public void OpenTooltip()
    {
        // transition into clicked state
        infoButton.GetComponent<Image>().sprite = infoClicked;

        // open tooltip
        tooltip.SetActive(true);

        bodyText.fontSize = 18;
    }

    List<float> GetAspirantStats()
    {
        List<float> stats = new List<float>();

        stats.Add(phaseHandler.selectedPlayer.health);            // 0
        stats.Add(phaseHandler.selectedPlayer.attackStat);        // 1
        stats.Add(phaseHandler.selectedPlayer.control);           // 2
        stats.Add(phaseHandler.selectedPlayer.armor);             // 3
        stats.Add(phaseHandler.selectedPlayer.magicResistance);   // 4
        stats.Add(phaseHandler.selectedPlayer.movement);          // 5
        stats.Add(phaseHandler.selectedPlayer.armorPenetration);  // 6
        stats.Add(phaseHandler.selectedPlayer.magicPenetration);  // 7

        return stats;
    }
}
