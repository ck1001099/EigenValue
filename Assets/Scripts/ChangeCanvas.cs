using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCanvas : MonoBehaviour {
	public GameObject init;
	public GameObject final;

	public void changeCanvas(){
		init.SetActive (false);
		final.SetActive (true);
	}
}
