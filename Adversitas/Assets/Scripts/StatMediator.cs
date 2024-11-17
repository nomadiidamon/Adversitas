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

    // Method to add a resistance (both positive and negative)
    public void AddResistance(Resistances resistance)
    {
        resistances.Add(resistance);
    }

    // Method to add a factor (could represent different kinds of modifiers)
    public void AddFactor(Factors factor)
    {
        factors.Add(factor);
    }

    // Method to calculate the stat value after applying factors and resistances
    public int CalculateStatValue()
    {
        // Start with the base value (could be the stat's base value or a formula)
        int baseValue = CalculateBaseValue();

        // Apply resistances (Positive buffs and Negative debuffs)
        ApplyResistances();

        // Apply any other factors that might modify the stat (like temporary boosts)
        ApplyFactors();

        // Return the final calculated value
        return baseValue;
    }

    // Base calculation (could include stat level, other base stats, etc.)
    private int CalculateBaseValue()
    {
        int baseValue = 100 * stat.m_level.m_value; // Example base value, depending on stat type
        return baseValue;
    }

    // Apply resistances (buffs and debuffs)
    private void ApplyResistances()
    {
        foreach (var resistance in resistances)
        {
            resistance.ApplyResistance();
        }
    }

    // Apply factors (e.g., temporary boosts or external influences)
    private void ApplyFactors()
    {
        foreach (var factor in factors)
        {
            // Add logic to apply factors to the stat (e.g., scaling, multipliers, etc.)
        }
    }
}