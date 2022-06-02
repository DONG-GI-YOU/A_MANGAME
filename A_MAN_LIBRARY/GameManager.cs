using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour {
  

    public static int Level = 0;
    public static int lives = 3;
	public enum GameState { Init, Game, Dead, Scores }
	public static GameState gameState;

    private GameObject Student;
    private GameObject Prof1;
    private GameObject Prof2;
    private GameObject Prof3;
    private GameObject Prof4;
    private GameGUINavigation gui;
    
    static public int score;

    public float SpeedPerLevel;
    
    private static GameManager _instance;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            if(this != _instance)   
                Destroy(this.gameObject);
        }
        AssignGhosts();
    }
	void Start () 
	{
		gameState = GameState.Init;
	}
    void OnLevelWasLoaded()
    {
        if (Level == 0) lives = 3;
        Debug.Log("Level " + Level + " Loaded!");
        AssignGhosts();
        ResetVariables();
     
        Prof4.GetComponent<Profmove>().speed += Level * SpeedPerLevel;
        Prof1.GetComponent<Profmove>().speed += Level * SpeedPerLevel;
        Prof2.GetComponent<Profmove>().speed += Level * SpeedPerLevel;
        Prof3.GetComponent<Profmove>().speed += Level * SpeedPerLevel;
        Student.GetComponent<PlayerController>().speed += Level*SpeedPerLevel/2;
    }
    private void ResetVariables()
    {
        _timeToCalm = 0.0f;
        scared = false;
        PlayerController.killstreak = 0;
    }
   
	void Update () 
	{
		if(scared && _timeToCalm <= Time.time)
			CalmProfs();
	}
	public void ResetScene()
	{
        CalmProfs();
		Student.transform.position = new Vector3(15f, 11f, 0f);
		Prof1.transform.position = new Vector3(15f, 20f, 0f);
		Prof2.transform.position = new Vector3(14.5f, 17f, 0f);
		Prof3.transform.position = new Vector3(16.5f, 17f, 0f);
		Prof4.transform.position = new Vector3(12.5f, 17f, 0f);
		Student.GetComponent<PlayerController>().ResetDestination();
		Prof1.GetComponent<Profmove>().InitializeProf();
		Prof2.GetComponent<Profmove>().InitializeProf();
		Prof3.GetComponent<Profmove>().InitializeProf();
		Prof4.GetComponent<Profmove>().InitializeProf();
        gameState = GameState.Init;  
        gui.H_ShowReadyScreen();
	}
	
    void AssignGhosts()
    {
       
        Prof4 = GameObject.Find("Prof4");
        Prof2 = GameObject.Find("Prof2");
        Prof3 = GameObject.Find("Prof3");
        Prof1 = GameObject.Find("Prof1");
        Student = GameObject.Find("Student");
        if (Prof4 == null || Prof2 == null || Prof3 == null || Prof1 == null) Debug.Log("One of profs are NULL");
        if (Student == null) Debug.Log("Student is NULL");
        gui = GameObject.FindObjectOfType<GameGUINavigation>();
        if(gui == null) Debug.Log("GUI Handle Null!");
    }
    public void LoseLife()
    {
        lives--;
        gameState = GameState.Dead;
    
        
        UIScript ui = GameObject.FindObjectOfType<UIScript>();
        Destroy(ui.lives[ui.lives.Count - 1]);
        ui.lives.RemoveAt(ui.lives.Count - 1);
    }
    public static void DestroySelf()
    {
        score = 0;
        Level = 0;
        lives = 3;
        Destroy(GameObject.Find("Game Manager"));
    }
}