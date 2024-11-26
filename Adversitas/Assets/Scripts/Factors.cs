using System;
using UnityEngine;

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

[System.Serializable]
public enum PositiveStatusType { equilibrium, other, FLowState, gainHealth, gainMana, gainStamina, breakOpponenet, executeOpponent, dodge };
[System.Serializable]
public enum NegativeStatusType { equilibrium, other, ToHeat, ToCold, Wet, Shocked, Burdened, Poisoned, Bleeding, stunned }

[System.Serializable]
public class PositiveStatus : IPositiveStatusEffect
{

    [SerializeField] public PositiveStatusType status {  get; set; }
    [SerializeField] public bool isActive { get; set; }
    [SerializeField] public float effectDuration { get; set; }
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

[System.Serializable]
public class NegativeStatus : INegativeStatusEffect
{
    [SerializeField] public NegativeStatusType status {  get; set; }
    [SerializeField] public bool isActive { get; set; }
    [SerializeField] public float effectDuration { get; set; }

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

