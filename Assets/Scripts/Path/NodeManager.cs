using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NodeManager : MonoBehaviour
{
	public List<Transform> nodes = new();

	void OnValidate()
	{
		nodes = transform.Cast<Transform>().ToList();
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
}
