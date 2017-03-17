using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {
	
	public float rotateSpeed = 5f;
	
	void Update () {
		transform.Rotate (new Vector3(0, rotateSpeed*Time.deltaTime,0));
	}
}
