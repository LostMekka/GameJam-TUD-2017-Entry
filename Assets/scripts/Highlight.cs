using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Highlight : MonoBehaviour
{
	private readonly Color mouseOverColor = Color.blue;
	private Color originalColor;
	private MeshRenderer meshRenderer;

	public void Start()
	{
		meshRenderer = GetComponentInChildren<MeshRenderer>();
		originalColor = meshRenderer.material.color;
	}

	public void OnMouseEnter() { meshRenderer.material.color = mouseOverColor; }

	public void OnMouseExit() { meshRenderer.material.color = originalColor; }
}