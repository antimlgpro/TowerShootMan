using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Defense/New Tower")]
public class TowerSO : ScriptableObject
{
	public float damage;
	public float range;
	public float RPM;

	public int cost;
	public Sprite sprite;


	public enum TowerType
	{
		Projectile,
		AreaOfEffect
	}

	public TowerType towerType;

	public GameObject prefab;
}
