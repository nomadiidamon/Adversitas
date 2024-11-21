using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum ItemType {BASIC_ITEM, KEY_ITEM, MATERIAL_ITEM, EQUIPMENT_ITEM, CONSUMABLE_ITEM}

public class Item
{
	private string m_name {  get; set; }
	ItemType m_type { get; set; }
	private int m_value { get; set; }
	private string m_description {  get; set; }
	

	public Item(string name, int value, string description, ItemType type)
	{
		m_name = name;
		m_type = type;
		m_value = value;
		m_description = description;
	}



}
