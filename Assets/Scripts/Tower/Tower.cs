using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract	class Tower : MonoBehaviour
{
	void Start() {
		Initialize();
	}

	public virtual void Initialize() {
		StartCoroutine(TowerLoop());
	}

	public abstract void ToggleTower();

	protected abstract IEnumerator TowerLoop();
}
