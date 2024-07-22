using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using FluxNamespace;

public class AspirantInterface : MonoBehaviour
{
    private GameObject uiObject;
    [SerializeField] public GameObject tooltip;
    [SerializeField] public RectTransform divider;
    [SerializeField] public GameObject aspirantStats;

    private PhaseHandler phaseHandler;
    private StatusEffectHandlerScript effectHandler;
    private EventSystem eventSystem;

    // SERIALIZED IS JUST FOR CHECKING
    private List<string> actionsInOrder;
    private List<string> statsInOrder;
    private List<string> statAbbreviations;

    // BUTTONS
    [SerializeField] private Button lastClickedAbility;
    private List<Button> actionButtons;

    private Button traverseButton;
    private Button signatureMoveButton;
    private Button infoButton;

    // LOCAL SPRITE STORAGE
    private List<Sprite> currentButtons; // for current list of button sprites
    private Sprite currentActiveBorder; // border sprite to be used
                                        // if the current selected player's flux affinity is met

    // HEALTH SLIDER
    // (mana bar is updated in RS)
    private Slider healthBar;

    // IMAGE COMPONENTS
    private Image aspirantImage;
    private Image nameTag;

    // SPRITES (only stuff needed to be referenced; others are taken care of below)
    // for Action Buttons
    // traverse
    public Sprite traverseUnclicked;
    public Sprite traverseClicked;

    // for Aspirants' other buttons
    // (for each action: hover, then unclicked, then clicked, then next action)
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

    // for Aspirant Name Tags
    public Sprite maikoNameTag;
    public Sprite dedraNameTag;
    public Sprite citrineNameTag;

    // for Image Borders
    public Sprite inactiveBorder;
    public Sprite maikoBorder;
    public Sprite dedraBorder;
    public Sprite citrineBorder;

    // for Stat Icons
    // (for each stat, base, then stat up, then stat down, then next stat)
    public List<Sprite> statIcons;

    // for Windswept Woods
    public Sprite windsweptWoods;

    // ===== end of sprites section =====

    // TEXT COMPONENTS
    private TextMeshProUGUI healthText;
    [SerializeField] public TextMeshProUGUI headerText;
    [SerializeField] public TextMeshProUGUI subText;
    [SerializeField] public TextMeshProUGUI subText2;
    [SerializeField] public TextMeshProUGUI bodyText;
    private List<TextMeshProUGUI> effectTexts;

    // TEXTS (strings)
    // for ability descriptions / definitions
    // traverse
    private string traverseAbilityDef;

    // for each aspirant's other abilities
    // (for each ability: name, then description, then next ability)
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
        divider = GameObject.Find("Divider").GetComponent<RectTransform>();
        aspirantStats = GameObject.Find("AspirantStats");

        phaseHandler = GameObject.Find("PhaseHandler").GetComponent<PhaseHandler>();
        effectHandler = GameObject.Find("StatusEffectHandler").GetComponent<StatusEffectHandlerScript>();
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        actionsInOrder = new List<string>{"BasicAttack", "Skill", "SignatureMove"};
        statsInOrder = new List<string>{"attackStat", "armor", "magicResistance", "armorPenetration", "magicPenetration", "movement"};
        statAbbreviations = new List<string>{"ATK", "ARMOR", "MAGIC RES.", "ARMOR PEN.", "MAGIC PEN.", "MOVEMENT"};

        SetActionButtons();
        SetTextObjects();

        healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();

        aspirantImage = GameObject.Find("AspirantImage").GetComponent<Image>();
        nameTag = GameObject.Find("NameTag").GetComponent<Image>();

        traverseAbilityDef = "• Traverse is the ability to move from one tile to another.\n• The distance traveled (in tiles) is determined by the Aspirant’s Movement stat.\n• Traversing can only be done once per round per Aspirant.";

        maikoAbilityDefs = new List<string>
        {
            "Riptide",
            "Deal Physical DMG equal to 5% of Maiko's Max HP + 60% of Maiko's Combined Armor and Magic Res to a target enemy.",
            "Blessing of Thalasenatha",
            "Deal Magic DMG equal to 10% of Maiko's Max HP + 100% of his ATK and Slow the target for 1 Round.",
            "Ocean's Pride",
            "• Passive: While Maiko is in a Aqua environment, he regenerates 3% of his missing HP at Round End.\n• Maiko gains +1 Movement this Round. Then, Maiko increases his Armor and Magic Res. by 35% and lowers enemy ATK by 20% in a 2 tile radius.\n• Lasts 3 Rounds."
        };

