using UnityEngine;
using System.Collections;

public class NPCanimate : MonoBehaviour {
	
	public Animation _animation;
	
	[SerializeField]
	AnimationClip _animationClip;
	
	//public GameObject _player;
	
	//public float _distance;

	
	// Use this for initialization
	void Start () {
		
		_animation = GetComponent<Animation> ();
		string _animationToPlay = _animationClip.name.ToString();
		
		//int animCount = _animation.GetClipCount(); // clip?! Unity people, why not state??
    	//Debug.Log("Animations found: " + animCount );
		
		
		
		//_player = GameObject.FindGameObjectWithTag("Player");
		
		_animation[_animationToPlay].wrapMode = WrapMode.Loop;
			
		_animation.PlayQueued(_animationToPlay, QueueMode.CompleteOthers);
		
//		foreach(AnimationState s in _animation) {
//				
//			string theName = s.name.ToString();
//			//_animation[theName].wrapMode = WrapMode.Once;
//			_animation.PlayQueued(theName, QueueMode.CompleteOthers);
//			
//			
//			
//		}
//		
	
	}
	
	// Update is called once per frame
	void Update () {
//		
//		if( _player != null) {
//		
//			_distance = Vector3.Distance(_player.transform.position, gameObject.transform.position);
//			
//			if ( _distance < 20.0f ){
//				
//			}
//			
//		}
		//_animation.PlayQueued("FireReady_standing", QueueMode.CompleteOthers);
	}
	
	
//	public string[] GetAnimationNames (Animation anim) {
//		List<string> _list = new List<string>();
//		
//		foreach(Animation s in anim) {
//				
//			_list.Add(s.name);
//			
//		}
//		
//		return _list;
//		
//	}
	
	
}
