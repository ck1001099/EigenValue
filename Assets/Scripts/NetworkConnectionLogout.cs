using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using LitJson;

public class NetworkConnectionLogout : MonoBehaviour {
	private const string url = "http://hnhn789.pythonanywhere.com/accounts/logout/";

	private string username;
	private string points;
	private string storycode;

	public void Connection(){
		username = GameObject.Find ("GameController").GetComponent<GameController> ().userName;
		points = GameObject.Find ("GameController").GetComponent<GameController> ().points.ToString ();
		storycode = GameObject.Find ("GameController").GetComponent<GameController> ().storyCode;
		StartCoroutine (Logout ());
	}

	IEnumerator Logout() {
		
		WWWForm form = new WWWForm();
		form.AddField ("username", username);
		form.AddField ("points", points);
		form.AddField ("stories", storycode);

		UnityWebRequest www = UnityWebRequest.Post(url, form);
		yield return www.Send();

		if(www.isError) {
			GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		}
		else {
			JsonData jsonData = JsonMapper.ToObject (www.downloadHandler.text);
			if (jsonData["success"].ToString() == "True") {
				PlayerPrefs.SetInt ("login", 0);
				PlayerPrefs.SetString ("username", "");
				PlayerPrefs.SetString ("storycode", "");
				PlayerPrefs.SetInt ("points", 0);
				PlayerPrefs.SetString ("boughtItem", "");
				SceneManager.LoadScene ("preMain");
			}


			//{"success":true,"messages":"登出成功！"}

			//Debug.Log(www.downloadHandler.text);
		}
	}

	public void logoutCheck(){
		GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("確定登出？", true);
		GameObject.Find ("CanvasM/Message/TwoButton/YES").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("CanvasM/Message/TwoButton/YES").GetComponent<Button> ().onClick.AddListener (Connection);
	}

}