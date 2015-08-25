using UnityEngine;
using System.Collections;

public class CloseCameraFade : MonoBehaviour
{
	new public Camera camera;
	public Transform cameraTarget;
	new public Renderer[] renderer;
	public float fadeDistance = 2.0f, hideDistance = 1.0f;
	
	
	void Reset ()
	{
		Setup ();
	}
	
	
	void Setup ()
	{
		if (cameraTarget == null)
		{
			cameraTarget = GetComponent<Transform> ();
		}
		
		if (renderer == null)
		{
			renderer = GetComponents<Renderer> ();
		}
		
		if (camera == null)
		{
			camera = Camera.main;
		}
	}
	
	
	void Start ()
	{
		Setup ();
		
		if (cameraTarget == null)
		{
			Debug.LogError ("No camera target assigned. Please correct and restart.");
			enabled = false;
			return;
		}
		
		if (renderer == null)
		{
			Debug.LogError ("No renderer assigned. Please correct and restart.");
			enabled = false;
			return;
		}
		
		if (camera == null)
		{
			Debug.LogError ("No camera assigned. Please correct and restart.");
			enabled = false;
			return;
		}
	}
	
	
	void Update ()
	{
		float distance = (cameraTarget.transform.position - camera.transform.position).magnitude;
		
		if (distance < hideDistance)
		{
			foreach(Renderer r in renderer) {
				r.enabled = false;
			}
		}
		else if (distance < fadeDistance)
		{
			foreach(Renderer r in renderer) {
				r.enabled = true;
			}
			
			float alpha = 1.0f - (fadeDistance - distance) / (fadeDistance - hideDistance);
			
			foreach(Renderer r in renderer) {
				if (r.material.color.a != alpha)
				{
					r.material.color = new Color (r.material.color.r, r.material.color.g, r.material.color.b, alpha);
				}
			}
		}
		else
		{
			foreach(Renderer r in renderer) {
				r.enabled = true;
				if (r.material.color.a != 1.0f)
				{
					r.material.color = new Color (r.material.color.r, r.material.color.g, r.material.color.b, 1.0f);
				}
			}
		}
	}
}
