using System;

public class Item
{
	private string m_name {  get; set; }
	enum ItemType m_type = {BASIC_ITEM, KEY_ITEM, MATERIAL_ITEM, EQUIPMENT_ITEM, CONSUMABLE_ITEM, HOT_KEY} { get; set; }
	private int m_value { get; set; }
	private string m_description {  get; set; }
	

	public Item(string name, int value, string description, ItemType type)
	:
	{

	}


}



