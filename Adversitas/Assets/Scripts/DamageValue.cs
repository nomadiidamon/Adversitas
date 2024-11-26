using System;
using UnityEngine;


//public enum PhysicalDamageType { PIERCING, SLASHING, BLUNT, NULL}
//public enum MagicalDamageType { FIRE, LAVA, METAL, EARTH, WATER, ICE, AIR, ELECTRICITY, NULL}
[System.Serializable]
public enum DamageType { PHYSICAL_PIERCING, PHYSICAL_SLASHING, PHYSICAL_BLUNT, MAGIC_FIRE, MAGIC_LAVA, MAGIC_METAL, MAGIC_EARTH, MAGIC_WATER, MAGIC_ICE, MAGIC_AIR, MAGIC_ELECTRICITY, NULL }

[System.Serializable]
[SerializeField]
public class DamageValue
{
	[SerializeField] public DamageType damageType;
	[SerializeField] public int pointsToTakeFromHealth;

	public DamageValue(int points, DamageType type = DamageType.PHYSICAL_BLUNT)
	{
        damageType = type;
		pointsToTakeFromHealth = points;
	}

}
