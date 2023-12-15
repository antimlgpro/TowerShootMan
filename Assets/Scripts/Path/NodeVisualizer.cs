using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisualizer : MonoBehaviour
{

	[SerializeField, Range(0f, 1f)] private float delay;
	[SerializeField, Range(0f, 5f)] private float resetDelay;
	[SerializeField, Range(0.1f, 100f)] private float speed;
	[SerializeField] private GameObject trailObject;

	private List<Vector3> nodes;

	private bool pause;
	private int targetNodeIndex;

    // Start is called before the first frame update
    void Start()
    {
        nodes = NodeManager.Instance.smoothedNodes;

		trailObject.SetActive(false);
		targetNodeIndex = 0;
		trailObject.transform.position = nodes[targetNodeIndex];
		trailObject.SetActive(true);

		GameController.Instance.m_OnWaveStart.AddListener(OnWaveStart);
		GameController.Instance.m_OnWaveStop.AddListener(OnWaveStop);

		StartCoroutine(ParticleLoop());
    }

	void OnWaveStart() {
		pause = true;
	}

	void OnWaveStop() {
		pause = true;
	}


	IEnumerator ParticleLoop() {
		while (true) {
			// I do NOT want this nesting but its required AFAIK.
			if (pause == true) {
				yield return null;
			} else {
				float t = speed * Time.deltaTime;

				trailObject.transform.position = Vector3.MoveTowards(
					trailObject.transform.position,
					nodes[targetNodeIndex],
					t
				);

				var dist = Vector3.Distance(trailObject.transform.position, nodes[targetNodeIndex]);
				if (dist <= 0.5)
				{
					targetNodeIndex += 1;

					// stop moving when we hit the last node
					if (targetNodeIndex == nodes.Count) {
						trailObject.SetActive(false);
						yield return new WaitForSeconds(resetDelay - delay);
						targetNodeIndex = 0;
						trailObject.transform.position = nodes[targetNodeIndex];
						trailObject.SetActive(true);
					};
				}

				yield return new WaitForSeconds(delay);
			}
		}
	}
}
