using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	
	[SerializeField]
	GameObject Bullet;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0)){
			Shoot ();	
		}
	}
	
	public void Shoot () {
		
//		
//		Ray mouseRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
//		RaycastHit hitInfo;
//		
//		if (Physics.Raycast(mouseRay, out hitInfo)) {
//			
//			Debug.Log(hitInfo.transform.name);	
//		}
//		
		
 
         
        Instantiate(Bullet,transform.position,transform.rotation);
    	Bullet.rigidbody.AddForce(transform.forward);
	
	
		
//	    RaycastHit hit;
//		
//		if (Physics.Raycast(_muzzle.position, Vector3.forward, out hit)) {
//			
//			Debug.Log(hit.transform.name);
//			
//			Object clone = Instantiate ( Bullet, _muzzle.position, Quaternion.identity );
//
//   			Bullet.rigidbody.AddForce ( _muzzle.forward * 1000 );
//		
//			
//		}
		
		
	}
	
}
