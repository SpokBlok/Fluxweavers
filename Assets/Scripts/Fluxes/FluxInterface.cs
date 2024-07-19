using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FluxNamespace;
public class FluxInterface : MonoBehaviour
{
    //Terrain interface
    [SerializeField] EnvironmentInterface ei; 
    //Interactables
    [SerializeField] Button ignisButton;
    [SerializeField] Button aquaButton;
    [SerializeField] Button foliaButton;
    [SerializeField] Button terraButton;
    [SerializeField] Button ventusButton;
    [SerializeField] Button castButton;
    [SerializeField] Button clearButton;
    [SerializeField] Image highlight1;
    [SerializeField] Image highlight2;

    //Flux prefabs
    [SerializeField] public GameObject singe;
    [SerializeField] public GameObject blaze;
    [SerializeField] public GameObject scald;
    [SerializeField] public GameObject wildfire;
    [SerializeField] public GameObject cinderCone;
    [SerializeField] public GameObject scorchingWinds;
    [SerializeField] public GameObject highTide;
    [SerializeField] public GameObject rivershape;
    [SerializeField] public GameObject swamp;
    [SerializeField] public GameObject waterfall;
    [SerializeField] public GameObject rain;
    [SerializeField] public GameObject regrowth;
    [SerializeField] public GameObject reforestation;
    [SerializeField] public GameObject mountainSpires;
    [SerializeField] public GameObject windsweptWoods;
    [SerializeField] public GameObject earthArise;
    [SerializeField] public GameObject seismicWave;
    [SerializeField] public GameObject sandstorm;
    [SerializeField] public GameObject gust;
    [SerializeField] public GameObject tornado;
    private Color color1, color2;
    private List<GameObject> fluxes;
    public enum Elements {
        Ignis,
        Aqua,
        Folia,
        Terra,
        Ventus
    };

    public FluxNames FluxNames;
    
    /*
        currentFlux is the current flux made from the element combination
        currentElements is the combination of elements
        castedFlux is the flux that has been newly placed on the board
    */
    private FluxNames currentFlux;
    private List<Elements> currentElements = new List<Elements>();
    public Flux castedFlux;
    [SerializeField] Image led;

    // if true, button clicks will work
    public bool isClickable;

    [SerializeField] ResourceScript rs;
    /*  

        The Start function does the following:
    
        - Instantiates the list of flux prefabs
        - Makes sure the element list, plcedFlux, and castedFlux is empty at start
    */
    void Start() {
        isClickable = true;
        EnvironmentInterface.onToggleUI += ToggleUI; //subscribes to the event when a hex is placed
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
        color1 = highlight1.color;
        color2 = highlight2.color;
    }

    void Update() {
        led.color = Color.Lerp(color1, color2, Mathf.Sin(Time.time)/2.0f+0.5f);
    }
    /*
        THE GENERAL FLOW:
        - Add<Element>, ElementsChanged and clear function updates and generates the current flux and the instantiated prefab
        - The flux can then be picked up by the mouse and placed on a hex tile
        - Once placed, data is sent from the hex to this interface via FluxPlaced
        - If the flux is an environment spell, then the data of the hex is sent to TerrainInterface
    */

    public void FluxPlaced(GameObject fluxObject, Hex hex) {
        castedFlux = fluxObject.GetComponent<Flux>();
        bool castable = true;
        if(!rs.playerAbilityUseCheck(castedFlux.manaCost))
            castable = false;
        if((castedFlux.fluxCode == FluxNames.Gust || castedFlux.fluxCode == FluxNames.Tornado) && hex.HexOccupant() == null)
            castable = false;
        if(hex.currentFlux == FluxNames.CinderCone)
            castable = false;
        if(castable) {
            rs.playerAbilityUseManaUpdate(castedFlux.manaCost);
            Destroy(fluxObject);
            Clear();
            ei.SetFlux(hex, castedFlux);
        }
    }

    public void Cast(FluxNames fluxName){
        if(currentFlux != FluxNames.None) {
            foreach(GameObject flux in fluxes) {
                if(flux.name == fluxName.ToString()) {
                    Instantiate(flux, GameObject.Find("FluxHolder").transform);
                }
            }
        }
    }
    



    public void Clear(){
        currentElements.Clear();
        ElementsChanged();
    }

    void AddElement(Elements element) {
        if(isClickable) {
            if(currentElements.Count < 2){
                currentElements.Add(element);
                ElementsChanged();
            }
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

    void UpdateUI(){
        // castFluxText.text = currentFlux == FluxNames.None ? "" : currentFlux.ToString() ?? "" ;
        // currentElement1Text .text = currentElements.Count >= 1 ? currentElements[0].ToString() : "";
        // currentElement2Text.text = currentElements.Count >= 2 ? currentElements[1].ToString() : "";
        

        if(currentElements.Count >= 0) {
            color1 = color2 = Color.white;
        }
        if(currentElements.Count >= 1) {
            switch(currentElements[0]){
                case Elements.Ignis:
                    ColorUtility.TryParseHtmlString("#CB342A", out color1);
                    break;
                case Elements.Aqua:
                    ColorUtility.TryParseHtmlString("#2697FE", out color1);
                    break;
                case Elements.Folia:
                    ColorUtility.TryParseHtmlString("#3FB470", out color1);
                    break;
                case Elements.Terra:
                    ColorUtility.TryParseHtmlString("#B4703C", out color1);
                    break;
                case Elements.Ventus:
                    ColorUtility.TryParseHtmlString("#C6ECE4", out color1);
                    break;
            }
        }
        if(currentElements.Count >= 2) {
            switch(currentElements[1]){
                case Elements.Ignis:
                    ColorUtility.TryParseHtmlString("#CB342A", out color2);
                    break;
                case Elements.Aqua:
                    ColorUtility.TryParseHtmlString("#2697FE", out color2);
                    break;
                case Elements.Folia:
                    ColorUtility.TryParseHtmlString("#3FB470", out color2);
                    break;
                case Elements.Terra:
                    ColorUtility.TryParseHtmlString("#B4703C", out color2);
                    break;
                case Elements.Ventus:
                    ColorUtility.TryParseHtmlString("#C6ECE4", out color2);
                    break;
            }
        }

        highlight1.color = color1;
        highlight2.color = color2;
    }

    private void ToggleUI(){
        isClickable = !isClickable;
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
        
        UpdateUI();
    }
}


