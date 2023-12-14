using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingScreen : MonoBehaviour
{
	[Header("Hide UIManager")]
	[SerializeField] protected bool isVisible;
	[SerializeField] protected List<GameObject> objectsToHide;


	[Header("Loading stuff")]
	[SerializeField] private TextMeshProUGUI loadingText;
	private CanvasGroup canvasGroup;

	[SerializeField, Range(0f, 100f)] private float fadeDuration;

	private bool isLoading;
	private int index = 0;
	[SerializeField, Range(0, 10)] private int maxDots = 3;
	[SerializeField, Range(0f, 10f)] private float timeBetweenDot = 0.3f;

	[HideInInspector] public UnityEvent m_OnFadeLoadingScreen;
	[HideInInspector] public UnityEvent m_OnLoadLevel;

    void Start()
    {
		canvasGroup = GetComponent<CanvasGroup>();

		isLoading = false;

		m_OnFadeLoadingScreen ??= new();
		m_OnLoadLevel ??= new();

		m_OnFadeLoadingScreen.AddListener(OnFadeLoadingScreen);
		m_OnLoadLevel.AddListener(ActivateLoading);


		Reset();
    }

	void Reset() {
		Toggle(false);
		canvasGroup.alpha = 1;
	}

	// Singelton pattern stolen from https://gamedevbeginner.com/singletons-in-unity-the-right-way/
	public static LoadingScreen Instance { get; private set; }
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

	public void Toggle(bool value) {
		isVisible = value;

		foreach (GameObject toHide in objectsToHide) {
			toHide.SetActive(isVisible);
		}
	}

	private void ActivateLoading() {
		if (isLoading == true) return;


		Toggle(true);
		isLoading = true;

		StartCoroutine(DotLoop());
	}

	private void OnFadeLoadingScreen() {
		isLoading = false;
		StartCoroutine(FadeLoadingScreen(0f, fadeDuration));
	}

	IEnumerator DotLoop() {
		while (isLoading) {
			index += 1;
			string dots = new string('.', 1 + (index % maxDots));
			loadingText.text = string.Format("Loading{0}", dots);

			yield return new WaitForSeconds(timeBetweenDot);
		}
	}

	IEnumerator FadeLoadingScreen(float targetValue, float duration) {
        float startValue = canvasGroup.alpha;
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

		Reset();
    }
}
