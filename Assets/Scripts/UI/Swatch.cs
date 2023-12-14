using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Mathematics;

public class Swatch : MonoBehaviour
{
	private bool isEnabled;
	private bool isAnimating;

	[SerializeField] private string text;
	[SerializeField,Range(0f, 100f)] private float timeToMove = 0.1f;

	[Header("References")]
	[SerializeField] private Button swatchButton;
	[SerializeField] private RectTransform swatchTransform;
	[SerializeField] private Image circle;
	[SerializeField] private Image fill;
	[SerializeField] private TextMeshProUGUI titleText;


	private Vector2 circleDefaultPosition;
	private Vector2 circleEnabledPosition;
	private float elapsedTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
		circleDefaultPosition = new Vector2(0, 0);
		circleEnabledPosition = new Vector2(swatchTransform.rect.width - circle.rectTransform.rect.width, 0);
		titleText.text = text;
		fill.enabled = true;
    }

	void Start() {		
		isAnimating = false;
		elapsedTime = 0f;

		// TODO: These should connect to a scriptable object or playerprefs.
		isEnabled = false;
        swatchButton.onClick.AddListener(OnClick);
	}

	void OnClick() {
		isEnabled = !isEnabled;

		if (isAnimating) return;
		elapsedTime = 0f;
		if (isEnabled) {
			StartCoroutine(MoveCircle(circleDefaultPosition, circleEnabledPosition, new Vector2(0f, 0f), new Vector2(swatchTransform.rect.width, 0f)));
		} else {
			StartCoroutine(MoveCircle(circleEnabledPosition, circleDefaultPosition, new Vector2(swatchTransform.rect.width, 0f), new Vector2(0f, 0f)));
		}
	}

	IEnumerator MoveCircle(Vector2 origin, Vector2 target, Vector2 widthOrigin, Vector2 widthTarget) {
		isAnimating = true;
		while (elapsedTime < timeToMove) {
			circle.rectTransform.anchoredPosition = Vector2.Lerp(origin, target, elapsedTime / timeToMove);
			fill.rectTransform.sizeDelta = Vector2.Lerp(widthOrigin, widthTarget, elapsedTime / timeToMove);
			elapsedTime += Time.deltaTime;
      		yield return new WaitForEndOfFrame();
		}

		isAnimating = false;
	}
}
