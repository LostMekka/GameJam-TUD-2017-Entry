using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public int unitHealth = 100;
    public int unitStamina = 100;
    public Object tileInfo;
    public int unitSize = 1;
    public string unitAction = "idle";
    public Dictionary<string, string> skillList = new Dictionary<string, string>();

	// Use this for initialization
	void Start () {

        // Add some elements to the dictionary.
        skillList.Add("txt", "notepad.exe");
        skillList.Add("bmp", "paint.exe");
        skillList.Add("dib", "paint.exe");
        skillList.Add("rtf", "wordpad.exe");

    }
	
	// Update is called once per frame
	void Update () {



    }
}
