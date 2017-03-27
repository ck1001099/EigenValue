using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR || UNITY_STANDALONE
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Debug.Log(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)){
				if (hit.transform.tag == "storyNode")
					hit.transform.GetComponent<Node> ().Click ();
				
				//Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red, 0.1f, true);
				//Debug.Log(hit.transform.name);
			}
		}
		#endif
	}

	public void Click(Vector2 touchPosition){
		Vector3 pos = new Vector3 (touchPosition.x, touchPosition.y, 0.0f);
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)){
			if (hit.transform.tag == "storyNode") {
				hit.transform.GetComponent<Node> ().Click ();
			}
			//Debug.DrawLine(Camera.main.transform.position, hit.transform.position, Color.red, 0.1f, true);
			//Debug.Log(hit.transform.name);
		}
	}
}
