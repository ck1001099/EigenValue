using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	private RectTransform rectTransform;

	public float time = 0f;

	// Use this for initialization
	void Start () {
		rectTransform = this.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		time = time + Time.fixedDeltaTime;
		if ((int)(time % 2) == 0)
			rectTransform.transform.Rotate (new Vector3 (0f, 0f, 3.6f));
	}
}
