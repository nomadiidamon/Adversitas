using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : ScriptableObject
{
	List<Item> items;
	int maxCapacity;
	int currentCapacity;
	int totalWeight;

	public Inventory()
	{

	}
}
