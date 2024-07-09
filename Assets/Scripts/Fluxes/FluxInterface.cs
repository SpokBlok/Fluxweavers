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
    [SerializeField] TextMeshProUGUI castFluxText;

    [SerializeField] GameObject singe;
    [SerializeField] GameObject blaze;
    [SerializeField] GameObject scald;
    [SerializeField] GameObject wildfire;
    [SerializeField] GameObject cinderCone;

    [SerializeField] GameObject scorchingWinds;

    private List<GameObject> fluxes;
    public enum Elements {
        Ignis,
        Aqua,
        Folia,
        Terra,
        Ventus
    };

    public enum FluxNames {
        None,
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
    private FluxNames castedFlux;
    private List<Elements> currentElements = new List<Elements>();
    public Flux pickedUpFlux;

    void Start() {
        fluxes = new List<GameObject>{singe, blaze, scald, wildfire, cinderCone, scorchingWinds};
        Clear();
        pickedUpFlux = null;
        castedFlux = FluxNames.None;

        foreach(GameObject flux in fluxes) {
            Debug.Log(flux.name);
        }
    }

    public void FluxPlaced(GameObject fluxObject) {
        pickedUpFlux = fluxObject.GetComponent<Flux>();
        Destroy(fluxObject);
    }

    void Update() {
    }

    void AddElement(Elements element) {
        if(currentElements.Count < 2){
            currentElements.Add(element);
        }
    }
    public void Cast(FluxNames fluxName){
        if(castedFlux != FluxNames.None) {
            foreach(GameObject flux in fluxes) {
                if(flux.name == fluxName.ToString()) {
                    Debug.Log("Instantiated!");
                    Instantiate(flux, GameObject.Find("FluxHolder").transform);
                }
            }
        }
    }
    
    public void Clear(){
        ElementsChanged();
        currentElements.Clear();
        castFluxText.text = "";
    }
    
    public void AddIgnis(){
        AddElement(Elements.Ignis);
        ElementsChanged();
    }
    
    public void AddAqua(){
        AddElement(Elements.Aqua);
        ElementsChanged();
    }
    
    public void AddFolia(){
        AddElement(Elements.Folia);
        ElementsChanged();
    }
    
    public void AddTerra(){
        AddElement(Elements.Terra);
        ElementsChanged();
    }
    
    public void AddVentus(){
        AddElement(Elements.Ventus);
        ElementsChanged();
    }

    private void ElementsChanged(){
        GetCombination();
        if(GameObject.Find("FluxHolder").transform.childCount > 0)
            Destroy(GameObject.Find("FluxHolder").transform.GetChild(0).gameObject);
        Cast(castedFlux);
    }

    private void GetCombination() {
        switch(currentElements.Count) {
            case 1:
                switch(currentElements[0]) {
                    case Elements.Ignis:
                        castedFlux = FluxNames.Singe;
                        break;
                    case Elements.Aqua:
                        castedFlux = FluxNames.HighTide;
                        break;
                    case Elements.Folia:
                        castedFlux = FluxNames.Regrowth;
                        break;
                    case Elements.Terra:
                        castedFlux = FluxNames.EarthArise;
                        break;
                    case Elements.Ventus:
                        castedFlux = FluxNames.Gust;
                        break;
                }
                break;
            case 2:
                switch(currentElements[0]) {
                    
                    case Elements.Ignis:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedFlux = FluxNames.Blaze;
                                break;
                            case Elements.Aqua:
                                castedFlux = FluxNames.Scald;
                                break;
                            case Elements.Folia:
                                castedFlux = FluxNames.Wildfire;
                                break;
                            case Elements.Terra:
                                castedFlux = FluxNames.CinderCone;
                                break;
                            case Elements.Ventus:
                                castedFlux = FluxNames.ScorchingWinds;
                        
                            break;
                        }
                        break;
                    
                    case Elements.Aqua:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedFlux = FluxNames.Scald;
                                break;
                            case Elements.Aqua:
                                castedFlux = FluxNames.Rivershape;
                                break;
                            case Elements.Folia:
                                castedFlux = FluxNames.Swamp;
                                break;
                            case Elements.Terra:
                                castedFlux = FluxNames.Waterfall;
                                break;
                            case Elements.Ventus:
                                castedFlux = FluxNames.Rain;
                            break;
                        }
                        break;
                    
                    case Elements.Folia:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedFlux = FluxNames.Wildfire;
                                break;
                            case Elements.Aqua:
                                castedFlux = FluxNames.Swamp;
                                break;
                            case Elements.Folia:
                                castedFlux = FluxNames.Reforestation;
                                break;
                            case Elements.Terra:
                                castedFlux = FluxNames.MountainSpires;
                                break;
                            case Elements.Ventus:
                                castedFlux = FluxNames.WindsweptWoods;
                        
                            break;
                        }
                        break;
                    
                    case Elements.Terra:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedFlux = FluxNames.CinderCone;
                                break;
                            case Elements.Aqua:
                                castedFlux = FluxNames.Waterfall;
                                break;
                            case Elements.Folia:
                                castedFlux = FluxNames.MountainSpires;
                                break;
                            case Elements.Terra:
                                castedFlux = FluxNames.SeismicWave;
                                break;
                            case Elements.Ventus:
                                castedFlux = FluxNames.Sandstorm;
                            break;
                        }
                        break;
                    
                    case Elements.Ventus:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                castedFlux = FluxNames.ScorchingWinds;
                                break;
                            case Elements.Aqua:
                                castedFlux = FluxNames.Rain;
                                break;
                            case Elements.Folia:
                                castedFlux = FluxNames.WindsweptWoods;
                                break;
                            case Elements.Terra:
                                castedFlux = FluxNames.Sandstorm;
                                break;
                            case Elements.Ventus:
                                castedFlux = FluxNames.Tornado;
                        
                            break;
                        }
                        break;
                }
                break;
            default:
                castedFlux = FluxNames.None;
                break;
        }
        
        castFluxText.text = castedFlux == FluxNames.None ? "" : castedFlux.ToString() ?? "" ;
        currentElement1Text .text = currentElements.Count >= 1 ? currentElements[0].ToString() : "";
        currentElement2Text.text = currentElements.Count >= 2 ? currentElements[1].ToString() : "";
    }
}


