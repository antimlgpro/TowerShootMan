using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract	class Tower : MonoBehaviour
{
	public TowerSO towerObject;

	public abstract void ToggleTower(bool value);
	public abstract void AddUpgrade(TowerUpgradeSO towerUpgradeSO);

	protected abstract IEnumerator TowerLoop();
}
