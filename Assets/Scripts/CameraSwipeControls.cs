 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSNodeMap;


public class CameraSwipeControls : MonoBehaviour {
	public GameObject NodeMap;
	private float cameraPanSpeed = 1.10f;
	public Agent agent;
	private Vector3 DampVel1;
	private Vector3 dest;
	private Vector3[] CameraPos;
	private Quaternion[] CameraRo;
	private List <System.Action> ToAnimate = new List<System.Action>();
	private Vector3 positions;
	private Vector3 rotations;
	private int curRo = 0;

	public Camera maincam;

	public void setupCameraPos()
	{
		CameraRo = new Quaternion[4];
		CameraRo [0] = Quaternion.Euler (new Vector3 (0, 0, 0));

		CameraRo [1] = Quaternion.Euler (new Vector3(0, 90, 0f));

		CameraRo [2] = Quaternion.Euler (new Vector3 (0, 180, 0));

		CameraRo [3] = Quaternion.Euler (new  Vector3 (0, 270, 0f));


	}
	private void Awake()

		{
		setupCameraPos ();


		SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
		agent.OnMoveEnd += addPositionTracking;

	}

	public void addPositionTracking(Node targetNode)
	{
		dest = new Vector3(agent.transform.position.x-17.5f, transform.position.y, agent.transform.position.z-17.5f);
		ToAnimate.Add (updatePos);		
	}

	public int cycleRestrictorUp(int max, int num)
	{
		if (num + 1 >= max)
			return 0;
		else
			return (num + 1);
	}

	public int cycleRestrictorDown(int max, int num)
	{
		if (num -1 < 0)
			return (max-1);
		else
			return (num - 1);
	}


	public void RightRotation()
	{
		NodeMap.transform.rotation = Quaternion.Lerp (NodeMap.transform.rotation, CameraRo [curRo], 5 * Time.deltaTime);
		NodeMap.GetComponent<Map> ().RedrawMap(true);
		if (Mathf.RoundToInt (Quaternion.Angle (NodeMap.transform.rotation, CameraRo [curRo])) == 0) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<TrailRenderer> ().enabled = true;

			ToAnimate.Remove (RightRotation);
		}
		//targets [i].transform.rotation = Quaternion.Lerp (targets[i].transform.rotation, rotationVectors [i], speed*Time.deltaTime);
	}

	public void LeftRotation()
	{
		NodeMap.transform.rotation = Quaternion.Lerp (NodeMap.transform.rotation, CameraRo [curRo], 5 * Time.deltaTime);
		NodeMap.GetComponent<Map> ().RedrawMap(true);
		if (Mathf.RoundToInt (Quaternion.Angle (NodeMap.transform.rotation, CameraRo [curRo])) == 0) {
			GameObject.FindGameObjectWithTag ("Player").GetComponent<TrailRenderer> ().enabled = true;

			ToAnimate.Remove (LeftRotation);
		}
			
	}

	public void RotateRight()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<TrailRenderer> ().enabled = false;
		curRo = cycleRestrictorUp (4, curRo);
		ToAnimate.Add (RightRotation);
		//NodeMap.transform.Rotate (new Vector3 (0, 90, 0), Space.World);	
		}

	public void RotateLeft()
	{
		GameObject.FindGameObjectWithTag ("Player").GetComponent<TrailRenderer> ().enabled = false;
		curRo = cycleRestrictorDown (4, curRo);
		ToAnimate.Add (LeftRotation);
	
	}

	private void SwipeDetector_OnSwipe(SwipeData data)
	{
		if (data.Direction == SwipeDirection.Right)
		{
			
//			transform.position = new Vector3(0,30,0);
//			Debug.Log ("RIGHT");
//			curPos = cycleRestrictorUp (3, curPos);
//			curRo = cycleRestrictorUp (3, curRo);
//			ToAnimate.Add (CameraPosRo);
			//transform.RotateAround (transform.position, agent.transform.position, -10f);
			//ToAnimate.Add (updatePos);

		}
		if (data.Direction == SwipeDirection.Down) {
			//dest = new Vector3 (dest.x + increment, dest.y, dest.z);
			//ToAnimate.Add (updatePos);

		}
		if (data.Direction == SwipeDirection.Left)
		{
			
			//transform.Rotate (new Vector3 (10, 0, 0));
			//dest = new Vector3 (dest.x, dest.y, dest.z - increment);
			//ToAnimate.Add (updatePos);

		}
		if (data.Direction == SwipeDirection.Up)
		{
			//dest = new Vector3 (dest.x - increment, dest.y, dest.z);
			//ToAnimate.Add (updatePos);

		}

	}

	public void arrowControls()
	{
		if (Input.GetKeyDown (KeyCode.UpArrow) ) 
		{
			transform.position = new Vector3 (transform.position.x + 5, transform.position.y, transform.position.z);

		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			transform.position = new Vector3 (transform.position.x - 5, transform.position.y, transform.position.z);

		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			transform.position = new Vector3 (transform.position.x , transform.position.y, transform.position.z+5);

		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z-5);

		}
		
	}

	public void updatePos()
	{
		transform.position = Vector3.MoveTowards (transform.position, dest, 10*Time.deltaTime);
		//transform.position = Vector3.MoveTowards (transform.position, dest, increment*Time.deltaTime);
		if (transform.position == dest) {
			ToAnimate.Remove (updatePos);
		}
	}

	void Update()
	{



		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
			if (!FindObjectOfType<Inventory> ().inventoryOpen) {
				Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
				transform.Translate (-touchDeltaPosition.y * cameraPanSpeed * Time.deltaTime - (touchDeltaPosition.x * cameraPanSpeed * Time.deltaTime), 0, touchDeltaPosition.x * cameraPanSpeed * Time.deltaTime + (-touchDeltaPosition.y * cameraPanSpeed * Time.deltaTime), Space.World);
			}
		}

		if (Application.isEditor) {
			arrowControls ();
		}

		for (int i = 0; i < ToAnimate.Count; i++)
			ToAnimate [i] ();
			
	}

}
	

