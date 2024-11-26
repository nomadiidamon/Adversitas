using System;
using UnityEngine;

[System.Serializable]
[SerializeField]
public class Stat : Level
{
    [Header("Name")]
    [SerializeField] public string m_name;

    [Header("Progression Limiters")]
    [SerializeField] public int firstLimitValue;
    [SerializeField] public float firstLimitRate;
    [SerializeField] public int secondLimitValue;
    [SerializeField] public float secondLimitRate;  

    [SerializeField]
    public Stat(string name, int level)
        :base(level)
	{
        m_name = name;
	}



}
