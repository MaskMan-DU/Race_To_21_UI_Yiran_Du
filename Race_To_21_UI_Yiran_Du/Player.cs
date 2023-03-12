using System;
using System.Collections.Generic;

namespace Race_To_21_UI_Yiran_Du
{
	public class Player
	{
		public string name; // To store player's name
		public List<Card> cards = new List<Card>(); // To store the player's hands
		public PlayerStatus status = PlayerStatus.active; // Keep the player's status
		public int score; // Keep the score of the player's hands
		public int points = 0; // To store the Points the player won
		public bool isContinue = true;

		public string Name 
		{
			get { return name; }

			set { name = value; }
		} 


		public Player(string n)
		{
			//name = n;
			name = n;
        }

		/* Introduces player by name
		 * Called by CardTable object
		 */
		public void Introduce(int playerNum)
		{
			Console.WriteLine("Hello, my name is " + name + " and I am player #" + playerNum);
		}
	}
}

