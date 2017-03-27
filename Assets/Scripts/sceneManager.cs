using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneManager : MonoBehaviour {

	public static sceneManager ins;

	void Awake(){

		if(ins == null){

			ins = this;
			GameObject.DontDestroyOnLoad(gameObject);

		}else if(ins != this){

			Destroy(gameObject);
		}
	}

	void Update(){
		if (Input.GetKey (KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home)) {
			Application.Quit ();
		}
	}

}
