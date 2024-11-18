using System;
using System.Collections.Generic;

public class StatMediator
{
    private Stat stat;
    private List<Resistances> resistances;
    private List<Factors> factors;

    public StatMediator(Stat stat)
    {
        this.stat = stat;
        this.resistances = new List<Resistances>();
        this.factors = new List<Factors>();
    }

    public void AddResistance(Resistances resistance)
    {
        resistances.Add(resistance);
    }

    public void AddFactor(Factors factor)
    {
        factors.Add(factor);
    }

    public int CalculateStatValue()
    {
        int baseValue = CalculateBaseValue();

        ApplyResistances();

        ApplyFactors();

        return baseValue;
    }

    private int CalculateBaseValue()
    {
        int baseValue = 100 * stat.m_level.m_value;
        return baseValue;
    }

    private void ApplyResistances()
    {
        foreach (var resistance in resistances)
        {
            resistance.ApplyResistance();
        }
    }

    private void ApplyFactors()
    {
        foreach (var factor in factors)
        {
        }
    }
}