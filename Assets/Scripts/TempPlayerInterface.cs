using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempPlayerInterface : MonoBehaviour
{
    [SerializeField] PlayerObject player;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI manaText;
    private float health;
    private int mana;
    void Start()
    {
        health = player.health;
        mana = player.mana;
    }

    // Update is called once per frame
    void Update()
    {
        health = player.health;
        mana = player.mana;
        hpText.text = $"Player HP: {health}";
        manaText.text = $"Player mana: {mana}";
    }
}
