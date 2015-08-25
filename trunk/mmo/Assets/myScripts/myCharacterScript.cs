using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PhotonView))]
public class myCharacterScript : Photon.MonoBehaviour{
	
//	Animation _animation;
//	
//	Targeting _target;
//	
	[SerializeField]
	public Transform currentTarget = null;
	
	GameObject _marker;
	
	public MeshRenderer _targetRendererPlayer;
	GameObject _theTargetMarkerPlayer;
	MeshRenderer _targetRendererNPC;
	Transform _theTargetMarkerNPC;
	
	public bool _imDead = false;
	
	
	public float _targetRange;
	
	[SerializeField]
	public string _buttonOne_label = "AutoShot";
	[SerializeField]
	public string _buttonTwo_label = "Shove";
	[SerializeField]
	public string _buttonThree_label = "'Nade";
	

	public string spell_1_cd = "";
	public string spell_2_cd = "";
	public string spell_3_cd = "";
	
	[SerializeField]
	string resourcesName;
	
	int _bullets = 100;
	public int _stamina = 1000;
	
	float _staminaRegenTimer = 1.0F;
	float _staminaRegenNextTick = 0.0F;
	
	float _staminaDeductTimer = 2.0F;
	float _staminaDeductNextTick = 0.0F;
	
	float Spell_1_nextFire = 0.0F;
	float Spell_1_fireRate = 3.5F;
	
	float Spell_2_nextFire = 0.0F;
	float Spell_2_fireRate = 1.5F;
	
	float Spell_3_nextFire = 0.0F;
	float Spell_3_fireRate = 5.5F;
		
	ThirdPersonControllerNET controllerScript;
	HealthAndDamageNET _healthScript;

	GameObject[] _spawnPoints;
	
	


	// Use this for initialization
	void Start () {
		 //Screen.lockCursor = true;
		
		//_target = GetComponent<Targeting> ();
		
        //controllerScript = photonView.gameObject.GetComponent<ThirdPersonControllerNET>();
		
		_healthScript = GetComponent<HealthAndDamageNET> ();
		
		
		
		
	}
	
	void OnGUI () {
		
		if ( currentTarget != null) {
			
			if ( currentTarget.gameObject.tag == "NPC" ) {
				
				string stringToShow = currentTarget.gameObject.name.ToString();
			
				GUI.Box(new Rect(200, 30, 150, 25), "Target: " + stringToShow);
			
			}
			
			if ( currentTarget.gameObject.tag == "Player") {
				
				
				PhotonView _otherPlayer = currentTarget.GetComponent<PhotonView> ();
				
				string _otherPlayersName = _otherPlayer.owner.name.ToString();
				
				GUI.Box(new Rect(200, 30, 150, 25), "Target: " + _otherPlayersName);
				
			}
			
			if ( currentTarget.gameObject.tag == "Enemy") {
				
				
				FreeAI _theAI = currentTarget.GetComponent<FreeAI> ();
				
				string _enemyName = _theAI.EnemysName;
				
				GUI.Box(new Rect(200, 30, 150, 25), "Target: " + _enemyName);
				
			}
				
			
			_targetRange = Mathf.Round(_targetRange * 100.0f) / 100.0f;
			
			if (_targetRange < 0 )
				_targetRange = _targetRange * -1;
			
			GUI.Box(new Rect(200, 60, 150, 25), "Range: " + _targetRange);
		}
		
		
		//GUI.Box(new Rect(1, 60, 150, 25), "Health: " + _PlayerHealth);	
		GUI.Box(new Rect(1, 30, 120, 25), photonView.owner.name);
		GUI.Box(new Rect(10, 60, 100, 25), "Health: " + _healthScript.health);
		GUI.Box(new Rect(10, 90, 100, 25), "Bullets: " + _bullets);
		GUI.Box(new Rect(10, 120, 100, 25), "Stamina: " + _stamina);
		
		
		
		if ( GUI.Button (new Rect((Screen.width / 2) - 55, (Screen.height - 55), 50, 50), "1 \n" + _buttonOne_label + "\n " + spell_1_cd ) )
		{
				Spell (1);
		}
		
		if ( GUI.Button (new Rect((Screen.width / 2), (Screen.height - 55), 50, 50), "2 \n" + _buttonTwo_label + "\n " + spell_2_cd) )
		{
				Spell (2);
		}
		
		if ( GUI.Button (new Rect((Screen.width / 2) + 55, (Screen.height - 55), 50, 50), "3 \n" + _buttonThree_label + "\n " + spell_3_cd) )
		{
				Spell (3);
		}
		
		if ( _healthScript.health <= 0 ) {
			
			GUI.Box(new Rect(Screen.width /2, Screen.height / 2, 120, 25), "You Died");
				
			if ( GUI.Button (new Rect((Screen.width / 2), (Screen.height / 2) + 50, 120, 25), "Re Spawn?") )
			{
				 photonView.RPC("reSpawn", PhotonTargets.AllBuffered);
			}
		}
		
		
	}

	
	void deductStamina ( int s ) {
		
		if ( _stamina > 0 && Time.time > _staminaDeductNextTick ) {
			
			_staminaDeductNextTick = Time.time + _staminaDeductTimer;
			
			_stamina -= s;
		}
	
		
	}
	
