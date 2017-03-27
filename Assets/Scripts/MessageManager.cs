using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour {

	public GameObject canvas;
	public GameObject messageDisplay;
	public GameObject messageMask;

	public void ShowMessage(string text, bool twoButton){
		canvas.gameObject.SetActive (true);
		messageDisplay.gameObject.SetActive (true);
		messageDisplay.transform.Find ("Text").GetComponent<Text> ().text = text;
		if (twoButton) {
			messageDisplay.transform.Find ("TwoButton").gameObject.SetActive (true);
		} else {
			messageDisplay.transform.Find ("OneButton").gameObject.SetActive (true);
		}
		if (messageMask != null)
			messageMask.gameObject.SetActive (true);
	}

	public void CloseMessage(){
		messageDisplay.transform.Find ("TwoButton").gameObject.SetActive (false);
		messageDisplay.transform.Find ("OneButton").gameObject.SetActive (false);
		messageDisplay.gameObject.SetActive (false);
		canvas.gameObject.SetActive (false);
		if (messageMask != null)
			messageMask.gameObject.SetActive (false);
		GameController obj = GameObject.Find ("GameController").GetComponent<GameController> ();
		if (obj!=null && obj.ID == "Story") {
			obj.canSlideStory = true;
		}
	}
}
