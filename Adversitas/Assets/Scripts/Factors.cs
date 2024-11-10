using System;

public interface IStatusEffect()
{
    public void ApplyStatusEffect();
}

public interface IPositiveStatusEffect : IStatusEffect
{
    public void ApplyBuff();
}

public interface INegativeStatusEffect : IStatusEffect
{
    public void ApplyDebuff();
}


public class PositiveStatus : IPositiveStatusEffect
{
    enum PositiveStatus { equilibrium, other }// FLowState, gainHealth, gainMana, gainStamina, breakOpponenet, executeOpponent, dodge, other };
    public PositiveStatus status;
    public bool isActive;
    // my function pointer to give an effect to 


    public void ApplyBuff()
    {
        SetPositiveStatus();
    }

    public SetPositiveStatus(int statusType = 0)
    {
        status = statusType;
        // call out function that assigns the function pointer.
        //function pointer do your thing
    }

    // create funtion to assign our function pointer to something other than null
}

public class NegativeStatus
{
    enum NegativeStatus { equilibrium, other }// ToHeat, ToCold, Wet, Shocked, Burdened, Poisoned, Bleeding, stunned, other }
    public NegativeStatus status;
    public bool isActive;


    public SetNegativeStatus(int statusType)
    {
        status = statusType;
    }

}

public class Resistances
{
    PositiveStatus m_positiveStatus;
    NegativeStatus m_negativeStatus;
    public Resistances(bool positive, int Key)
    {
        if (positive)
        {
            m_positiveStatus.m_positiveStatus.status 
        }
    }
}

public class Factors
{
    List<Resistances> m_resistances;

    Factors(int Positve, int Negative)
    {
        for (int i = 0; i < (Positve); i++)
        {
            m_resistances.push_back(new Resistances(true, 1));
        }
        for (int i = 0; i < (Negative); i++)
        {
            m_resistances.push_back(new Resistances(true, 0));
        }

    }

    //method to remove all factors after equilibrium is hit. always keep (5)


}

