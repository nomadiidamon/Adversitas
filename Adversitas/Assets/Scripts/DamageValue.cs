using System;


//public enum PhysicalDamageType { PIERCING, SLASHING, BLUNT, NULL}
//public enum MagicalDamageType { FIRE, LAVA, METAL, EARTH, WATER, ICE, AIR, ELECTRICITY, NULL}
public enum DamageType { PHYSICAL_PIERCING, PHYSICAL_SLASHING, PHYSICAL_BLUNT, MAGIC_FIRE, MAGIC_LAVA, MAGIC_METAL, MAGIC_EARTH, MAGIC_WATER, MAGIC_ICE, MAGIC_AIR, MAGIC_ELECTRICITY, NULL }

[System.Serializable]
public class DamageValue
{
	DamageType damageType;
	int pointsToTakeFromHealth { get; }

	public DamageValue(DamageType type, int points)
	{
        damageType = type;
		pointsToTakeFromHealth = points;
	}
}
