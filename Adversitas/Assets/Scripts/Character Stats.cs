using System.Collections.Generic;
using Cinemachine;
using UnityEditor.SceneManagement;
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
        health.m_value = 10;
        // Health("Health", 10, 100, 1.25f);
        stamina = new Stamina("Stamina", 10, 100, 1.25f);
        mana = new Mana("Mana", 10, 100, 1.25f);

    }

    void Update()
    {
        
    }
}
