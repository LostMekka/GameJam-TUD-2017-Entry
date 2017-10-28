using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class count : MonoBehaviour
{
	private GameObject gameController;

	private Text txt;

	private int c = 0;
	// Use this for initialization
	void Start () {
		// Set the remaining rounds
		txt = gameObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		txt.text = c.ToString();
		++c;
	}
}
