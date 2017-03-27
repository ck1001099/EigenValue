using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GameController : MonoBehaviour {

	public BottomBarButton[] bottomBarButton;

	public GameObject Story;

	public Vector3[] nodeStory;
	public int page;
	public StoryLine[] storyLine;

	private string function = "Story";
	public bool canSlideStory = false;

	public GameObject storyText;
	public GameObject storyDisplay;
	public GameObject storyReadFunction;
	public int storyTextPage;
	public int storyTextPageNum;

	public string userName;
	public int points;
	public string storyCode; //////////////// from Server
	public string boughtItem;

	public GameObject articles;

	public GameObject backgroundMask;

	public GameObject treasureDisplay;
	public GameObject shopDisplay;

	public Text coins;
	public Text timer;

	public string ID;

	// Use this for initialization
	void Start () {
		ChangeFunction (function);
		ID = function;
		canSlideStory = true;
		page = 1;
		ChangePage (page);
		storyTextPage = 0;
		// Load data
		userName = PlayerPrefs.GetString("username");
		storyCode = PlayerPrefs.GetString("storycode");
		points = PlayerPrefs.GetInt ("points");
		boughtItem = PlayerPrefs.GetString ("boughtItem");

		SetStoryActive ();
		canSlideStory = false;
		this.GetComponent<MessageManager> ().ShowMessage ("觀迎您！\n" + userName + "\n" + "請搜集通關密語以點亮星星\n" + "可從以點亮的星星觀看故事", false);
	}
	
	// Update is called once per frame
	void Update () {
		if (storyText != null) {
			storyDisplay.transform.Find("Text").GetComponent<TextMesh> ().text = storyText.GetComponent<nodeText> ().storyText [storyTextPage].story;
			storyReadFunction.transform.Find ("PreviousPage").gameObject.SetActive (storyTextPage!=0);
			storyReadFunction.transform.Find ("NextPage").gameObject.SetActive (storyTextPage != storyTextPageNum - 1);
		}

		coins.text = String.Format("{0:D5}", points);
		DateTime beforeDateTime = new DateTime(2017, 04, 07, 18, 00, 00);
		DateTime nowDateTime = DateTime.Now;
		TimeSpan timeSpan = beforeDateTime.Subtract(nowDateTime);
		timer.text = String.Format("{0:D2}Days\n{1:D2}hr{2:D2}m{3:D2}s", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
	}

	public void ChangeFunction(string ID){
		this.ID = ID;
		for (int i = 0; i < bottomBarButton.Length; i++) {
			if (bottomBarButton [i].ID != ID) {
				bottomBarButton [i].Off.gameObject.SetActive (true);
				bottomBarButton [i].On.gameObject.SetActive (false);
			} else {
				bottomBarButton [i].Off.gameObject.SetActive (false);
				bottomBarButton [i].On.gameObject.SetActive (true);
			}
		}
		if (ID != "Story") {
			CloseStroyPage ();
			canSlideStory = false;
			backgroundMask.gameObject.SetActive (true);
		} else {
			canSlideStory = true;
			backgroundMask.gameObject.SetActive (false);
		}
		if (ID != "Treasure") {
			treasureDisplay.gameObject.SetActive (false);
		} else {
			treasureDisplay.gameObject.SetActive (true);
		}

		if (ID != "Shop") {
			GameObject product = shopDisplay.transform.Find ("ProductList/Viewport/Content").gameObject;
			for (int i = product.transform.childCount - 1; i >= 0; i--) {
				Destroy (product.transform.GetChild (i).gameObject);
			}
			shopDisplay.gameObject.SetActive (false);
		} else {
			shopDisplay.gameObject.SetActive (true);
		}
		
	}

	public void handDirection(gDefine.Direction mDirection){
		//Debug.Log (mDirection);
		if (!canSlideStory)
			return;
		if (function == "Story"){
			if (mDirection == gDefine.Direction.Left) {
				if (page < 2) {
					storyLine [page].Off.gameObject.SetActive (true);
					storyLine [page].On.gameObject.SetActive (false);
					page++;
					storyLine [page].Off.gameObject.SetActive (false);
					storyLine [page].On.gameObject.SetActive (true);
				}
				Story.transform.DOMove (nodeStory [page], 0.5f);
			} else if (mDirection == gDefine.Direction.Right) {
				if (page > 0) {
					storyLine [page].Off.gameObject.SetActive (true);
					storyLine [page].On.gameObject.SetActive (false);
					page--;
					storyLine [page].Off.gameObject.SetActive (false);
					storyLine [page].On.gameObject.SetActive (true);
				}
				Story.transform.DOMove (nodeStory [page], 0.5f);
			}
		}
	}

	public void ChangePage(int pageEnd){
		if (!canSlideStory)
			return;
		storyLine [page].Off.gameObject.SetActive (true);
		storyLine [page].On.gameObject.SetActive (false);
		page = pageEnd;
		storyLine [page].Off.gameObject.SetActive (false);
		storyLine [page].On.gameObject.SetActive (true);
		Story.transform.DOMove (nodeStory [page], 0.5f);
	}

	public void NextStoryPage(){
		if (storyTextPage < storyTextPageNum - 1) {
			storyTextPage++;
		}
	}
	public void PreviousStoryPage(){
		if (storyTextPage > 0) {
			storyTextPage--;
		}
	}
	public void CloseStroyPage(){
		storyText = null;
		storyDisplay.SetActive (false);
		storyReadFunction.SetActive (false);
		storyTextPage = 0;
		storyTextPageNum = 0;
		canSlideStory = true;
	}

	public void SetStoryActive(){
		List<Node> nodes = new List<Node>();
		GameObject PLine = articles.transform.Find ("P-line").gameObject;
		GameObject YLine = articles.transform.Find ("Y-line").gameObject;
		GameObject HLine = articles.transform.Find ("H-line").gameObject;
		for (int i = 0; i < PLine.transform.childCount; i++)
			nodes.Add(PLine.transform.GetChild (i).GetComponent<Node>());
		for (int i = 0; i < YLine.transform.childCount; i++)
			nodes.Add(YLine.transform.GetChild (i).GetComponent<Node>());
		for (int i = 0; i < HLine.transform.childCount; i++)
			nodes.Add(HLine.transform.GetChild (i).GetComponent<Node>());
		for (int i = 0; i < nodes.Count; i++) {
			if (storyCode [i] == '0')
				nodes [i].isActive = false;
			else if (storyCode [i] == '1')
				nodes [i].isActive = true;
		}
	}

	public void UpdateInfo(){
		userName = PlayerPrefs.GetString ("username");
		storyCode = PlayerPrefs.GetString ("storycode");
		points = PlayerPrefs.GetInt ("points");
		boughtItem = PlayerPrefs.GetString ("boughtItem");
	}

}

[System.Serializable]
public class BottomBarButton{
	public string ID;
	public GameObject Off;
	public GameObject On;
}

[System.Serializable]
public class StoryLine{
	public string ID;
	public GameObject Off;
	public GameObject On;
}