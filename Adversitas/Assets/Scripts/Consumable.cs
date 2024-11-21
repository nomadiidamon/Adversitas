using System;

public class Consumable : Item
{
    private delegate void m_effect();

    public Consumable(string name, int value, string description, ItemType type)
        : base(name, value, description, type)
    {

    }


}