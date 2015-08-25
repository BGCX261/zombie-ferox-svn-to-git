using UnityEngine;

using System.Collections;

using System.Collections.Generic;

public class Targeting : MonoBehaviour {

    // Use this for initialization

    public List<Transform> targets;

    public Transform SelectTarget;


    private Transform myTransform;

	
	myCharacterScript _playerScript;

    

    void Start () {

	    targets = new List<Transform>();
	
	    addallenemys();
	
	    SelectTarget = null;
	
	    myTransform = transform;


    }

    

    public void addallenemys()

    {
	
	    GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC"); 
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player"); 
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy"); 
	
	    foreach(GameObject n in npcs) {
			
			float _targetRangeNPC = Vector3.Distance (n.transform.position, gameObject.transform.position);
			
			if ( _targetRangeNPC < 20.0f ) 
         		AddTarget(n.transform);
			else
				SubtractTarget(n.transform);
			
		}

		foreach(GameObject p in players) {
			
			float _targetRangePlayer = Vector3.Distance (p.transform.position, gameObject.transform.position);
			
			if ( _targetRangePlayer < 20.0f ) 
				AddTarget(p.transform);
			else
				SubtractTarget(p.transform);
			
		}
		
		foreach(GameObject o in enemies) {
			
			float _targetRangeEnemy = Vector3.Distance (o.transform.position, gameObject.transform.position);
			
			if ( _targetRangeEnemy < 20.0f ) 
				AddTarget(o.transform);
			else
				SubtractTarget(o.transform);
			
		}
		

    }

    

 

    public void AddTarget(Transform objToAdd)

    {

    	targets.Add(objToAdd); 

    }
	
	public void SubtractTarget(Transform objToSub)

    {
		if ( targets != null ) {
			int theIndex = targets.IndexOf(objToSub);
			
			if ( theIndex != null )
	    		targets.RemoveAt(theIndex); 
		}

    }


    

    

    private void SortTargetsByDistance()

    {

    	targets.Sort(delegate(Transform t1, Transform t2) 
			{ 
			
			return (Vector3.Distance(t1.position, myTransform.position).CompareTo(Vector3.Distance(t2.position, myTransform.position)));   
        	});

    }

    private void targetenemy()

    {
		
		
		
   		if(SelectTarget == null)

        {
			
		
			addallenemys();
	        SortTargetsByDistance();
	
	        SelectTarget = targets[0];
				
			

        } 
		else
		{


		    int index = targets.IndexOf(SelectTarget);
			
		    if (index < targets.Count - 1)
		
		        {
		
		        	index = index + 1;
		
		        }
		
	        else 
	
		        {
		
		        	index = 0;
					
		        }
			
			
		    SelectTarget = targets[index];
			
			
			
			
		}

}

    

    // Update is called once per frame

    void Update () {

	    if(Input.GetButtonDown("Target")){
				
//			foreach ( Transform t in targets ) {
//				Transform _theTargetMarker1 = t.gameObject.transform.Find("targetMarker");
//			
//				MeshRenderer _targetRenderer1 = _theTargetMarker1.gameObject.GetComponent<MeshRenderer> ();
//				
//				_targetRenderer1.enabled = false;
//			}
//			
			foreach (GameObject o in GameObject.FindGameObjectsWithTag("targetMarker")) {
					MeshRenderer _targetRendererPlayer = o.GetComponent<MeshRenderer> ();
					_targetRendererPlayer = o.GetComponent<MeshRenderer> ();
					_targetRendererPlayer.enabled = false;
				
			}
				
            targetenemy();
		
			if (SelectTarget != null){
				
				_playerScript = GetComponent<myCharacterScript> ();
				
				float _targetRangePlayer = Vector3.Distance (SelectTarget.position, gameObject.transform.position);
			
				if ( _targetRangePlayer < 20.0f ) {
					_playerScript.currentTarget = SelectTarget;
			
					Transform _theTargetMarker = SelectTarget.gameObject.transform.Find("targetMarker");
				
					MeshRenderer _targetRenderer = _theTargetMarker.gameObject.GetComponent<MeshRenderer> ();
				
					if (_targetRenderer.enabled == false)
						_targetRenderer.enabled = true;
					
				}
				
			
				
			}
	    }
	
	
		
	}

}