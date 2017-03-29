using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

public class NetworkConnectionBuy : MonoBehaviour {

	private const string url = "http://hnhn789.pythonanywhere.com/shop/";

	public string ID;
	private string username;

	public string productname;
	public int points;

	public void Connection(){
		username = GameObject.Find ("GameController").GetComponent<GameController> ().userName;
		StartCoroutine (Buy ());
	}

	IEnumerator Buy() {

		string newURL = url + username + "/" + ID + "/";

		UnityWebRequest www = UnityWebRequest.Get(newURL);
		yield return www.Send();

		if(www.isError) {
			GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		}
		else {
			JsonData jsonData = JsonMapper.ToObject (www.downloadHandler.text);

			string success = jsonData ["success"].ToString ();

			if (success == "True") {
				PlayerPrefs.SetInt ("points", PlayerPrefs.GetInt ("points") - points);
				GameObject product = GameObject.Find ("Canvas/Shop/ProductList/Viewport/Content").gameObject;
				for (int i = product.transform.childCount - 1; i >= 0; i--) {
					Destroy (product.transform.GetChild (i).gameObject);
				}
				GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("購買成功！", false);
				GameObject.Find ("GameController").GetComponent<NetworkConnectionShopList> ().Connection ();
				string item = PlayerPrefs.GetString ("boughtItem");
				if (item.Contains ("\"item_name\":" + ID)) {
					int index1 = item.IndexOf ("\"item_name\":" + ID);
					int index2 = index1 + ("\"item_name\":" + ID + ",\"item_quantity\":").Length;
					int num = System.Convert.ToInt32 (item [index2]) - 48;
					num = num + 1;
					item = item.Insert (index2, num.ToString());
					item = item.Remove (index2 + 1, 1);
				} else {
					item = item.Insert (item.Length - 1, ",{\"item_name\":" + ID + "," + "\"item_quantity\":1}");
				}
				PlayerPrefs.SetString ("boughtItem", item);
				GameObject.Find ("GameController").GetComponent<GameController> ().UpdateInfo ();
			} else if (success == "False") {

				string message = jsonData ["messages"].ToString ();

				if (message == "很抱歉！您的點數不足！") {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("很抱歉！\n您的點數不足！", false);
				} else {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("很抱歉！\n您已超過此項目購買上限！\n請選購其他商品！", false);
				}
			}

			//{"success":false,
			//"messages":"很抱歉！您的點數不足！"}

			//{"success":false,
			//"messages":"很抱歉！您已超過此項目購買上限！請選購其他商品。"}

			//{"success":true,
			//"time":"2017-03-26T06:00:19.848858Z",
			//"messages":"購買成功！"}

		}
	}

	public void ShowMessage(){
		GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("確定購買以下商品？\n\n" + productname, true);
		GameObject.Find ("CanvasM/Message/TwoButton/YES").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("CanvasM/Message/TwoButton/YES").GetComponent<Button> ().onClick.AddListener (Connection);
	}
}
