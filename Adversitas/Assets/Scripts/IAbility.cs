using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public interface IAbility
{
	void ActivateAbility();
}
	

public class Ability : Monobehaviour, IAbility
{
	protected Action myAbility;

    protected virtual void Start()
	{
        myAbility = BeginAbility();
	}

	private void ActivateAbility ()
	{
		myAbility?.Invoke();
	};

	protected abstract void BeginAbility();




}
