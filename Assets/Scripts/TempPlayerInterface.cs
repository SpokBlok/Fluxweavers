using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TempPlayerInterface : MonoBehaviour
{
    [SerializeField] PlayerObject player;
    [SerializeField] ResourceScript rs;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI manaText;
    private float health;
    private int mana;
    void Start()
    {
        health = player.health;
        mana = rs.playerManaCount;
    }

    // Update is called once per frame
    void Update()
    {
        health = player.health;
        mana = rs.playerManaCount;
        hpText.text = $"Player HP: {health}";
        manaText.text = $"Player mana: {mana}";
    }
}
