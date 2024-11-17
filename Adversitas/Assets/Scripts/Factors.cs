using System;

public interface IStatusEffect
{
    void ApplyStatusEffect();
    float effectDuration {  get; set; }
}

public interface IPositiveStatusEffect : IStatusEffect
{
    void ApplyBuff();
}

public interface INegativeStatusEffect : IStatusEffect
{
    void ApplyDebuff();
}

public enum PositiveStatusType { equilibrium, other, FLowState, gainHealth, gainMana, gainStamina, breakOpponenet, executeOpponent, dodge };
public enum NegativeStatusType { equilibrium, other, ToHeat, ToCold, Wet, Shocked, Burdened, Poisoned, Bleeding, stunned }

public class PositiveStatus : IPositiveStatusEffect
{

    public PositiveStatusType status {  get; set; }
    public bool isActive { get; set; }
    public float effectDuration { get; set; }
    // my function pointer to give an effect to 
    //public StatusEffectDelegate StatusEffectFunction {  get; set; }


    public void ApplyBuff()
    {
        //StatusEffectFunction?.invoke();

    }

    public void ApplyStatusEffect()
    {
        ApplyBuff();
    }

    public PositiveStatus(PositiveStatusType statusType = PositiveStatusType.equilibrium, float duration = 1.5f)
    {
        status = statusType;
        isActive = false;
        effectDuration = duration;
        // call out function that assigns the function pointer.
        //function pointer do your thing
    }

    //public void SetStatusEffectFunction(StatusEffectDelegate func)
    //{
    //    StatusEffectFunction = func;
    //}
    // create funtion to assign our function pointer to something other than null
}

public class NegativeStatus : INegativeStatusEffect
{
    public NegativeStatusType status {  get; set; }
    public bool isActive { get; set; }
    public float effectDuration { get; set; }

    //public StatusEffectDelegate StatusEffectFunction { get; set; }

    public void ApplyDebuff()
    {
        //StatusEffectFunction?.invoke();
    }

    public void ApplyStatusEffect()
    {
        ApplyDebuff();
    }

    public NegativeStatus(NegativeStatusType statusType = NegativeStatusType.equilibrium, float duration = 1.5f)
    {
        status = statusType;
        effectDuration = duration;
        isActive=false;
    }

    //public void SetStatusEffectFunction(StatusEffectDelegate func)
    //{
    //    StatusEffectFunction = func;
    //}
}

public class Resistances
{
    PositiveStatus m_positiveStatus;
    NegativeStatus m_negativeStatus;
    public Resistances(bool positive, int duration)
    {
        if (positive)
        {
            m_positiveStatus = new PositiveStatus(PositiveStatusType, duration);
        }
        else
        {
            m_negativeStatus = new NegativeStatus(NegativeStatusType, duration);
        }
    }

    public void ApplyResistance()
    {
        if (m_positiveStatus != null && m_positiveStatus.isActive)
        {
            m_positiveStatus.ApplyBuff();
        }

        if (m_negativeStatus != null && m_negativeStatus.isActive)
        {
            m_negativeStatus.ApplyDebuff();
        }

    }

}

public class Factors
{
    //List<Resistances> m_resistances;

    //Factors(int Positve, int Negative)
    //{
    //    for (int i = 0; i < (Positve); i++)
    //    {
    //        m_resistances.push_back(new Resistances(true, 1));
    //    }
    //    for (int i = 0; i < (Negative); i++)
    //    {
    //        m_resistances.push_back(new Resistances(true, 0));
    //    }

   // }

    //method to remove all factors after equilibrium is hit. always keep (5)


}

