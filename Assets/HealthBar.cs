using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public int health;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetHealth(int setHealth)
    {
        health = setHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
