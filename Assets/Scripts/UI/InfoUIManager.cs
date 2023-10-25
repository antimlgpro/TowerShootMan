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
	
	// TODO: implement health and money.
	//[SerializeField] private TextMeshProUGUI healthText;
	//[SerializeField] private TextMeshProUGUI moneyText;


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

		waveText.text = string.Format("{0}/{1}", waveValue, waveMaxValue);
	}

	public void UpdateHealth(int value) {
		healthValue = value;
	}

	public void UpdateMoney(int value) {
		moneyValue = value;
	}
}
