using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	private EnemySO enemyObject;

	private List<Vector3> nodes;
	private EnemySpawner m_Spawner;
	int targetNodeIndex = 1;
	float speed = 0;

	float fastForwardMultiplier = 0f;
	bool fastForward = false;

	bool isMoving = true;
	public bool IsMoving => isMoving;

	public void Initialize(EnemySO _enemyObject, EnemySpawner spawner)
	{
		enemyObject = _enemyObject;
		speed = _enemyObject.speed;
		nodes = NodeManager.Instance.smoothedNodes;
		m_Spawner = spawner;
		isMoving = true;

		fastForwardMultiplier = GameController.Instance.FastForwardMultiplier;
		fastForward = GameController.Instance.fastForward;
		GameController.Instance.m_OnWaveFastForward.AddListener(FastForward);
	}

	void FastForward(bool value) {
		fastForward = value;
	}

	public void Stop()
	{
		isMoving = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (!GameController.Instance.IsPlaying) return;
		if (isMoving == false) return;
		if (nodes == null) return;

		float _speed = speed;
		if (fastForward) {
			_speed *= fastForwardMultiplier;
		}
		float t = _speed * Time.deltaTime;

		transform.position = Vector3.MoveTowards(
				transform.position,
				nodes[targetNodeIndex],
				t
			);

		var dist = Vector3.Distance(transform.position, nodes[targetNodeIndex]);
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
