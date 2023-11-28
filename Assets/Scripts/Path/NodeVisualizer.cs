using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeVisualizer : MonoBehaviour
{

	[SerializeField, Range(0f, 1f)] private float delay;
	[SerializeField, Range(0f, 5f)] private float resetDelay;
	[SerializeField, Range(0.1f, 100f)] private float speed;
	[SerializeField] private GameObject particleSystem;

	private List<Transform> nodes;

	private bool pause;
	private int targetNodeIndex;

    // Start is called before the first frame update
    void Start()
    {
        nodes = NodeManager.Instance.nodes;

		particleSystem.SetActive(false);
		targetNodeIndex = 0;
		particleSystem.transform.position = nodes[targetNodeIndex].position;
		particleSystem.SetActive(true);

		StartCoroutine(ParticleLoop());
    }


	IEnumerator ParticleLoop() {
		while (true) {
			if (pause == true) {
				yield return null;
			}

			float t = speed * Time.deltaTime;

			particleSystem.transform.position = Vector3.MoveTowards(
				particleSystem.transform.position,
				nodes[targetNodeIndex].position,
				t
			);

			var dist = Vector3.Distance(particleSystem.transform.position, nodes[targetNodeIndex].position);
			if (dist <= 0.5)
			{
				targetNodeIndex += 1;

				// stop moving when we hit the last node
				if (targetNodeIndex == nodes.Count) {
					particleSystem.SetActive(false);
					yield return new WaitForSeconds(resetDelay - delay);
					targetNodeIndex = 0;
					particleSystem.transform.position = nodes[targetNodeIndex].position;
					particleSystem.SetActive(true);
				};
			}

			yield return new WaitForSeconds(delay);
		}
	}
}
