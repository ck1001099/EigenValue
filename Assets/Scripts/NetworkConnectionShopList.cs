using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using LitJson;

public class NetworkConnectionShopList : MonoBehaviour {

	private const string url = "http://hnhn789.pythonanywhere.com/shop/update/";

	public Dictionary<string,string> items = new Dictionary<string, string>();

	public GameObject content;
	public GameObject product;

	public void Connection(){
		items.Clear ();
		StartCoroutine (ShopList ());
	}

	IEnumerator ShopList() {
		
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		if(www.isError) {
			GameObject.Find ("GameController").GetComponent<MessageManager> ().ShowMessage ("網路不穩定\n請重新確認網路連線！", false);
			//Debug.Log(www.error);
		}
		else {
			JsonData jsonData = JsonMapper.ToObject (www.downloadHandler.text);

			for (int i = 0; i < jsonData.Count; i++) {
				GameObject obj = GameObject.Instantiate (product);
				obj.transform.SetParent (content.transform);
				obj.transform.localScale = new Vector3 (1, 1, 1);

				string ID = jsonData [i] ["pk"].ToString ();
				string productname = jsonData [i] ["name"].ToString ();
				string price = jsonData [i] ["price"].ToString ();
				string remain = jsonData [i] ["remain"].ToString ();
				string imageurl = jsonData [i] ["image"].ToString ();

				//GameObject.Find ("Canvas/Message/TwoButton/YES").GetComponent<Button> ().onClick.AddListener (/*function*/);
				obj.AddComponent<NetworkConnectionBuy> ();
				obj.GetComponent<NetworkConnectionBuy> ().ID = ID;
				obj.GetComponent<NetworkConnectionBuy> ().productname = productname;
				obj.GetComponent<NetworkConnectionBuy> ().points = System.Convert.ToInt32 (price);
				obj.transform.FindChild ("Buy").GetComponent<Button> ().onClick.AddListener (obj.GetComponent<NetworkConnectionBuy> ().ShowMessage);

				obj.transform.Find ("Info").GetComponent<Text> ().text = productname + "\n" + "$ " + price + "P\n" + "剩餘數目：" + remain;
				StartCoroutine (downloadImage (imageurl, obj));

				items.Add (ID, productname);
			}

			//[{"pk":1,"name":"一抽獎卷(Samsonite電腦包)","price":50,"remain":999,"image":"http://i63.tinypic.com/scwg45.jpg"},
			//{"pk":2,"name":"一恩可廚坊-歐式核桃酥(2片裝)","price":180,"remain":37,"image":"http://i66.tinypic.com/wi33k.jpg"},
			//{"pk":3,"name":"Café Chat Leopard 豹紋喵喵-藍鵲茶薩布蕾","price":400,"remain":14,"image":"http://i67.tinypic.com/s1l3lv.jpg"},
			//{"pk":4,"name":"果昂甜品 illuminé牛奶糖蛋糕捲","price":400,"remain":10,"image":"http://i68.tinypic.com/6ig2ag.jpg"},
			//{"pk":5,"name":"咖芳工作室-雪球禮盒","price":650,"remain":9,"image":"http://i66.tinypic.com/33d9g5e.jpg"},
			//{"pk":6,"name":"布列德麵包店-1/4片蛋糕","price":450,"remain":6,"image":"http://i66.tinypic.com/1zn3fb8.jpg"},
			//{"pk":7,"name":"咖芳工作室-水果大福兩個裝","price":300,"remain":11,"image":"http://i64.tinypic.com/28vbsid.jpg"},
			//{"pk":8,"name":"努力手作 Lorak's Handmade資料夾","price":400,"remain":6,"image":"http://i65.tinypic.com/347tqma.jpg"},
			//{"pk":9,"name":"努力手作 Lorak's Handmade徽章+右手安培貼紙","price":400,"remain":4,"image":"http://i67.tinypic.com/6yi8nl.jpg"},
			//{"pk":10,"name":"努力手作 Lorak's Handmade明信片","price":400,"remain":2,"image":"http://i63.tinypic.com/jtb5ed.jpg"},
			//{"pk":12,"name":"一楊媽媽菓子工坊-金饌鳳梨酥兩個裝","price":300,"remain":15,"image":"http://www.wu2.com.tw/yangmama/other/10/138536529.jpg"}]

			//Debug.Log (www.downloadHandler.text);
		}
	}

	IEnumerator downloadImage(string url, GameObject obj){
		// Start a download of the given URL
		WWW www = new WWW(url);

		// Wait for download to complete
		yield return www;

		obj.transform.Find ("picture").GetComponent<CanvasRenderer> ().SetTexture (www.texture);

	}

}
