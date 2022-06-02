using UnityEngine;
using System.Collections;

public class Studot : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Student")
		{
			GameManager.score += 10;
		    GameObject[] pacdots = GameObject.FindGameObjectsWithTag("Studot");
            Destroy(gameObject);

		    if (Studots.Length == 1)
		    {
		        GameObject.FindObjectOfType<GameGUINavigation>().LoadLevel();
		    }
		}
	}
}