using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public int unitHealth = 100;
    public int unitStamina = 100;
    public TileInfo CurrentTile;
    public int unitSize = 1;
    public string unitAction = "idle";
    public Dictionary<string, string> skillList = new Dictionary<string, string>();

	// Use this for initialization
	void Start () {



    }
	
	// Update is called once per frame
	void Update () {



    }
}
