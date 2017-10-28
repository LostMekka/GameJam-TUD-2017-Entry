using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{

	public GameObject mob;
	Vector3 offSet = new Vector3(0.0F, 0.7F, 0.0F);
	// Use this for initialization
	void Start ()
	{
		transform.position = mob.transform.position + offSet;
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = mob.transform.position + offSet;

	}
}
