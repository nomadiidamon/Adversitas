using System;

[System.Serializable]
public class Level
{
    public int m_value { get; set; }
    public int m_currExperience { get; set; }
    public int m_expToNextLevel { get; set; }
    public float m_levelRate {  get; set; }


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
