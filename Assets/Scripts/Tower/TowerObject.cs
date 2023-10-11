using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Defense/New Tower")]
public class TowerObject : ScriptableObject
{
	public float damage;
	public float range;
	public float RPM;

	// Upgrades
	// TODO: Tier object for upgrades


	public enum TowerType
	{
		Projectile,
		AreaOfEffect
	}

	public TowerType towerType;
}
