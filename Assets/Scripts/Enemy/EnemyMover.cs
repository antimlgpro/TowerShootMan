using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	private List<Transform> nodes;
	int targetNodeIndex = 1;
	float speed = 0;

	bool stopMoving = false;
	public bool StopMoving => stopMoving;

	public void Initialize(EnemyObject enemyObject, float speedMultiplier)
	{
		speed = enemyObject.speed * speedMultiplier;
	}

	public void Stop()
	{
		stopMoving = true;
	}

	public void SetNodes(List<Transform> _nodes)
	{
		this.nodes = _nodes;
	}

	// Update is called once per frame
	void Update()
	{
		if (stopMoving) return;

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
			if (targetNodeIndex == nodes.Count) stopMoving = true;
		}
	}
}