	void RegenStamina ( int r ) {
		
		if ( _stamina < 1000 && Time.time > _staminaRegenNextTick ) {
				
				_staminaRegenNextTick = Time.time + _staminaRegenTimer;
				
				_stamina += r;
		}
		
		if (_stamina > 1000 ) {
			
			_stamina = 1000;
			
		}
	
	}
	
	[RPC]
	void onDeath() {
		
		_imDead = true;	
		photonView.RPC("Died", PhotonTargets.AllBuffered);
		
		controllerScript = photonView.gameObject.GetComponent<ThirdPersonControllerNET>();
		controllerScript.enabled = false;
	}
	
	[RPC]
	void onHit() {
		
		
		photonView.RPC("Hit", PhotonTargets.AllBuffered);
		
		//controllerScript = photonView.gameObject.GetComponent<ThirdPersonControllerNET>();
		//controllerScript.enabled = false;
	}
	
	
	[RPC]
	void reSpawn() {
		
		_spawnPoints = GameObject.FindGameObjectsWithTag("spawnPoint");
		
		GameObject _mySpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
		
		controllerScript = photonView.gameObject.GetComponent<ThirdPersonControllerNET>();
		
		_healthScript = GetComponent<HealthAndDamageNET> ();
		
		
		_healthScript.health = 100.0f;
		
		_imDead = false;
		
		controllerScript.enabled = true;
		
		gameObject.transform.position = _mySpawnPoint.transform.position;
		
		photonView.RPC("reSpawned", PhotonTargets.AllBuffered);
		
	}
	
	

	
	void AutoShot() {
		
		
			
			if ( _targetRange <= 10.0f ) {
	
				if ( Time.time > Spell_1_nextFire && _bullets >= 5 && _stamina >= 10 ) {
					
				
					if ( currentTarget != null && currentTarget.gameObject.name != gameObject.name )
					{	
					
							
							float dmg = 10.0f;
							
							_bullets -= 5;
					
							_stamina -= 10;
						
							Spell_1_nextFire = Time.time + Spell_1_fireRate;
					
							photonView.RPC ("Attacked", PhotonTargets.All, 1);
							
						
							PhotonView _targetNET = currentTarget.gameObject.GetComponent<PhotonView> ();
						
							if ( _targetNET != null) {
						
								//transform.LookAt(currentTarget.position);
						
								_targetNET.RPC("onDamageTaken",_targetNET.owner, dmg);
								
								
							}
			
					}
				}
			}
		
	}
	
	void Shove() {
		
		if ( _targetRange <= 2.5f ) {
	
				if ( Time.time > Spell_2_nextFire && _stamina >= 150 ) {
					
					
				
					if ( currentTarget != null && currentTarget.gameObject.name != gameObject.name )
					{	
							
							//transform.LookAt(currentTarget.position);		
					
							float dmg = 15.0f;
							
							_stamina -= 150;
						
							Spell_2_nextFire = Time.time + Spell_2_fireRate;
					
							photonView.RPC ("Attacked", PhotonTargets.All, 2);
						
							PhotonView _targetNET = currentTarget.gameObject.GetComponent<PhotonView> ();
						
							if ( _targetNET != null) {
						
								_targetNET.RPC("onDamageTaken",_targetNET.owner, dmg);
							}
			
					}
				}
			}

	}
	
