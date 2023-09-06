using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tower Defense/New Wave")]
public class Wave : ScriptableObject
{
	[System.Serializable]
	public class EnemyContainer
	{
		public EnemyObject enemyObject;

		[Range(1, 500)]
		public int amount;

		[Range(1, 100)]
		public float speedMultiplier;
	}

	public List<EnemyContainer> enemyContainers;
}
