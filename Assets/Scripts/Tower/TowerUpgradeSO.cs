using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Defense/New Tower upgrade")]
public class TowerUpgradeSO : ScriptableObject
{
	public enum UpgradeType {
		Damage,
		Range,
		Speed,
	}

	public string upgradeName;
	public int cost;

	// This is the part that changes upgrade types value for a tower. Ie the value is increased by x times.
	public float effectMultiplier;

	public UpgradeType type;
}
