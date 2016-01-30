using UnityEngine;
using System.Collections;

namespace Cultist
{	
	public class Loader : MonoBehaviour 
	{
		public GameObject Manager;			//GameManager prefab to instantiate.
		public GameObject soundManager;			//SoundManager prefab to instantiate.
		
		
		void Awake ()
		{
			//Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
			//if (Manager.instance == null)
				
				//Instantiate gameManager prefab
				//Instantiate(Manager);
			
			//Check if a SoundManager has already been assigned to static variable GameManager.instance or if it's still null
			//if (SoundManager.instance == null)
				
				//Instantiate SoundManager prefab
			  //  Instantiate(soundManager);
		}
	}
}