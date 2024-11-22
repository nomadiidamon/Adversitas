using System;

[System.Serializable]
public class Stat
{
	public string m_name {  get; set; }
	public Level m_level { get; set; }

    public int firstLimitValue { get; set; }
    public int secondLimitValue { get; set; }
    public float firstLimitRate { get; set; }
    public float secondLimitRate { get; set; }

    public Stat(string name, int level)
	{
        m_name = name;
        m_level.m_value = level;
	}



}
