using System;
using System.Collections.Generic;

namespace Race_To_21_UI_Yiran_Du
{
    public static class Game
    {
        public static int numberOfPlayers = 2; // number of players in current game
        public static List<Player> players = new List<Player>(); // list of objects containing player data

        public static List<Player> giveUpPlayers = new List<Player>(); // To keep the data of players who give up
        public static Deck deck = new Deck(); // deck of cards
        public static int currentPlayer = 0; // current player on list
        public static Tasks nextTask; // keeps track of game state
        private static readonly bool cheating = false; // lets you cheat for testing purposes if true
        public static bool Cheating { get { return cheating; } } // Use this to keep "cheating" readonly

        public static int highPoints = 0; // Implementation: Set a variable to keep track the high points (part of Level2)
        public static int pointsToGameOver = 60; // Implementation: Set a variable to determine the score that needs to be reached to end the game

        public static void SetUpGame()
        {
            deck.buildDeck();
            deck.Shuffle();

            /*Console.WriteLine("*****************");
            Console.WriteLine("Do you want to open cheat mode? (Y/N)");
            string response = Console.ReadLine();
            while (true)
            {
                if (response.ToUpper().StartsWith("Y"))
                {
                    cheating = true;
                    return;
                }
                else if (response.ToUpper().StartsWith("N"))
                {
                    cheating = false;
                    return;
                }
                else
                {
                    Console.WriteLine("Please answer Y(es) or N(o)!");
                }
            }*/

            nextTask = Tasks.GetNumberOfPlayers;
        }

        /* Adds a player to the current game
         * Called by DoNextTask() method
         */
        public static void AddPlayer(string n)
        { 
            if (players.Count < numberOfPlayers)
            {
                players.Add(new Player(n));
            }     
        }

        /// <summary>
        /// Calculate the total score of the player's hand.
        /// </summary>
        /// <param name="player">The player's data</param>
        /// <returns>the total score of the player's hand</returns>
        /// Is called by DoNextTask() method
        public static int ScoreHand(Player player)
        {
            int score = 0;

            if (Cheating == true && player.status == PlayerStatus.active)
            {
                string response = null;
                while (int.TryParse(response, out score) == false)
                {
                    Console.Write("OK, what should player " + player.Name + "'s score be?");
                    // response = Console.ReadLine();
                }
                return score;
            }
            else
            {
                foreach (Card card in player.cards)
                {
                    string cardId = card.Id; // Adjust: Because the card's id has encapsulated, so add a local variable to get the data. In this way, we can get the value of the card
                    string faceValue = cardId.Remove(cardId.Length - 1);
                    switch (faceValue)
                    {
                        case "K":
                        case "Q":
                        case "J":
                            score = score + 10;
                            break;
                        case "A":
                            score = score + 1;
                            break;
                        default:
                            score = score + int.Parse(faceValue);
                            break;
                    }
                }
            }
            return score;
        }

        /// <summary>
        /// Check if the status of any players is active
        /// </summary>
        /// <returns>Return the judgment, if there is an active player, it is true, if not, it is false</returns>
        /// Is called by DoNextTask() method
        public static bool CheckActivePlayers()
        {
            // Adjust: When check the first winner, end the game
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.win)
                {
                    return false; // check the first winner
                }
            }

            // Adjust: Returns the end signal if all but one player bust
            int bustPlayerNumber = 0; // Save bust player number

            foreach (var player in players)
            {
                if (player.status == PlayerStatus.bust)
                {
                    bustPlayerNumber++; // If one player bust, plus the bust player number
                }
            }
            // Check if all but one players are bust
            if (bustPlayerNumber == players.Count - 1)
            {
                return false;
            }


            foreach (var player in players)
            {
                if (player.status == PlayerStatus.active)
                {
                    return true; // at least one player is still going!
                }
            }

            return false; // everyone has stayed!
        }


        /// <summary>
        /// Get the winner.
        /// </summary>
        /// <returns>The winner's data</returns>
        /// Is called by DoNextTask() method
        public static Player DoFinalScoring()
        {
            int highScore = 0; // Fix: reset this value
            foreach (var player in players)
            {
                if (player.status == PlayerStatus.win) // someone hit 21
                {
                    return player;
                }
                if (player.status == PlayerStatus.stay) // still could win...
                {
                    if (player.score > highScore)
                    {
                        highScore = player.score;
                    }
                }
                // if busted don't bother checking!
            }

            if (highScore > 0) // someone scored, anyway! And no one bust
            {
                // find the FIRST player in list who meets win condition
                return players.Find(player => player.score == highScore); 
            }

            // Fix: Check if there is a player
            if (players.Find(player => player.status == PlayerStatus.bust) != null)
            {
                return players.Find(player => player.status != PlayerStatus.bust);  // Adjust: if all but one player “busts”, remaining player should immediately win
            }

            // No player draws card
            return null;
        }


        /// <summary>
        /// Shuffle player list
        /// </summary>
        /// Is called by DoNextTask() method
        public static void shufflePlayer()
        {
            Random rng = new Random();

            for (int i = 0; i < players.Count; i++)
            {
                Player tmp = players[i];
                int swapindex = rng.Next(players.Count);
                players[i] = players[swapindex];
                players[swapindex] = tmp;
            }
        }
    }
}
