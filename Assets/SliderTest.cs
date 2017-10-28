using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderTest : MonoBehaviour {
	
	public Slider healthBarSlider;
	// Use this for initialization
	void Start ()
	{
		healthBarSlider.value = 1.0F;
	}
	
	// Update is called once per frame
	void Update ()
	{
		healthBarSlider.value -= 0.001F;
	}
}