	void Grenade() {
		
		if ( _targetRange <= 7.5f ) {
	
				if ( Time.time > Spell_3_nextFire && _stamina >= 15 ) {
					
					
				
					if ( currentTarget != null && currentTarget.gameObject.name != gameObject.name )
					{	
							
							//transform.LookAt(currentTarget.position);		
					
							float dmg = 95.0f;
							
							_stamina -= 15;
						
							Spell_2_nextFire = Time.time + Spell_2_fireRate;
					
							photonView.RPC ("Attacked", PhotonTargets.All, 3);
						
							PhotonView _targetNET = currentTarget.gameObject.GetComponent<PhotonView> ();
						
							if ( _targetNET != null) {
						
								_targetNET.RPC("onDamageTaken",_targetNET.owner, dmg);
							}
			
					}
				}
			}

	}
	
	
	
	void Spell ( int i){
			
		switch ( i )
		{
			case 1:
				AutoShot();	
			break;
			
			case 2:
				Shove();
			break;
			
			case 3:
				Grenade();
			break;
		}
		
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
		if ( Spell_1_nextFire - Time.time > 0 ) {
			spell_1_cd = Mathf.Round((((Spell_1_nextFire - Time.time) * 100.0f) / 100.0f)).ToString();
		}
		if ( Spell_2_nextFire - Time.time > 0 ) {
			spell_2_cd = Mathf.Round((((Spell_2_nextFire - Time.time) * 100.0f) / 100.0f)).ToString();
		}
		if ( Spell_3_nextFire - Time.time > 0 ) {
			spell_3_cd = Mathf.Round((((Spell_3_nextFire - Time.time) * 100.0f) / 100.0f)).ToString();
		}
		
		if ( spell_1_cd == "0" ){
				spell_1_cd = "";
		}
		if ( spell_2_cd == "0" ){
				spell_2_cd = "";
		}
		if ( spell_3_cd == "0" ){
				spell_3_cd = "";
		}
		
		if ( currentTarget != null) 
			_targetRange = Vector3.Distance (currentTarget.position, gameObject.transform.position);
		
		if ( Input.GetButtonDown ("Hotkey1")) 
			Spell (1);
		
		if ( Input.GetButtonDown ("Hotkey2")) 
			Spell (2);
		
		if ( Input.GetButtonDown ("Hotkey3")) 
			Spell (3);
		
		
		
		if ( Input.GetMouseButtonDown(0)){
			
			RaycastHit hitInfo;
			
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//currentTarget = null;
			if (Physics.Raycast (ray, out hitInfo, 500.0f)){
			
				if ( hitInfo.transform.gameObject.tag == "NPC" || hitInfo.transform.gameObject.tag == "Player"
					|| hitInfo.transform.gameObject.tag == "Enemy"){
					
					//If a PC or NPC is clicked, Enable its target marker icon
					foreach (GameObject o in GameObject.FindGameObjectsWithTag("targetMarker")) {
					
						_targetRendererPlayer = o.GetComponent<MeshRenderer> ();
						_targetRendererPlayer.enabled = false;
					
					}
	
					//Our new target
					currentTarget = hitInfo.transform;
					
					_theTargetMarkerNPC = currentTarget.gameObject.transform.Find("targetMarker");
				
					_targetRendererNPC = _theTargetMarkerNPC.gameObject.GetComponent<MeshRenderer> ();
				
					if (_targetRendererNPC.enabled == false)
						_targetRendererNPC.enabled = true;
				
				}
				
				}else {
					//No suitable target was found
					currentTarget = null;	
				}

		}
		
	}
	
	
//	void MeleeAttack () {
//		_animation = gameObject.GetComponent<Animation>();
//		_animation["Butt_kick"].wrapMode = WrapMode.Once;
//		_animation.Play("Butt_kick");	
//	}
	

}