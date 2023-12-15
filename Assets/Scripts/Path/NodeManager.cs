using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeManager : MonoBehaviour
{
	public List<Transform> nodes = new();

	public bool Chaikin;
	public int Iterations;
	public List<Vector3> smoothedNodes = new();

	List<Vector3> ChaikinsIteration(List<Vector3> path, int iterations) {
		if (iterations == 0) return path;

		var output = new List<Vector3>();

        if (path.Count > 0)
        {
            output.Add(path[0]);
        }

        for (var i = 0; i < path.Count - 1; i++)
        {
            var p0 = path[i];
            var p1 = path[i + 1];
            var p0x = p0.x;
            var p0z = p0.z;
			var p0y = p0.y;
            var p1x = p1.x;
            var p1z = p1.z;
			var p1y = p1.y;

            var qx = 0.75f * p0x + 0.25f * p1x;
            var qy = 0.75f * p0y + 0.25f * p1y;
            var qz = 0.75f * p0z + 0.25f * p1z;
            var Q = new Vector3(qx, qy, qz);

            var rx = 0.25f * p0x + 0.75f * p1x;
            var ry = 0.25f * p0y + 0.75f * p1y;
            var rz = 0.25f * p0z + 0.75f * p1z;
            var R = new Vector3(rx, ry, rz);

            output.Add(Q);
            output.Add(R);
        }

        if (path.Count > 1)
        {
            output.Add(path[^1]);
        }

		if (iterations == 1) {
			return output;
		} else {
			return ChaikinsIteration(output, iterations - 1);
		}
	}

	void OnValidate()
	{
		nodes = transform.Cast<Transform>().ToList();

		if (Chaikin) {
			smoothedNodes = ChaikinsIteration(nodes.Select(x => x.position).ToList(), Iterations);
			Chaikin = false;
		}
	}

	// Singelton pattern stolen from https://gamedevbeginner.com/singletons-in-unity-the-right-way/
	public static NodeManager Instance { get; private set; }
	private void Awake()
	{
		// If there is an instance, and it's not me, delete myself.

		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

	void OnDrawGizmos()
    {
        if (smoothedNodes != null)
        {
			for (int i = 0; i < smoothedNodes.Count; i++)
			{
				if (i + 1 >= smoothedNodes.Count) return;

            	Gizmos.color = Color.blue;
				Gizmos.DrawLine(smoothedNodes[i], smoothedNodes[i + 1]);
			}
        }
    }
}
