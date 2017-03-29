using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

public class NetworkConnectionQRcode : MonoBehaviour {

	private const string url = "http://hnhn789.pythonanywhere.com/QRcode/";

	public Text QRcode;
	private string username;

	private List<string> error_message = new List<string>(){
		"一人做事一人當\n小叮做事小叮噹",
		"水能載舟\n亦能煮粥",
		"五湖四海皆兄弟\n永和四海皆豆漿",
		"小橋流水人家\n古道梅子綠茶",
		"三十而立\n四十大盜",
		"東邊日出西邊雨\n南極企鵝北極熊",
		"兩岸猿聲啼不住\n主席已換洪秀柱",
		"有朋自遠方來\n非奸即盜",
		"垂死病中驚坐起\n想到衣服還沒洗"};

	public void Connection(){
		username = GameObject.Find ("GameController").GetComponent<GameController> ().userName;
		StartCoroutine (QRCode ());
	}

	IEnumerator QRCode() {

		string qrcode = QRcode.text;

		if (qrcode == "") {
			GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("好歹填個東西吧XD", false);
			yield break;
		}

		string newURL = url + username + "/" + qrcode + "/";

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
				string points = jsonData ["point_received"].ToString ();
				//store information
				int get_points = System.Convert.ToInt32 (points);
				GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("恭喜獲得 " + get_points.ToString () + " points\n請繼續努力唷>w<", false);
				PlayerPrefs.SetInt ("points", PlayerPrefs.GetInt ("points") + get_points);
				string storyCode = PlayerPrefs.GetString ("storycode");
				int key = Random.Range (0, 34);
				storyCode = storyCode.Insert (key, "1");
				storyCode = storyCode.Remove (key + 1, 1);
				PlayerPrefs.SetString ("storycode", storyCode);
				GameObject.Find ("GameController").GetComponent<GameController> ().UpdateInfo ();
				GameObject.Find ("GameController").GetComponent<GameController> ().SetStoryActive ();
			} else if (success == "False") {
				string message = jsonData ["messages"].ToString ();
				if (message == "QRcode不存在") {
					int count = error_message.Count;
					int num = Random.Range (0, count - 1);
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage (error_message [num], false);
				} else {
					GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("4小時內只能用一次唷~~\n找點別的吧", false);
				}
			}

			//{"success":false,
			//"messages":"QRcode不存在"}

			//{"success":false,
			//"time":"95120.251597",
			//"messages":"此QRcode冷卻中，還不能使用...剩餘時間：1小時34分39秒"}

			//{"success":true,
			//"point_received":"11",
			//"time":"2017-03-26T02:22:05.254126Z",
			//"messages":"成功得到點數！"}

			//Debug.Log (www.downloadHandler.text);
		}
	}
}
