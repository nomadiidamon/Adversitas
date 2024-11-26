using System;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class Health : Stat
{
    [Header("Values")]
    [SerializeField] public int currentHealthPoints;
    [SerializeField] public int maxHealthPoints;

    public Health(string name, int level, int expToNextLvl, float levelRate)
        :base(name, level)
	{
        m_expToNextLevel = expToNextLvl;
        m_levelRate = levelRate;

        CalculateInitialHealthPoints();        
    }

    private void CalculateInitialHealthPoints()
    {
        if (m_value > 1)
        {
            int target = 175 * m_value;
            currentHealthPoints = target;
            maxHealthPoints = 300 * m_value;
        }
        else
        {
            currentHealthPoints = 175;
            maxHealthPoints = 300;
        }
    }
}