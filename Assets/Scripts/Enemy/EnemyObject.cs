using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Defense/New enemy")]
public class EnemyObject : ScriptableObject
{
	public new string name;
	[Range(1f, 500f)]
	public float baseHealth = 1;
	[Range(1, 6)]
	public int armorTier = 1;

	[Range(1f, 100f)]
	public float speed = 1;


	public GameObject enemyPrefab;
}
