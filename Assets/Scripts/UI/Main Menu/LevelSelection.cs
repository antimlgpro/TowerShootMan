using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
	private bool isActive;


	[SerializeField] private List<GameObject> levelPrefabs = new();
	[SerializeField] private List<string> scenes = new();
	private Dictionary<string, GameObject> sceneToLevel;


	private int currentSelectedIndex;
	private List<GameObject> spawnedLevels;

	[SerializeField] private GameObject levelParent;
	[SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
		if (levelPrefabs.Count != scenes.Count) return;

		sceneToLevel = new();

		for(int i = 0; i < levelPrefabs.Count; i++) {
			sceneToLevel.Add(scenes[i], levelPrefabs[i]);
		}

		spawnedLevels = new();

		for(int i = 0; i < levelPrefabs.Count; i++) {

			GameObject instance = Instantiate(levelPrefabs[i], Vector3.zero, Quaternion.identity);
			instance.transform.SetParent(levelParent.transform);
			instance.transform.localPosition = offset * i;

			spawnedLevels.Add(instance);
		}
    }

	void SelectLevel(int index) {
		currentSelectedIndex = index;

		MoveLevels(index);
	}

	void MoveLevels(int index) {
		levelParent.transform.position = -offset * index;
	}

    void Update()
    {
        
    }
}
