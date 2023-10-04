using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Defense/New tower")]
public class TowerObject : ScriptableObject
{
	public float damage;
	public float range;
	public float RPM;

	// Upgrades
	// TODO: Tier object for upgrades
}