        dedraAbilityDefs = new List<string>
        {
            "Hush",
            "Deal Physical DMG equal to 100% of Dedra's ATK to a target enemy.",
            "Eyes on the Hunt",
            "• Dedra's next 3 Basic Attacks deal 185% of her ATK instead and she gains +14% Armor Pen.\n• This bonus increases to 225% of ATK for targets below 35% of their Max HP.\n• Lasts 3 Rounds.",
            "Shadow among the Trees",
            "• Dedra's next basic attack costs -2 Mana and has +1 Range.\n• If the target dies, this effect is refreshed."
        };

        citrineAbilityDefs = new List<string>
        {
            "Holo Earth",
            "Deal Magic DMG equal to 120% of Citrine's ATK to a target enemy.",
            "Dazzle!",
            "Increases all allies' Armor Pen. and Magic Pen. by 12 for 2 Rounds.",
            "Sacred Shards",
            "• Shield all allies negating the next instance of damage.\n• Increase all allies ATK by 50% for 2 Rounds."
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
            divider.anchoredPosition = new Vector2(divider.anchoredPosition.x, 90);
            tooltip.SetActive(false);
        }

        else if (uiObject.activeSelf && phaseHandler.selectedPlayer == null)
            uiObject.SetActive(false);

        if (phaseHandler.selectedPlayer != null)
        {
            // to check flux affinity and update UI accordingly
            if (phaseHandler.selectedPlayer.isMeetingFluxAffinity())
                SignatureMoveSetActive(true);
            else
                SignatureMoveSetActive(false);

            // to update health bar accordingly
            healthText.text = phaseHandler.selectedPlayer.health + " / " + phaseHandler.selectedPlayer.maxHealth;
            healthBar.value = phaseHandler.selectedPlayer.health / phaseHandler.selectedPlayer.maxHealth;
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
        // health bar text
        healthText = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();

        // tooltip text
        headerText = GameObject.Find("HeaderText").GetComponent<TextMeshProUGUI>();
        subText = GameObject.Find("SubText").GetComponent<TextMeshProUGUI>();
        subText2 = GameObject.Find("SubText2").GetComponent<TextMeshProUGUI>();
        bodyText = GameObject.Find("BodyText").GetComponent<TextMeshProUGUI>();

        // for active effects displayed in tooltip
        effectTexts = new List<TextMeshProUGUI>();

        for (int i = 5; i < aspirantStats.transform.childCount; i++)
            effectTexts.Add(aspirantStats.transform.GetChild(i).GetComponent<TextMeshProUGUI>());

        ResetEffectDisplays();
    }

    void ResetEffectDisplays()
    {
        for (int i = 5; i < aspirantStats.transform.childCount-1; i++)
        {
            TextMeshProUGUI textField = aspirantStats.transform.GetChild(i).GetComponent<TextMeshProUGUI>();

            // empty text
            textField.text = "";

            // blank sprite that camouflages to the background
            Transform textFieldTransform = textField.gameObject.transform;
            Image effectImage = textFieldTransform.GetChild(textFieldTransform.childCount-1).GetComponent<Image>();
            effectImage.sprite = null;
            effectImage.color = new Color32(158, 160, 173, 255);
        }

        // empty text for last text (might be And more..)
        effectTexts[9].text = "";
    }

