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
	private int lastIndex;
	private List<GameObject> spawnedLevels;

	[SerializeField] private GameObject levelParent;
	[SerializeField] private Vector3 offset;

	private float elapsedTime = 0f;
	[SerializeField, Range(0f, 100f)] private float timeToMove;
	private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
		if (levelPrefabs.Count != scenes.Count) return;

		elapsedTime = 0f;

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

		MenuController.Instance.m_OnLevelSwap.AddListener(SwapLevel);
		MenuController.Instance.m_OnClickSelectLevel.AddListener(SelectLevelButton);
    }

	void SwapLevel(bool value) {
		// true = left, false = right
		if (value == true) {
			SelectLevel(Mathf.Min(currentSelectedIndex + 1, spawnedLevels.Count - 1));
		} else {
			SelectLevel(Mathf.Max(0, currentSelectedIndex - 1));
		}
	}

	void SelectLevelButton() {
		MenuController.Instance.m_OnLevelSelect.Invoke(scenes[currentSelectedIndex]);
	}

	void SelectLevel(int index) {
		if (isMoving) return;
		if (index > spawnedLevels.Count - 1) return;

		lastIndex = currentSelectedIndex;
		currentSelectedIndex = index;

		MenuController.Instance.m_LevelSwapResult.Invoke(scenes[currentSelectedIndex]);

		MoveLevels(index);
	}

	void MoveLevels(int index) {
		int deltaIndex = index - lastIndex;
		lastPosition = levelParent.transform.position;
		target = -offset * deltaIndex + lastPosition;

		isMoving = true;
		StartCoroutine(MoveLevelsSmoothly());
	}

	private Vector3 lastPosition;
	private Vector3 target;
	IEnumerator MoveLevelsSmoothly() {
		while (elapsedTime < timeToMove) {
			levelParent.transform.position = Vector3.Lerp(lastPosition, target, InOut(elapsedTime / timeToMove));
			elapsedTime += Time.deltaTime;
      		yield return new WaitForEndOfFrame();
		}

		if(elapsedTime >= timeToMove) { 
			levelParent.transform.position = target;
			elapsedTime = 0f;
			isMoving = false;
		}
	}

	private float InOut(float t) {
		return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
	}
}
