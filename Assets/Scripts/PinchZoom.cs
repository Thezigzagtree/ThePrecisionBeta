using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchZoom : MonoBehaviour {
		
	public float perspectiveZoomSpeed = 0.5f;
	public float orthoZoomSpeed = 0.5f;
	public Camera maincam;
	private enum DraggedDirection { Up, Down}


	void Update()
	{
			
		if (Input.touchCount == 2) {
			
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;



			float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;

			if (maincam.orthographic) {
				maincam.orthographicSize += deltaMagnitudediff * orthoZoomSpeed;
				maincam.orthographicSize = Mathf.Max (maincam.orthographicSize, .1f);
				if (maincam.orthographicSize >= 58)
					maincam.orthographicSize = 58;
				else if (maincam.orthographicSize <= 10)
					maincam.orthographicSize = 10;
			} else {
				maincam.fieldOfView += deltaMagnitudediff * perspectiveZoomSpeed;
				maincam.fieldOfView = Mathf.Clamp (maincam.fieldOfView, 30f, 75f);

			}
		}
	}

}
