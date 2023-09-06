using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	private EnemyMover mover;

	private EnemyObject enemyObject;

	[SerializeField]
	private float health;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float armorTier;

	public void SetEnemyObject(EnemyObject _enemyObject)
	{
		enemyObject = _enemyObject;
		health = enemyObject.baseHealth;
		speed = enemyObject.speed;
		armorTier = enemyObject.armorTier;
	}

	public void TakeDamage(float damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Die();
		}
	}

	public void Die()
	{
		print("I died");
		mover.Stop();
	}
}
