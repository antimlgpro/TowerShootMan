using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	private EnemySO enemyObject;

	private List<Transform> nodes;
	private EnemySpawner m_Spawner;
	int targetNodeIndex = 1;
	float speed = 0;

	bool isMoving = true;
	public bool IsMoving => isMoving;

	public void Initialize(EnemySO _enemyObject, EnemySpawner spawner)
	{
		enemyObject = _enemyObject;
		speed = _enemyObject.speed;
		nodes = NodeManager.Instance.nodes;
		m_Spawner = spawner;
		isMoving = true;
	}

	public void Stop()
	{
		isMoving = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (isMoving == false) return;

		if (nodes == null) return;

		float t = speed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(
				transform.position,
				nodes[targetNodeIndex].position,
				t
			);

		var dist = Vector3.Distance(transform.position, nodes[targetNodeIndex].position);
		if (dist <= 0.5)
		{
			targetNodeIndex += 1;

			// stop moving when we hit the last node
			if (targetNodeIndex == nodes.Count) {
				isMoving = false;
				m_Spawner.OnEnemyFinished.Invoke(enemyObject);
			};
		}
	}
}
