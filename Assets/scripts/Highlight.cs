using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Highlight : MonoBehaviour {

	//When the mouse hovers over the GameObject, it turns to this color (red)
	Color m_MouseOverColor = Color.blue;
	//This stores the GameObject’s original color
	Color m_OriginalColor;
	//Get the GameObject’s mesh renderer to access the GameObject’s material and color
	MeshRenderer m_Renderer;

	void Update()
	{
	}

	void Start()
	{
		//Fetch the mesh renderer component from the GameObject
		m_Renderer = GetComponentInChildren<MeshRenderer>();
		//Fetch the original color of the GameObject
		m_OriginalColor = m_Renderer.material.color;
	}

	void OnMouseOver()
	{
		m_Renderer.material.color = m_MouseOverColor;
		Debug.Log("Mouse is over GameObject.");
	}

	void OnMouseExit()
	{
		m_Renderer.material.color = m_OriginalColor;
	}
}
