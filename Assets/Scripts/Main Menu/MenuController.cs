using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuController : MonoBehaviour
{
	// Names are selfexplanitory
	public UnityEvent<bool> ToggleMainMenu;
	public UnityEvent<bool> ToggleLevelSelection;


	public UnityEvent OnClickPlay;
	public UnityEvent OnClickSettings;

	// Invoked when camera started flipping.
	public UnityEvent OnCameraFlip;
	public bool isCameraFlipped;


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
    	OnClickPlay ??= new();
		OnClickSettings ??= new();
		OnCameraFlip ??= new();

		ToggleMainMenu ??= new();
		ToggleLevelSelection ??= new();


		OnCameraFlip.AddListener(CameraFlip);
    }

	void CameraFlip() {
		isCameraFlipped = !isCameraFlipped;

		if (isCameraFlipped == true) {
			ToggleMenu(false);
		} else {
			ToggleMenu(true);
		}
	}

	void ToggleMenu(bool value) {
		ToggleMainMenu.Invoke(value);
		ToggleLevelSelection.Invoke(!value);
	}
}
