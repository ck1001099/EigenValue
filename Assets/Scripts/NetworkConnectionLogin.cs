using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

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
			string serverMessage = www.downloadHandler.text;
			string str1 = serverMessage.Substring (1, serverMessage.Length - 2);

			string[] str2 = str1.Split (',');
			int length = str2.Length;
			string[] str21 = str2 [0].Split (':');
			if (str21 [1] == "true") {
				//split ':'
				string[] str22 = str2 [1].Split (':');
				string[] str23 = str2 [2].Split (':');
				//string[] str24 = str2 [3].Split (':');
				string str25_s = "";
				for (int i = 4; i < length - 1; i++) {
					if (i != length - 2) {
						str25_s = str25_s + str2 [i] + ",";
					}
					else {
						str25_s = str25_s + str2 [i];
					}
				}
				string[] str25 = str25_s.Split ('[');
				str25 [0] = str25 [0].Substring (0, str25 [0].Length - 2);
				string[] str26 = str2 [length - 1].Split (':');

				int key1 = 1;
				str22[1] = str22[1].Insert (key1, "1");
				str22[1] = str22[1].Remove (key1 + 1, 1);
				int key2 = 13;
				str22[1] = str22[1].Insert (key2, "1");
				str22[1] = str22[1].Remove (key2 + 1, 1);
				int key3 = 25;
				str22[1] = str22[1].Insert (key3, "1");
				str22[1] = str22[1].Remove (key3 + 1, 1);

				//store information
				PlayerPrefs.SetInt("login", 1);
				PlayerPrefs.SetString("storycode", str22 [1].Substring (1, str22 [1].Length - 2));
				PlayerPrefs.SetInt ("points", System.Convert.ToInt32 (str23 [1]));
				PlayerPrefs.SetString ("username", str26 [1].Substring (1, str26 [1].Length - 2));
				PlayerPrefs.SetString ("boughtItem", str25 [1]);
				//Change Scene
				SceneManager.LoadScene("main");
			} else if (str21 [1] == "false") {
				string[] str22 = str2 [1].Split (':');
				if (str22 [1] == "\"使用者名稱或密碼有誤\"") {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("使用者名稱或密碼有誤！", false);
				} else {
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
			//Debug.Log(www.downloadHandler.text);
		}
	}
}
