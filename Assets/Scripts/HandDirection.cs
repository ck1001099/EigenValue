using UnityEngine;
using System.Collections;

public class gDefine {
	public enum Direction{
		Up,
		Down,
		Left,
		Right,
		None
	}
}

public class HandDirection : MonoBehaviour {

	//紀錄手指觸碰位置
	private Vector2 m_screenPos = new Vector2 ();

	void Update () {
		#if UNITY_EDITOR || UNITY_STANDALONE
		MouseInput();   // 滑鼠偵測
		#elif UNITY_ANDROID || UNITY_IOS
		MobileInput();  // 觸碰偵測
		#endif
	}

	void MouseInput(){
		if (Input.GetMouseButtonDown(0)){
			m_screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		}
		if(Input.GetMouseButtonUp(0)){
			Vector2 pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			gDefine.Direction mDirection = handDirection(m_screenPos, pos);
			//Debug.Log("mDirection: " + mDirection.ToString());
			this.GetComponent<GameController>().handDirection(mDirection);
		}
	}

	void MobileInput (){
		if (Input.touchCount <= 0)
			return;

		//1個手指觸碰螢幕
		if (Input.touchCount == 1) {

			//開始觸碰
			if (Input.touches [0].phase == TouchPhase.Began) {
				//Debug.Log("Began");
				//紀錄觸碰位置
				m_screenPos = Input.touches [0].position;

				//手指移動
			} else if (Input.touches [0].phase == TouchPhase.Moved) {
				//Debug.Log("Moved");
				//移動攝影機
				//Camera.main.transform.Translate (new Vector3 (-Input.touches [0].deltaPosition.x * Time.deltaTime, -Input.touches [0].deltaPosition.y * Time.deltaTime, 0));
			}

			//手指離開螢幕
			if (Input.touches [0].phase == TouchPhase.Ended || Input.touches [0].phase == TouchPhase.Canceled) {
				//Debug.Log("Ended");
				Vector2 pos = Input.touches [0].position;

				gDefine.Direction mDirection = handDirection(m_screenPos, pos);
				//Debug.Log("mDirection: " + mDirection.ToString());
				if (mDirection != gDefine.Direction.None)
					this.GetComponent<GameController> ().handDirection (mDirection);
				else
					Camera.main.transform.GetComponent<CameraController> ().Click (pos);
			}
		} 
	}

	gDefine.Direction handDirection(Vector2 StartPos, Vector2 EndPos){
		gDefine.Direction mDirection;
		if ((StartPos - EndPos).magnitude <= 50)
			return gDefine.Direction.None;
		//手指水平移動
		if (Mathf.Abs (StartPos.x - EndPos.x) > Mathf.Abs (StartPos.y - EndPos.y)) {
			if (StartPos.x > EndPos.x) {
				//手指向左滑動
				mDirection = gDefine.Direction.Left;
			} else {
				//手指向右滑動
				mDirection = gDefine.Direction.Right;
			}
		} else {
			if (m_screenPos.y > EndPos.y) {
				//手指向下滑動
				mDirection = gDefine.Direction.Down;
			} else {
				//手指向上滑動
				mDirection = gDefine.Direction.Up;
			}
		}
		return mDirection;
	}
}