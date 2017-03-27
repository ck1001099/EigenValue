using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class NetworkConnectionRegist : MonoBehaviour {

	private const string url = "http://hnhn789.pythonanywhere.com/accounts/signup/";

	public Text Username;
	public InputField Password;
	public InputField CheckPassword;
	public Text Realname;
	public Text Department;

	public void Connection(){
		if (Password.text != CheckPassword.text) {
			GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("請重新確認密碼是否正確！", false);
			return;
		}
		else
			StartCoroutine (Regist ());
	}

	IEnumerator Regist() {
		string username = Username.text;
		string password = Password.text;
		string realname = Realname.text;
		string department = Department.text;

		WWWForm form = new WWWForm();
		form.AddField ("username", username);
		form.AddField ("password", password);
		form.AddField ("realname", realname);
		form.AddField ("department", department);

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
			string[] str21 = str2 [0].Split (':');
			string[] str22 = str2 [1].Split (':');
			str22 [1] = str22 [1].Substring (1, str22 [1].Length - 2);
			if (str21 [1] == "false") {
				if (str22 [1] == "註冊資料不完全") {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("註冊資料不完全！", false);
				} else if (str22 [1] == "此信箱已被註冊過") {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("此信箱已被註冊過！", false);
				}
			} else if (str21 [1] == "true") {
				GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("認證信已寄出！\n請至學校信箱確認！", false);
			}



			//{"success":true,"messages":"認證信已寄出！請至學校信箱確認！"}
			//{"success":false,"messages":"註冊資料不完全"}
			//{"success":false,"messages":"此信箱已被註冊過"}
		}
	}
}
