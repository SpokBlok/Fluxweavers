using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FluxInterface : MonoBehaviour
{
    
    [SerializeField] Button ignisButton;
    [SerializeField] Button aquaButton;
    [SerializeField] Button foliaButton;
    [SerializeField] Button terraButton;
    [SerializeField] Button ventusButton;
    [SerializeField] Button castButton;
    [SerializeField] Button clearButton;
    [SerializeField] TextMeshProUGUI currentElement1Text;
    [SerializeField] TextMeshProUGUI currentElement2Text;
    [SerializeField] TextMeshProUGUI castSpellText;

    public enum Elements {
        Ignis,
        Aqua,
        Folia,
        Terra,
        Ventus
    };

    public enum FluxNames {
        Singe,
        Blaze,
        Scald,
        Wildfire,
        CinderCone,
        ScorchingWinds,
        HighTide,
        Rivershape,
        Swamp,
        Waterfall,
        Rain,
        Regrowth,
        Reforestation,
        MountainSpires,
        WindsweptWoods,
        EarthArise,
        SeismicWave,
        Sandstorm,
        Gust,
        Tornado
    }
    private FluxNames castedSpell;
    private List<Elements> currentElements = new List<Elements>();
    
    void Start() {
        currentElements.Clear();
    }

    void Update() {
        currentElement1Text.text = currentElements.Count >= 1 ? currentElements[0].ToString() : "";
        currentElement2Text.text = currentElements.Count >= 2 ? currentElements[1].ToString() : "";

    }

    void AddElement(Elements element) {
        if(currentElements.Count < 2){
            currentElements.Add(element);
        }
    }
    
    public void Cast(){
        switch(currentElements.Count) {
            case 1:
                switch(currentElements[0]) {
                    case Elements.Ignis:
                        castedSpell = FluxNames.Singe;
                        break;
                    case Elements.Aqua:
                        castedSpell = FluxNames.HighTide;
                        break;
                    case Elements.Folia:
                        castedSpell = FluxNames.Regrowth;
                        break;
                    case Elements.Terra:
                        castedSpell = FluxNames.EarthArise;
                        break;
                    case Elements.Ventus:
                        castedSpell = FluxNames.Gust;
                        break;
                }
                break;
            case 2:
                switch(currentElements[0]) {
                    
                    case Elements.Ignis:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedSpell = FluxNames.Blaze;
                                break;
                            case Elements.Aqua:
                                castedSpell = FluxNames.Scald;
                                break;
                            case Elements.Folia:
                                castedSpell = FluxNames.Wildfire;
                                break;
                            case Elements.Terra:
                                castedSpell = FluxNames.CinderCone;
                                break;
                            case Elements.Ventus:
                                castedSpell = FluxNames.ScorchingWinds;
                        
                            break;
                        }
                        break;
                    
                    case Elements.Aqua:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedSpell = FluxNames.Scald;
                                break;
                            case Elements.Aqua:
                                castedSpell = FluxNames.Rivershape;
                                break;
                            case Elements.Folia:
                                castedSpell = FluxNames.Swamp;
                                break;
                            case Elements.Terra:
                                castedSpell = FluxNames.Waterfall;
                                break;
                            case Elements.Ventus:
                                castedSpell = FluxNames.Rain;
                            break;
                        }
                        break;
                    
                    case Elements.Folia:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedSpell = FluxNames.Wildfire;
                                break;
                            case Elements.Aqua:
                                castedSpell = FluxNames.Swamp;
                                break;
                            case Elements.Folia:
                                castedSpell = FluxNames.Reforestation;
                                break;
                            case Elements.Terra:
                                castedSpell = FluxNames.MountainSpires;
                                break;
                            case Elements.Ventus:
                                castedSpell = FluxNames.WindsweptWoods;
                        
                            break;
                        }
                        break;
                    
                    case Elements.Terra:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedSpell = FluxNames.CinderCone;
                                break;
                            case Elements.Aqua:
                                castedSpell = FluxNames.Waterfall;
                                break;
                            case Elements.Folia:
                                castedSpell = FluxNames.MountainSpires;
                                break;
                            case Elements.Terra:
                                castedSpell = FluxNames.SeismicWave;
                                break;
                            case Elements.Ventus:
                                castedSpell = FluxNames.Sandstorm;
                            break;
                        }
                        break;
                    
                    case Elements.Ventus:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedSpell = FluxNames.ScorchingWinds;
                                break;
                            case Elements.Aqua:
                                castedSpell = FluxNames.Rain;
                                break;
                            case Elements.Folia:
                                castedSpell = FluxNames.WindsweptWoods;
                                break;
                            case Elements.Terra:
                                castedSpell = FluxNames.Sandstorm;
                                break;
                            case Elements.Ventus:
                                castedSpell = FluxNames.Tornado;
                        
                            break;
                        }
                        break;
                }
                break;
            default:
                break;
        }

        // Replace with actual spell code
      
        currentElements.Clear();
        castSpellText.text = castedSpell.ToString() ?? "";
    }
    
    public void Clear(){
        currentElements.Clear();
    }
    
    public void AddIgnis(){
        AddElement(Elements.Ignis);
    }
    
    public void AddAqua(){
        AddElement(Elements.Aqua);
    }
    
    public void AddFolia(){
        AddElement(Elements.Folia);
    }
    
    public void AddTerra(){
        AddElement(Elements.Terra);
    }
    
    public void AddVentus(){
        AddElement(Elements.Ventus);
    }
}


