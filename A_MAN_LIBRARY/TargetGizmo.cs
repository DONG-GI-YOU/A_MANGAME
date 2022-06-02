using UnityEngine;
using System.Collections;

public class TargetGizmo : MonoBehaviour {

	public GameObject Prof;
	 
	void Start () 
	{
	
	}
		 
	void Update () 
	{
		if(Prof.GetComponent<AI>().targetTile != null)
		{
			Vector3 pos = new Vector3(Prof.GetComponent<AI>().targetTile.x, 
										Prof.GetComponent<AI>().targetTile.y, 0f);
			transform.position = pos;
		}
	}
}