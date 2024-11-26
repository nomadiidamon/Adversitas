using System;
using System.Collections.Generic;

public class StatMediator
{
    private Stat stat;

    public StatMediator(Stat stat)
    {
        this.stat = stat;
    }

    public void AddResistance()
    {
        
    }

    public void AddFactor()
    {
        
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
        int baseValue = 100 * stat.m_value;
        return baseValue;
    }

    private void ApplyResistances()
    {

    }

    private void ApplyFactors()
    {

    }
}