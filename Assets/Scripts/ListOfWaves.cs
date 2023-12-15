using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CreateAssetMenu(menuName = "Tower Defense/New Wave List")]
public class ListOfWaves : ScriptableObject
{
	public List<Wave> waves = new();
}
