using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoughtItem : MonoBehaviour {

	public GameObject bagDisplay;

	public void OpenBag(){
		StartCoroutine (loadingBagItem ());
		bagDisplay.SetActive (true);
	}

	public void CloseBag(){
		bagDisplay.SetActive (false);
	}

	IEnumerator loadingBagItem(){

		string item = GameObject.Find ("GameController").GetComponent<GameController> ().boughtItem;
		string showtext = "";

		if (item != "[]") {
			string[] str1 = item.Split ('{');
			int length = str1.Length;
			for (int i = 1; i < length; i++) {
				str1 [i] = str1 [i].Substring (0, str1 [i].Length - 2);
				string[] str2 = str1 [i].Split (',');

				string[] str21 = str2 [0].Split (':');
				string[] str22 = str2 [1].Split (':');

				Dictionary<string,string> items = GameObject.Find ("GameController").GetComponent<NetworkConnectionShopList> ().items;

				showtext = showtext + items [str21 [1]] + "：" + str22[1] + "個\n";
			}
		} else {
			showtext = "無";
		}


		bagDisplay.transform.Find ("bought/Text").GetComponent<Text> ().text = showtext;

		yield return 0;
	}

}
