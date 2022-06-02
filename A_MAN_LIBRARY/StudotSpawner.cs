using UnityEngine;
using System.Collections;

public class StudotSpawner : MonoBehaviour {
	
	public GameObject Studot;
	public float interval;
	public float startOffset;
	
	private float startTime;
	
	void Start () 
	{
		startTime = Time.time + startOffset;
	}
		
	void Update () 
	{
		if(Time.time > startTime + interval)
		{
			GameObject obj = (GameObject)Instantiate(Studot, transform.position, Quaternion.identity);
			obj.transform.parent = transform;
			startTime = Time.time;
		}
	}
}