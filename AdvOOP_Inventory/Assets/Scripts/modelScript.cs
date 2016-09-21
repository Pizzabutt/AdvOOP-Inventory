using UnityEngine;
using System.Collections;

public class modelScript : MonoBehaviour
{
	public float roteSpeed, Sensitivity;
	public float rotationDamper;

	private Vector3 mouseReferrence, mouseOffset;
	private Vector3 rotation;
	private bool isRotating;

	void Update()
	{
		if(isRotating)
		{
			mouseOffset = Input.mousePosition - mouseReferrence;
			rotation.y = (mouseOffset.x + mouseOffset.y) * -Sensitivity;
			transform.Rotate(rotation);
			mouseReferrence = Input.mousePosition;
		}
		else
		{
			float sign = rotation.y < 0 ? -1 : 1;
			rotation.y -= rotationDamper * sign * Time.deltaTime;
			rotation.y = Mathf.Abs(rotation.y) <= sign ? sign : rotation.y;
			transform.rotation *= Quaternion.AngleAxis(roteSpeed * rotation.y, rotation);
		}
	}

	void OnMouseDown()
	{
		isRotating = true;
		mouseReferrence = Input.mousePosition;
	}
	void OnMouseUp()
	{
		isRotating = false;
	}
}
