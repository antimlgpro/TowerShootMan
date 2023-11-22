using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoUIManager : UIManagerBase
{
	[Header("Data")]
	[SerializeField] private int waveValue;
	[SerializeField] private int waveMaxValue;
	[SerializeField] private int healthValue;
	[SerializeField] private int moneyValue;

	[Header("References")]
	[SerializeField] private TextMeshProUGUI waveText;
	[SerializeField] private TextMeshProUGUI healthText;
	[SerializeField] private TextMeshProUGUI moneyText;


	[Header("Default")]
	[SerializeField] private string currency;

    public override bool LoadUIManager()
    {
		UpdateWave(waveValue, waveMaxValue);
		UpdateHealth(healthValue);
		UpdateMoney(moneyValue);

		return true;
    }

	public void UpdateWave(int value, int max) {
		waveValue = value;
		waveMaxValue = max;

		// waves are indexed from zero but is displayed as one indexed.
		waveText.text = string.Format("{0}/{1}", waveValue + 1, waveMaxValue);
	}

	public void UpdateHealth(int value) {
		healthValue = value;

		healthText.text = value.ToString();
	}

	public void UpdateMoney(int value) {
		moneyValue = value;

		moneyText.text = string.Format("{0}{1}", currency, value);
	}
}