    public void SetupButtonsAndImages()
    {
        string name = phaseHandler.selectedPlayer.objectName;

        if (name.Equals("Maiko"))
        {
            currentButtons = maikoButtons;
            aspirantImage.sprite = maikoPfp;
            nameTag.sprite = maikoNameTag;
            currentActiveBorder = maikoBorder;
        }
        else if (name.Equals("Dedra"))
        {
            currentButtons = dedraButtons;
            aspirantImage.sprite = dedraPfp;
            nameTag.sprite = dedraNameTag;
            currentActiveBorder = dedraBorder;
        }
        else if (name.Equals("Citrine"))
        {
            currentButtons = citrineButtons;
            aspirantImage.sprite = citrinePfp;
            nameTag.sprite = citrineNameTag;
            currentActiveBorder = citrineBorder;
        }
        
        for(int i = 0; i < actionButtons.Count-1; i++)
        {
            // set the hover sprite to their respective buttons
            SpriteState newSpriteState = new SpriteState();
            newSpriteState.highlightedSprite = currentButtons[i*3];
            actionButtons[i].spriteState = newSpriteState;

            // set each sprite to their unclicked states
            actionButtons[i].GetComponent<Image>().sprite = currentButtons[i*3+1];
        }

        // (by default) set sig to inactive
        actionButtons[actionButtons.Count-1].GetComponent<Image>().sprite = sigInactive;

        // set up highlighted sprite for the signature move
        SpriteState sigSpriteState = new SpriteState();
        sigSpriteState.highlightedSprite = currentButtons[6];
        actionButtons[actionButtons.Count-1].spriteState = sigSpriteState;

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
            signatureMoveButton.GetComponent<Image>().sprite = currentButtons[7];

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

        eventSystem.SetSelectedGameObject(null);
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

                lastClickedAbility.GetComponent<Image>().sprite = currentButtons[index * 3 + 2];
            }
        }
        else
        {
            if (lastClickedAbility == traverseButton)
                lastClickedAbility.GetComponent<Image>().sprite = traverseUnclicked;
            else
            {
                int index = actionsInOrder.IndexOf(lastClickedAbility.name);

                lastClickedAbility.GetComponent<Image>().sprite = currentButtons[index * 3 + 1];
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
            int index = actionsInOrder.IndexOf(phaseHandler.playerAspirant.selectedAbility);
            string name = phaseHandler.selectedPlayer.objectName;

            if (lastClickedAbility == traverseButton)
            {
                headerText.text = "Traverse";

                // show movement stat of selected player
                subText.text = "Mana Cost: 0";
                subText2.text = "Range: " + phaseHandler.selectedPlayer.movement;

                bodyText.text = traverseAbilityDef;
            }

            else
            {
                int range = 0;

                if (index == 0)
                {
                    subText.text = "Mana Cost: " + phaseHandler.selectedPlayer.basicAttackMana;

                    range = (int) phaseHandler.selectedPlayer.basicAttackRange;
                }
                else if (index == 1)
                {
                    subText.text = "Mana Cost: " + phaseHandler.selectedPlayer.skillMana;
                    
                    range = (int) phaseHandler.selectedPlayer.skillRange;
                }
                else if (index == 2)
                {
                    subText.text = "Mana Cost: " + phaseHandler.selectedPlayer.signatureMoveMana;
                    
                    range = (int) phaseHandler.selectedPlayer.signatureMoveRange;
                }

                if (range > 0)
                {
                    range += phaseHandler.playerAspirant.additionalRange;

                    subText2.text = "Range: " + range;
                }
                else if (range == 0)
                    subText2.text = "Range: Self (0)";
                else
                    subText2.text = "Range: Global";


                if (name.Equals("Maiko"))
                {
                    headerText.text = maikoAbilityDefs[index*2];
                    bodyText.text = maikoAbilityDefs[index*2+1];
                }

                else if (name.Equals("Dedra"))
                {
                    headerText.text = dedraAbilityDefs[index*2];
                    bodyText.text = dedraAbilityDefs[index*2+1];
                }
                
                else if (name.Equals("Citrine"))
                {
                    headerText.text = citrineAbilityDefs[index*2];
                    bodyText.text = citrineAbilityDefs[index*2+1];
                }
            }

            divider.anchoredPosition = new Vector2(divider.anchoredPosition.x, 90);
            aspirantStats.SetActive(false);
        }

        else
        {
            headerText.text = "";
            subText.text = "";
            subText2.text = "";
            bodyText.text = "ACTIVE EFFECTS";

            bool hasEffects = false;
            ResetEffectDisplays();
            int effectsInTooltip = 0;

            Hex currentHex = GetCurrentHex();
            int additionalRange = GetAdditionalRange(currentHex);

            // check if current tile has effects on range
            if (additionalRange != 0)
            {
                effectsInTooltip = 1;
                
                if (additionalRange < 0)
                    effectTexts[0].text = "\t-1 range";
                else
                    effectTexts[0].text = "\t+1 range";

                Transform effectTransform = effectTexts[0].transform;
                Image effectImage = effectTransform.GetChild(effectTransform.childCount-1).GetComponent<Image>();

                RectTransform rt = effectImage.GetComponent<RectTransform>();
                rt.localScale = new Vector3(0.2f, 0.5f, rt.localScale.z);

                effectImage.sprite = currentHex.hexSprite.sprite;

                hasEffects = true;
            }

            List<float> stats = GetAspirantStats();
            List<float> originalStats = GetAspirantStats();

            int offset = effectHandler.effectList.Count-1;

            for (int i = (effectHandler.effectList.Count-1) - effectsInTooltip; i >= 0; i--)
            {
                if (offset-i >= effectTexts.Count-1)
                {
                    effectTexts[9].text = "\t(And more..)";
                    break;
                }

                StatusEffect effect = effectHandler.effectList[i];

                if (effect.targets.Contains(phaseHandler.selectedPlayer))
                {
                    int index = statsInOrder.IndexOf(effect.statusEffectName);

                    if (index >= 0)
                    {
                        if (effect.isAdditive)
                        {
                            if (index < 5)
                                originalStats[index] = originalStats[index] - effect.statusEffect;

                            effectTexts[offset-i].text = "\t+" + effect.statusEffect + " " + statAbbreviations[index];
                        }
                        else
                        {
                            if (index < 5)
                                originalStats[index] = originalStats[index] / effect.statusEffect;
                            
                            effectTexts[offset-i].text = "\t+" + ((effect.statusEffect-1)*100) + "% " + statAbbreviations[index];
                        }

                        Transform effectTransform = effectTexts[offset-i].transform;
                        Image effectImage = effectTransform.GetChild(effectTransform.childCount-1).GetComponent<Image>();
                        effectImage.color = Color.white;

                        string[] sourceDetails = effect.statusEffectSource.Split(" ");
                        int j = actionsInOrder.IndexOf(sourceDetails[1]);

                        RectTransform rt = effectImage.GetComponent<RectTransform>();

                        if (sourceDetails[0].Equals("Tile"))
                        {
                            if (sourceDetails[1].Equals("WindsweptWoods"))
                                effectImage.sprite = windsweptWoods;

                            rt.localScale = new Vector3(0.2f, 0.5f, rt.localScale.z);
                        }
                        else
                        {
                            rt.localScale = new Vector3(0.28f, 0.28f, rt.localScale.z);

                            if (sourceDetails[0].Equals("Maiko"))
                                effectImage.sprite = maikoButtons[j*3+1];

                            else if (sourceDetails[0].Equals("Dedra"))
                                effectImage.sprite = dedraButtons[j*3+1];

                            else if (sourceDetails[0].Equals("Citrine"))
                                effectImage.sprite = citrineButtons[j*3+1];
                        }

                        hasEffects = true;
                    }
                }
            }

            if (!hasEffects)
                effectTexts[0].text = "\t  • None";

            for (int i = 0; i < stats.Count; i++)
            {
                Transform stat = aspirantStats.transform.GetChild(i);

                TextMeshProUGUI textField = stat.GetChild(0).GetComponent<TextMeshProUGUI>();
                textField.text = (Math.Round(stats[i],1)).ToString();

                if (stats[i] == originalStats[i])
                {
                    // neutral
                    stat.gameObject.GetComponent<Image>().sprite = statIcons[i*3];
                }
                else if (stats[i] > originalStats[i])
                {
                    // up
                    stat.gameObject.GetComponent<Image>().sprite = statIcons[i*3+1];
                }
                else if (stats[i] < originalStats[i])
                {
                    // down
                    stat.gameObject.GetComponent<Image>().sprite = statIcons[i*3+2];
                }
            }

            divider.anchoredPosition = new Vector2(divider.anchoredPosition.x, 58);
            aspirantStats.SetActive(true);
        }
    }

    public void OpenTooltip()
    {
        // transition into clicked state
        infoButton.GetComponent<Image>().sprite = infoClicked;

        // open tooltip
        tooltip.SetActive(true);
    }

    List<float> GetAspirantStats()
    {
        List<float> stats = new List<float>();

        stats.Add(phaseHandler.selectedPlayer.attackStat);        // 0
        stats.Add(phaseHandler.selectedPlayer.armor);             // 1
        stats.Add(phaseHandler.selectedPlayer.magicResistance);   // 2
        stats.Add(phaseHandler.selectedPlayer.armorPenetration);  // 3
        stats.Add(phaseHandler.selectedPlayer.magicPenetration);  // 4

        return stats;
    }

    Hex GetCurrentHex()
    {
        AspirantMovement aspirant = phaseHandler.selectedPlayer.GetComponent<AspirantMovement>();

        return phaseHandler.playerAspirant.tiles.Tiles[aspirant.currentYIndex, aspirant.currentXIndex].GetComponent<Hex>();
    }

    int GetAdditionalRange(Hex currentHex)
    {
        if (phaseHandler.mountainMakingFluxes.Contains(currentHex.currentFlux)) // if aspirant is currently on a mountain
            return 1; // there is an additional 1 range for abilities
        else if (currentHex.currentFlux == FluxNames.Sandstorm) // if aspirant is currently in a sandstorm
            return -1; // there is an additonal -1 range for abilties

        return 0;
    }
}
