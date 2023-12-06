using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract	class Tower : MonoBehaviour
{
	public abstract void ToggleTower(bool value);

	protected abstract IEnumerator TowerLoop();
}
