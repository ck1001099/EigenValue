using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Information : MonoBehaviour {
	public string userName;
	public string storyCode;
	public int points;
	public string boughtItem;

	public void UpdateInfo(){
		userName = PlayerPrefs.GetString ("username");
		storyCode = PlayerPrefs.GetString ("storycode");
		points = PlayerPrefs.GetInt ("points");
		boughtItem = PlayerPrefs.GetString ("boughtItem");
	}
}