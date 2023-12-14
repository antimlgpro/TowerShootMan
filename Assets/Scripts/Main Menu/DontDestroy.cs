using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
	private static DontDestroy dontDestroyInstance;

    void Awake()
    {
		DontDestroyOnLoad(gameObject);

		if (dontDestroyInstance == null) {
			dontDestroyInstance = this;
		} else {
			DestroyObject(gameObject);
		}
    }
}
