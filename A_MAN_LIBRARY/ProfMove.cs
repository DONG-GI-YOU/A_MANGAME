using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Profmove : MonoBehaviour {


	private Vector3 waypoint;			
	private Queue<Vector3> waypoints;	

	public Vector3 _direction;
	public Vector3 direction 
	{
		get
		{
			return _direction;
		}

		set
		{
			_direction = value;
			Vector3 pos = new Vector3((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
			waypoint = pos + _direction;
		
		}
	}

	public float speed = 0.3f;

 
	public float waitLength = 0.0f;


	private float timeToEndWait;

	enum State { Wait, Init, Chase, Run };
	State state;

    private Vector3 _startPos;
    private float _timeToWhite;
    private float _timeToToggleWhite;
    private float _toggleInterval;
    private bool isWhite = false;

	
	public GameGUINavigation GUINav;
    public PlayerController Student;
    private GameManager _gm;

	
	void Start()
	{
	    _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _toggleInterval = _gm.scareLength * 0.33f * 0.20f;  
		Initializeprof();
	}

    public float DISTANCE;

	void FixedUpdate ()
	{
	    DISTANCE = Vector3.Distance(transform.position, waypoint);

		if(GameManager.gameState == GameManager.GameState.Game){
			animate ();

			switch(state)
			{
			case State.Wait:
				Wait();
				break;

			case State.Init:
				Init();
				break;

			case State.Chase:
				ChaseAI();
				break;

			case State.Run:
				RunAway();
				break;
			}
		}
	}

	
	public void InitializeGhost()
	{
	    _startPos = getStartPosAccordingToName();
		waypoint = transform.position;	
		state = State.Wait;
	    timeToEndWait = Time.time + waitLength + GUINav.initialDelay;
		InitializeWaypoints(state);
	}

    public void Initializeprof(Vector3 pos)
    {
        transform.position = pos;
        waypoint = transform.position;	
        state = State.Wait;
        timeToEndWait = Time.time + waitLength + GUINav.initialDelay;
        InitializeWaypoints(state);
    }
	

    private void InitializeWaypoints(State st)
    {
 
        string data = "";
        switch (name)
        {
        case "Prof1": 
            data = @"22 20
22 26

27 26
27 30
22 30
22 26";
            break;
        case "Prof2":
            data = @"14.5 17
14 17
14 20
7 20

7 26
7 30
2 30
2 26";
            break;
        case "Prof3":
            data = @"16.5 17
15 17
15 20
22 20

22 8
19 8
19 5
16 5
16 2
27 2
27 5
22 5";
            break;
        case "Prof4":
            data = @"12.5 17
14 17
14 20
7 20

7 8
7 5
2 5
2 2
13 2
13 5
10 5
10 8";
            break;
        
        }

       
        string line;

        waypoints = new Queue<Vector3>();
        Vector3 wp;

        if (st == State.Init)
        {
            using (StringReader reader = new StringReader(data))
            {
                
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Length == 0) break;  

                    string[] values = line.Split(' ');
                    float x = float.Parse(values[0]);
                    float y = float.Parse(values[1]);

                    wp = new Vector3(x, y, 0);
                    waypoints.Enqueue(wp);
                }
            }
        }

       
        if (st == State.Wait)
        {
            Vector3 pos = transform.position;

           
            if (transform.name == "Prof3" || transform.name == "Prof4")
            {
                waypoints.Enqueue(new Vector3(pos.x, pos.y - 0.5f, 0f));
                waypoints.Enqueue(new Vector3(pos.x, pos.y + 0.5f, 0f));
            }
            
            else
            {
                waypoints.Enqueue(new Vector3(pos.x, pos.y + 0.5f, 0f));
                waypoints.Enqueue(new Vector3(pos.x, pos.y - 0.5f, 0f));
            }
        }

    }

    private Vector3 getStartPosAccordingToName()
    {
        switch (gameObject.name)
        {
            case "Prof1":
                return new Vector3(15f, 20f, 0f);

            case "Prof2":
                return new Vector3(14.5f, 17f, 0f);
            
            case "Prof3":
                return new Vector3(16.5f, 17f, 0f);

            case "Prof4":
                return new Vector3(12.5f, 17f, 0f);
        }

        return new Vector3();
    }

	
	void animate()
	{
		Vector3 dir = waypoint - transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
		GetComponent<Animator>().SetBool("Run", false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Student")
		{
			
		    if (state == State.Run)
		    {
		        Calm();
		        Initializeprof(_startPos);
                Student.UpdateScore();
		    }
		       
		    else
		    {
		        _gm.LoseLife();
		    }

		}
	}

	
	void Wait()
	{
		if(Time.time >= timeToEndWait)
		{
			state = State.Init;
		    waypoints.Clear();
			InitializeWaypoints(state);
		}

		
		MoveToWaypoint(true);
	}

	void Init()
	{
	    _timeToWhite = 0;

		
		if(waypoints.Count == 0)
		

		
		MoveToWaypoint();
	}



    void ChaseAI()
	{

       
        if (Vector3.Distance(transform.position, waypoint) > 0.000000000001)
		{
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint, speed);
			GetComponent<Rigidbody2D>().MovePosition(p);
		}

		
		else GetComponent<AI>().AILogic();

	}

	
	void MoveToWaypoint(bool loop = false)
	{
		waypoint = waypoints.Peek();		
        if (Vector3.Distance(transform.position, waypoint) > 0.000000000001)	
		{									                        
			_direction = Vector3.Normalize(waypoint - transform.position);	
			Vector2 p = Vector2.MoveTowards(transform.position, waypoint, speed);
			GetComponent<Rigidbody2D>().MovePosition(p);
		}
		else 	
		{
			if(loop)	waypoints.Enqueue(waypoints.Dequeue());
			else		waypoints.Dequeue();
		}
	}


}