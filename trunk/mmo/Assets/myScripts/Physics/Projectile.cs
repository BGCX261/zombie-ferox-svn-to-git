using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public bool _isExplosive;
	public int _damage;
	public float _dissapearTime=1.0f;
	public GameObject _explosionPrefab;
	public GameObject[] _collisionPrefab;//0 = obstacle, 1 = enemy/player hit
	
	void Start(){
		Destroy(gameObject,_dissapearTime);
	}
	
	void OnCollisionEnter(Collision col){
		ContactPoint contact = col.contacts[0];
		Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		if(_isExplosive)
			Instantiate(_explosionPrefab,pos,Quaternion.identity);
		else{
			col.gameObject.SendMessage("ApplyDamage",_damage, SendMessageOptions.DontRequireReceiver);
			if(col.gameObject.tag =="Player" || col.gameObject.tag =="Enemy")
				Instantiate(_collisionPrefab[1],pos,rot);
			else
				Instantiate(_collisionPrefab[0],pos,rot);
		}
		Destroy(gameObject);	
	}
}