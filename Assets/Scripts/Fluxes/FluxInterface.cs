using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FluxInterface : MonoBehaviour
{
    //Interactables
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

    //Flux prefabs
    [SerializeField] GameObject singe;
    [SerializeField] GameObject blaze;
    [SerializeField] GameObject scald;
    [SerializeField] GameObject wildfire;
    [SerializeField] GameObject cinderCone;
    [SerializeField] GameObject scorchingWinds;
    [SerializeField] GameObject highTide;
    [SerializeField] GameObject rivershape;
    [SerializeField] GameObject swamp;
    [SerializeField] GameObject waterfall;
    [SerializeField] GameObject rain;
    [SerializeField] GameObject regrowth;
    [SerializeField] GameObject reforestation;
    [SerializeField] GameObject mountainSpires;
    [SerializeField] GameObject windsweptWoods;
    [SerializeField] GameObject earthArise;
    [SerializeField] GameObject seismicWave;
    [SerializeField] GameObject sandstorm;
    [SerializeField] GameObject gust;
    [SerializeField] GameObject tornado;

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
    /*
        currentFlux is the current flux made from the element combination
        currentElements is the combination of elements
        castedFlux is the flux that has been newly placed on the board
    */
    private FluxNames currentFlux;
    private List<Elements> currentElements = new List<Elements>();
    public Flux castedFlux;

    /*  

        The Start function does the following:
    
        - Instantiates the list of flux prefabs
        - Makes sure the element list, plcedFlux, and castedFlux is empty at start
    */
    void Start() {
        fluxes = new List<GameObject>{
            singe, 
            blaze, 
            scald, 
            wildfire, 
            cinderCone, 
            scorchingWinds, 
            highTide, 
            rivershape, 
            swamp,
            waterfall,
            rain,
            regrowth,
            reforestation,
            mountainSpires,
            windsweptWoods,
            earthArise,
            seismicWave,
            sandstorm,
            gust,
            tornado
        };  
        Clear();
        castedFlux = null;
        currentFlux = FluxNames.None;

        foreach(GameObject flux in fluxes) {
            Debug.Log(flux.name);
        }
    }

    /*
        THE GENERAL FLOW:
        - Add<Element>, ElementsChanged and clear function updates and generates the current flux and the instantiated prefab
        - The flux can then be picked up by the mouse and placed on a hex tile
        - Once placed, data is sent from the hex to this interface via FluxPlaced
    */
    public void FluxPlaced(GameObject fluxObject) {
        castedFlux = fluxObject.GetComponent<Flux>();
        Destroy(fluxObject);
        Clear();
    }

    public void Cast(FluxNames fluxName){
        if(currentFlux != FluxNames.None) {
            foreach(GameObject flux in fluxes) {
                if(flux.name == fluxName.ToString()) {
                    Debug.Log("Instantiated!");
                    Instantiate(flux, GameObject.Find("FluxHolder").transform);
                }
            }
        }
    }
    
    public void Clear(){
        currentElements.Clear();
        castFluxText.text = "";
        ElementsChanged();
    }
    void AddElement(Elements element) {
        if(currentElements.Count < 2){
            currentElements.Add(element);
            ElementsChanged();
        }
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

    private void ElementsChanged(){
        GetCombination();
        if(GameObject.Find("FluxHolder").transform.childCount > 0)
            Destroy(GameObject.Find("FluxHolder").transform.GetChild(transform.childCount).gameObject);
        Cast(currentFlux);
    }

    private void GetCombination() {
        switch(currentElements.Count) {
            case 1:
                switch(currentElements[0]) {
                    case Elements.Ignis:
                        currentFlux = FluxNames.Singe;
                        break;
                    case Elements.Aqua:
                        currentFlux = FluxNames.HighTide;
                        break;
                    case Elements.Folia:
                        currentFlux = FluxNames.Regrowth;
                        break;
                    case Elements.Terra:
                        currentFlux = FluxNames.EarthArise;
                        break;
                    case Elements.Ventus:
                        currentFlux = FluxNames.Gust;
                        break;
                }
                break;
            case 2:
                switch(currentElements[0]) {
                    
                    case Elements.Ignis:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                currentFlux = FluxNames.Blaze;
                                break;
                            case Elements.Aqua:
                                currentFlux = FluxNames.Scald;
                                break;
                            case Elements.Folia:
                                currentFlux = FluxNames.Wildfire;
                                break;
                            case Elements.Terra:
                                currentFlux = FluxNames.CinderCone;
                                break;
                            case Elements.Ventus:
                                currentFlux = FluxNames.ScorchingWinds;
                        
                            break;
                        }
                        break;
                    
                    case Elements.Aqua:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                currentFlux = FluxNames.Scald;
                                break;
                            case Elements.Aqua:
                                currentFlux = FluxNames.Rivershape;
                                break;
                            case Elements.Folia:
                                currentFlux = FluxNames.Swamp;
                                break;
                            case Elements.Terra:
                                currentFlux = FluxNames.Waterfall;
                                break;
                            case Elements.Ventus:
                                currentFlux = FluxNames.Rain;
                            break;
                        }
                        break;
                    
                    case Elements.Folia:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                currentFlux = FluxNames.Wildfire;
                                break;
                            case Elements.Aqua:
                                currentFlux = FluxNames.Swamp;
                                break;
                            case Elements.Folia:
                                currentFlux = FluxNames.Reforestation;
                                break;
                            case Elements.Terra:
                                currentFlux = FluxNames.MountainSpires;
                                break;
                            case Elements.Ventus:
                                currentFlux = FluxNames.WindsweptWoods;
                        
                            break;
                        }
                        break;
                    
                    case Elements.Terra:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                currentFlux = FluxNames.CinderCone;
                                break;
                            case Elements.Aqua:
                                currentFlux = FluxNames.Waterfall;
                                break;
                            case Elements.Folia:
                                currentFlux = FluxNames.MountainSpires;
                                break;
                            case Elements.Terra:
                                currentFlux = FluxNames.SeismicWave;
                                break;
                            case Elements.Ventus:
                                currentFlux = FluxNames.Sandstorm;
                            break;
                        }
                        break;
                    
                    case Elements.Ventus:
                        switch(currentElements[1]) {
                            case Elements.Ignis:
                                currentFlux = FluxNames.ScorchingWinds;
                                break;
                            case Elements.Aqua:
                                currentFlux = FluxNames.Rain;
                                break;
                            case Elements.Folia:
                                currentFlux = FluxNames.WindsweptWoods;
                                break;
                            case Elements.Terra:
                                currentFlux = FluxNames.Sandstorm;
                                break;
                            case Elements.Ventus:
                                currentFlux = FluxNames.Tornado;
                        
                            break;
                        }
                        break;
                }
                break;
            default:
                currentFlux = FluxNames.None;
                break;
        }
        
        castFluxText.text = currentFlux == FluxNames.None ? "" : currentFlux.ToString() ?? "" ;
        currentElement1Text .text = currentElements.Count >= 1 ? currentElements[0].ToString() : "";
        currentElement2Text.text = currentElements.Count >= 2 ? currentElements[1].ToString() : "";
    }
}


