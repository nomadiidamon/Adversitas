using System;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class Level : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] public int m_value;

    [Header("Experience / Progression")]
    [SerializeField] public int m_currExperience;
    [SerializeField] public int m_expToNextLevel;
    [SerializeField] public float m_levelRate;


    public Level(int value, int currExperience = 0, int expToNextLevel = 100, float levelRate = 1.0f)
    {
        m_value = value;
        m_currExperience = currExperience;
        m_expToNextLevel = expToNextLevel;
        m_levelRate = levelRate;
    }

    public bool AddExperience(int exp)
    {
        m_currExperience += exp;

        if (m_currExperience >= m_expToNextLevel)
        {
            LevelUp();
            return true;
        }

        return false;
    }

    private void LevelUp()
    {
        m_value++;
        m_currExperience -= m_expToNextLevel;
        m_expToNextLevel = (int)(m_expToNextLevel * m_levelRate * m_value);
    }

}
