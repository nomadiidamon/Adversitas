using System;

public interface IDamageType;

public enum PhysicalDamageType : IDamageType { PIERCING, SLASHING, BLUNT, NULL}
public enum MagicalDamageType : IDamageType { FIRE, LAVA, METAL, EARTH, WATER, ICE, AIR, ELECTRICITY, NULL}

public class DamageValue
{
	IDamageType DamageType;
	int pointsToTakeFromHealth;

	public DamageValue(IDamageType physORmage, int points)
	{
		DamageType = physORmage;
		pointsToTakeFromHealth = points;
	}
}
