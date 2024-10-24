using System;

public class Stamina
{

	private float currStaminaPoints; { get; set; }
	private float maxStaminaPoints; {  get; set; }
    private float staminaRegenRate; { get; set; }

    public Stamina()
	{

	}

	public void ReduceStamina(float value)
	{

	}

	public UpdateStaminaValues()
	{
		if (currStaminaPoints == null || maxStaminaPoints == 
			null || staminaRegenRate == null)
		{
			Debug.Log("not enough values");
			return;
		}

	}

}
