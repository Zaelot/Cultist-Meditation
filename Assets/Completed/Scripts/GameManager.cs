using UnityEngine;
using System.Collections;

/// <summary>
/// Game manager taken from 2D Roguelike Asset package. Repurposing for our project.
/// ~Z 16.01.30 |
/// </summary>
namespace Completed
{
	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	using UnityEngine.SceneManagement;		//~Z 16.01.30 | Allows the use of SceneManager
	
	public class GameManager : MonoBehaviour
	{
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.1f;							//Delay between each Player turn.
		public int playerFoodPoints = 100;						//Starting value for Player food points. ~Z 16.01.30 | I guess this'll be our health
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		[HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
		
		
		private Text levelText;									//Text to display current level number. ~Z 16.01.30 | Called floors
		private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
		private int level = 1;									//Current level number, expressed in game as "Day 1". ~Z 16.01.30 | Floor, though text is omitted
		private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands. ~Z 16.01.30 | Need to remember position and status
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		public bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
		//Done ~Z 16.01.30 | Remember to activate doingSetup when in StartMenu as well (on the following playthroughs)
		//~Z 16.01.30 | Should work by just loading static level from a separate scene.
		//~Z 16.01.30 | Separate lists for each floor, to track progress between playthroughs.
		private List<Enemy> enemies2;
		private List<Enemy> enemies3;
		private List<Enemy> enemies4;
		private List<Enemy> enemies5;
		
		
		//Awake is always called before any Start functions ~Z 16.01.30 | Not 100% clear whether this is done on each Scene reload?
		void Awake()
		{
			Debug.Log ("GameManager is Awake!");
			//~Z 16.01.30 | Setting up this singleton - important since we're using this with persistent DontDestroyOnLoad
			//Check if instance already exists
			if (instance == null)				
				//if not, set instance to this
				instance = this;			
			//If instance already exists and it's not this:
			else if (instance != this)
				//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
				Destroy(gameObject);	
			
			//Sets this to not be destroyed when reloading scene - or loading a new scene.
			DontDestroyOnLoad(gameObject);
			
			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();
			enemies2 = new List<Enemy>(); //~Z 16.01.30 | assuming this is only run once per game session.
			enemies3 = new List<Enemy>();
			enemies4 = new List<Enemy>();
			enemies5 = new List<Enemy>();
			
			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardManager>();
			
			//Call the InitGame function to initialize the first level 
			InitGame();
		} //End.Awake()
		
		//This is called each time a scene is loaded.
		void OnLevelWasLoaded(int index)
		{
			Debug.Log ("Changed level, incrementing: " + level);
			//~Z 16.01.30 | if on second playthrough, reset level count and skip building the level
			if ( SceneManager.GetActiveScene ().name == "HomeApartment" ) {
				level = 0;
				doingSetup = true; //~Z 16.01.30 | don't attempt enemy movement while in start menu
				return;
			}
				
			//Add one to our level number.
			level++;
			//Call InitGame to initialize our level.
			InitGame ();
		} //End.OnLevelWasLoaded()
		
		//Initializes the game for each level.
		void InitGame()
		{
			string levelName = SceneManager.GetActiveScene ().name;
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;
			
			//Get a reference to our image LevelImage by finding it by name.
//			levelImage = GameObject.Find("LevelImage");
			levelImage = GameObject.Find("Image Loading"); //~Z 16.01.30 | Used to hide the level generation/Loading
			
			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
//			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			levelText = GameObject.Find("Text Floor").GetComponent<Text>(); //~Z 16.01.30 | Used to inform viewers of the progress
			
			//Set the text of levelText to the string "Day" and append the current level number.
//			levelText.text = "Day " + level;
			levelText.text = level.ToString();
			
			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true); //TODO ~Z 16.01.30 | Decide if it's indeed needed. If so, use HideLevelImage as well.
			
			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay); //~Z 16.01.30 | Apparently assuming the level creation will be done by the time the levelStartDelay runs out. Odd.
			
