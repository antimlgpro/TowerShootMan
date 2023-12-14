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
		circleEnabledPosition = new Vector2(swatchTransform.rect.width, 0);
		titleText.text = text;
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
			StartCoroutine(MoveCircle(circleDefaultPosition, circleEnabledPosition));
		} else {
			StartCoroutine(MoveCircle(circleEnabledPosition, circleDefaultPosition));
		}
	}

	IEnumerator MoveCircle(Vector2 origin, Vector2 target) {
		isAnimating = true;
		while (elapsedTime < timeToMove) {
			circle.rectTransform.anchoredPosition = Vector2.Lerp(origin, target, elapsedTime / timeToMove);
			elapsedTime += Time.deltaTime;
      		yield return new WaitForEndOfFrame();
		}

		fill.enabled = isEnabled;
		isAnimating = false;
	}
}
