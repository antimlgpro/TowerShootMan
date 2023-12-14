using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
	// Names are selfexplanitory
	[HideInInspector] public UnityEvent<bool> m_ToggleLevelSelection;
	[HideInInspector] public UnityEvent<bool> m_ToggleMainMenu;


	[HideInInspector] public UnityEvent m_OnClickPlay;
	[HideInInspector] public UnityEvent m_OnClickSettings;

	// Invoked when camera started flipping.
	[HideInInspector] public UnityEvent m_OnCameraFlip;
	[HideInInspector] public bool m_isCameraFlipped;


	// Level selection
	[HideInInspector] public UnityEvent<bool> m_OnLevelSwap;
	[HideInInspector] public UnityEvent m_OnClickSelectLevel;
	[HideInInspector] public UnityEvent<string> m_OnLevelSelect;

	[Range(0f, 100f)] public float artificialLoadingDelay;


	// Singelton pattern stolen from https://gamedevbeginner.com/singletons-in-unity-the-right-way/
	public static MenuController Instance { get; private set; }
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

    void Start()
    {
    	m_OnClickPlay ??= new();
		m_OnClickSettings ??= new();
		m_OnCameraFlip ??= new();

		m_ToggleMainMenu ??= new();
		m_ToggleLevelSelection ??= new();

		m_OnLevelSwap ??= new();
		m_OnClickSelectLevel ??= new();
		m_OnLevelSelect ??= new();

		m_OnCameraFlip.AddListener(CameraFlip);
		m_OnLevelSelect.AddListener(LoadLevel);
    }

	void CameraFlip() {
		m_isCameraFlipped = !m_isCameraFlipped;

		if (m_isCameraFlipped == true) {
			ToggleMenu(false);
		} else {
			ToggleMenu(true);
		}
	}

	void ToggleMenu(bool value) {
		m_ToggleMainMenu.Invoke(value);
		m_ToggleLevelSelection.Invoke(!value);
	}

	void LoadLevel(string sceneName) {
		LoadingScreen.Instance.m_OnLoadLevel.Invoke();
		StartCoroutine(LoadLevelAsync(sceneName));
	}

	IEnumerator LoadLevelAsync(string sceneToLoad) {
		yield return new WaitForSeconds(artificialLoadingDelay);

		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

		operation.completed += (_) => {
			LoadingScreen.Instance.m_OnFadeLoadingScreen.Invoke();
		};

		while (!operation.isDone)
        {
            yield return null;
        }
	}
}
