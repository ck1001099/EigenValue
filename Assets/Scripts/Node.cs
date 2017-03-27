using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	public GameController gameController;

	public bool isActive;
	public SpriteRenderer sprite;
	public Sprite gray;
	public Sprite cyan;

	// Use this for initialization
	void Start () {
		sprite = this.GetComponent<SpriteRenderer> ();
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (isActive)
			sprite.sprite = cyan;
		else
			sprite.sprite = gray;
		
	}

	public void Click(){
		if (!isActive)
			return;
		gameController.storyText = this.gameObject;
		gameController.storyDisplay.SetActive (true);
		gameController.storyReadFunction.SetActive (true);
		gameController.storyTextPage = 0;
		gameController.storyTextPageNum = this.GetComponent<nodeText> ().storyText.Length;
		gameController.canSlideStory = false;
	}
}
