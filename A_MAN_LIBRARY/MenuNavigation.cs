using UnityEngine;
using System.Collections;

public class MenuNavigation : MonoBehaviour {

	public void MainMenu()
	{
		Application.LoadLevel("menu");
	}

	public void Quit()
	{
		Application.Quit();
	}
	
	public void Play()
	{
		Application.LoadLevel("game");
	}
	

    public void Credits()
    {
        Application.LoadLevel("credits");
    }

	public void SourceCode()
	{
		Application.OpenURL("https://github.com/DONG-GI-YOU/a_man");
	}
}
