using System;



public enum PhysicalDamageType { PIERCING, SLASHING, BLUNT, NULL}
public enum MagicalDamageType { FIRE, LAVA, METAL, EARTH, WATER, ICE, AIR, ELECTRICITY, NULL}

public class DamageValue
{
	PhysicalDamageType physicalDamage;
	MagicalDamageType magicalDamage;
	int pointsToTakeFromHealth { get; }

	public DamageValue(MagicalDamageType magDamage, int points)
	{
		magicalDamage = magDamage;
		pointsToTakeFromHealth = points;
	}
	public DamageValue(PhysicalDamageType physDamage, int points)
	{
		physicalDamage = physDamage;
		pointsToTakeFromHealth = points;
	}
}
