using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[SaveDuringPlay]
[System.Serializable]
[SerializeField]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(Mana))]
public class CharacterStats : MonoBehaviour
{
    public Health health;
    public Stamina stamina;
    public Mana mana;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