			//Clear any Enemy objects in our List to prepare for next level. ~Z 16.01.30 | Nope, we're going to store them here for reference on further playthroughs
			//enemies.Clear();
			
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			if (levelName != "Level_1" && levelName != "Level_2" && levelName != "Level_3" && levelName != "Level_4" && levelName != "Level_5")
				boardScript.SetupScene(level); //TODO ~Z 16.01.30 | Need something here to decide if it's not the first playthrough and recreate the already created levels?
			
			
		} //End.InitGame()
		

		//~Z 16.01.30 | Probably not needed at all - though that depends on how long we load levels, especially on the first pass. Might be useful after all.
		//Hides black image used between levels
		void HideLevelImage()
		{
			Debug.Log ("Disabling cover.");
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);
			
			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		} //End.HideLevelImage()
		
		//Update is called every frame.
		void Update()
		{
			//Check that playersTurn or enemiesMoving or doingSetup are not currently true. //TODO ~Z 16.01.30 | Add start menu detection as well
			if(playersTurn || enemiesMoving || doingSetup)
				
				//If any of these are true, return and do not start MoveEnemies.
				return;
			
			//Start moving enemies.
			StartCoroutine (MoveEnemies ()); //FIXME ~Z 16.01.30 | I'm a bit unsure, doesn't this start n-instances of this Coroutine? Like we only need one?
		} //End.Update()
		
		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//TODO ~Z 16.01.30 | Need to determine which level we are on to know which list to add to.
			//Add Enemy to List enemies.
			enemies.Add(script);
		} //End.AddEnemyToList()
		
		
		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
			//TODO ~Z 16.01.30 | Our level counter is on the far right, so need to ensure the text gets displayed by overflowing to the left
			//					Also need to display different game over messages.
			//Set levelText to display number of levels passed and game over message
			levelText.text = "After " + level + " days, you starved.";
			
			//Enable black background image gameObject.
			levelImage.SetActive(true);


			//Disable this GameManager. ~Z 16.01.30 | Nope, we just start over with fresh character
			//enabled = false;
			//TODO ~Z 16.01.30 | Clear current player and set up a fresh one. If we're using DontDestroyOnLoad on player, need to manage it.
			SceneManager.LoadScene(0); //~Z 16.01.30 | should move to start menu I suppose
		} //End.GameOver()
		
		//Coroutine to move enemies in sequence. //FIXME ~Z 16.01.30 | We neve yield return break; this, so won't it run forever?
		IEnumerator MoveEnemies()
		{
			//While enemiesMoving is true player is unable to move.
			enemiesMoving = true;
			
			//Wait for turnDelay seconds, defaults to .1 (100 ms).
			yield return new WaitForSeconds(turnDelay);
			
			//If there are no enemies spawned (IE in first level):
			if (enemies.Count == 0) 
			{
				//Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
				yield return new WaitForSeconds(turnDelay);
			}
			
			//Loop through List of Enemy objects.
			for (int i = 0; i < enemies.Count; i++)
			{
				//Call the MoveEnemy function of Enemy at index i in the enemies List.
				enemies[i].MoveEnemy ();
				
				//Wait for Enemy's moveTime before moving next Enemy, 
				yield return new WaitForSeconds(enemies[i].moveTime);
			}
			//Once Enemies are done moving, set playersTurn to true so player can move.
			playersTurn = true;
			
			//Enemies are done moving, set enemiesMoving to false.
			enemiesMoving = false;
		} //End.MoveEnemies()

		//~Z 16.01.31 | switch between scenes
		public void ChangeLevel () {
			Debug.Log ("Level: " + level);
			if (level >= 6) //if we're at the end, go to start menu
				level = 0;
			Debug.Log ( "Changing scene to: " + level ); //somehow constantly 0
			SceneManager.LoadScene (level);
		} //End.ChangeLeveL()
	} //End.GameManager{}
} //End.Completed{} - namespace

