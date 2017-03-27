using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeText : MonoBehaviour {

	public StoryText[] storyText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

[System.Serializable]
public class StoryText{
	public string ID;
	[TextArea(5,15)]
	public string story;
}
