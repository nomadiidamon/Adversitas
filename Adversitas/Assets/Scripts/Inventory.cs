using System;
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
