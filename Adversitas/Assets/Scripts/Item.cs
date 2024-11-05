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


public class Consumable : Item
{
	private delegate void m_effect();

    public Consumable(string name, int value, string description, ItemType type) 
		: base(name, value, description, type)
    {
    
	}


}

public class Equipment : Item
{
	int m_maxDurability { get; set; }
    int m_currDurability { get; set; }
    int m_statBonus { get; set; }
	int m_weight { get; set; }

	public Equipment(string name, int value, string description, ItemType type, 
		int max_durability, int curr_durability, int statBonus, int weight) 
		: base(name, value, description, type)
    {
		m_maxDurability = max_durability;
		m_currDurability = curr_durability;
		m_statBonus = statBonus;
		m_weight = weight;
	}
}

public enum ArmorSlot { HEAD, CHEST, ARMS, LEGS}

public class Armor : Equipment
{
	int m_defense { get; set; }
	ArmorSlot armorSlot { get; set; }

	public Armor(string name, int value, string description, ItemType type, 
		int max_durability, int curr_durability, int statBonus, int weight, 
		int defense, ArmorSlot slot)
        : base(name, value, description, type, max_durability, curr_durability, statBonus, weight)
    {
		m_defense = defense;
		armorSlot = slot;
	} 

}

public class Weapon : Equipment
{
	int m_accuracy { get; set; }
	int m_damage { get; set; }

	public Weapon(string name, int value, string description, ItemType type,
        int max_durability, int curr_durability, int statBonus, int weight,
		int accuracy, int damage)
		: base(name, value, description, type, max_durability, curr_durability, statBonus, weight)
    {
		m_accuracy = accuracy;
		m_damage = damage;
	}
}


