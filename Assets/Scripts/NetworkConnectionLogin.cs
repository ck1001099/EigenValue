using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using LitJson;

public class NetworkConnectionLogin : MonoBehaviour {

	private const string url = "http://hnhn789.pythonanywhere.com/accounts/login/";

	public Text Username;
	public InputField Password;

	public void Connection(){
		StartCoroutine (Login ());
	}

	IEnumerator Login() {
		string username = Username.text;
		string password = Password.text;

		WWWForm form = new WWWForm();
		form.AddField ("username", username);
		form.AddField ("password", password);

		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.Send();
		
		if(www.isError) {
			GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		}
		else {
			JsonData jsonData = JsonMapper.ToObject (www.downloadHandler.text);

			string success = jsonData ["success"].ToString ();

			if (success == "True") {
				string storyCode = jsonData ["stories"].ToString ();
				string points = jsonData ["points"].ToString ();
				string boughtItem = jsonData ["boughtitems"].ToJson();
				string userName = jsonData ["username"].ToString ();

				int key1 = 0;
				storyCode = storyCode.Insert (key1, "1");
				storyCode = storyCode.Remove (key1 + 1, 1);
				int key2 = 12;
				storyCode = storyCode.Insert (key2, "1");
				storyCode = storyCode.Remove (key2 + 1, 1);
				int key3 = 24;
				storyCode = storyCode.Insert (key3, "1");
				storyCode = storyCode.Remove (key3 + 1, 1);

				//store information
				PlayerPrefs.SetInt("login", 1);
				PlayerPrefs.SetString("storycode", storyCode);
				PlayerPrefs.SetInt ("points", System.Convert.ToInt32 (points));
				PlayerPrefs.SetString ("username", userName);
				PlayerPrefs.SetString ("boughtItem", boughtItem);
				//Change Scene
				SceneManager.LoadScene("main");
			} else if (success == "False") {
				string message = jsonData ["messages"].ToString ();
				if (message == "使用者名稱或密碼有誤") {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("使用者名稱或密碼有誤！", false);
				} else if (message == "信箱尚未認證") {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("信箱尚未認證！", false);
				}
			}

			//"success":false,
			//"messages":"使用者名稱或密碼有誤"

			//{"success":false,
			//"messages":"信箱尚未認證"}

			//{"success":true,
			//"stories":"11101111111101011101101101111110101",
			//"points":99,
			//"messages":"登入成功",
			//"boughtitems":[{"item_name":6,"item_quantity":1},{"item_name":7,"item_quantity":1},{"item_name":2,"item_quantity":3}],
			//"username":"b03202042"}
			//Debug.Log(www.downloadHandler);
		}
	}
}
