using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nodeConnection : MonoBehaviour {

	private LineRenderer lineRenderer;
	private Transform trans;
	private List<Vector3> points;

	// Use this for initialization
	void Start () {
		lineRenderer = this.GetComponent<LineRenderer> ();
		trans = this.transform;
		points = new List<Vector3> ();
		for (int i = 0; i < trans.childCount; i++) {
			Vector3 pos = trans.GetChild (i).transform.localPosition;
			pos.z = pos.z + 0.01f;
			points.Add (pos);
		}
		lineRenderer.numPositions = trans.childCount;
		lineRenderer.SetPositions (points.ToArray ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
