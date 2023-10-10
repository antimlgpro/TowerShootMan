using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocity : MonoBehaviour
{
    public Vector3 velocity;
	private Vector3 previousPosition;
    void Update()
    {
        velocity = (transform.position - previousPosition) / Time.deltaTime;
		previousPosition = transform.position;
    }
}
