using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaterialInChildren : MonoBehaviour
{
	[SerializeField] private List<MeshRenderer> meshRenderers = new();

	public void SetMaterial(Material material) {
		foreach (var renderer in meshRenderers) {
			renderer.material = material;
		}
	}
}
