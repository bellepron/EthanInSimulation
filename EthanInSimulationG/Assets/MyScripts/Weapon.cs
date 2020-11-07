using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float weaponDamage;
    private float powerFactor;

    void Awake()
    {
        powerFactor = GameObject.FindWithTag("Player").GetComponent<MyController>().arrowPowerFactor;
        if (powerFactor == 0)
            powerFactor = 1; //for melee attack(Swords)
        weaponDamage *= powerFactor;
    }
    void Update()
    {

    }

}
