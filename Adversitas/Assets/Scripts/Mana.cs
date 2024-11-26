using System;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class Mana : Stat
{
    [Header("Values")]
    [SerializeField] public int currentManaPoints;
    [SerializeField] public int maxManaPoints;


    public Mana(string name, int level, int expToNextLevel, float levelRate)
        : base(name, level)
    {
        m_expToNextLevel = expToNextLevel;
        m_levelRate = levelRate;

        CalculateInitialManaPoints();
    }

    private void CalculateInitialManaPoints()
    {
        if (m_value > 1)
        {
            currentManaPoints = 75 * m_value;  
            maxManaPoints = 120 * m_value;    
        }
        else
        {
            currentManaPoints = 75;
            maxManaPoints = 120;
        }
    }
}
