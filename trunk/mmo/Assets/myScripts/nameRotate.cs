using UnityEngine;
using System.Collections;

public class nameRotate : MonoBehaviour {
	
	//float _theRotationX;
	//float _theRotationY;
	//float _theRotationZ;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//_theRotationY = transform.rotation.y;
		
		//transform.LookAt(Camera.main.transform);
		
		Quaternion _camera = Camera.main.transform.rotation;
		
		transform.rotation = _camera;
		
	
		
	}
}
