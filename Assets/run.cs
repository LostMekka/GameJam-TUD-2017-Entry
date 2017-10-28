using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class run : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3( 0.005F, 0.005F, 0.005F);
	}
}
