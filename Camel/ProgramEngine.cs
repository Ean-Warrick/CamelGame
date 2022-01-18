namespace Camel
{
    class ProgramEngine
    {
        private readonly int FINISH = 200;
        private readonly int MAXIMUM_DRINKS = 3;
        private int playerPosition;
        private int enemyPosition;
        private int drinks;
        private int tiredness;
        private int thirst;
        private bool done;
   
        Random randomNumberGenerator;
        Dictionary<string, Func<bool>> commandMap;
        List<Func<bool>> gameStateCheckArray;

        public ProgramEngine()
        {
            done = false;
            drinks = this.MAXIMUM_DRINKS;
            enemyPosition = -20;
            playerPosition = 0;
            tiredness = 0;
            thirst = 0;
            randomNumberGenerator = new Random();
            commandMap = new Dictionary<string, Func<bool>>();
            gameStateCheckArray = new List<Func<bool>>();

            SetUpCommandMap();
            SetUpGameStateCheckArray();
        }

        // Main loop function. Gets called to start the game.
        // Returns a bool that represents if the player wants to retry/play again.
        public void Start()
        {
            while (!done)
            {
                bool proccessed = false;
                while (!proccessed)
                {
                    Directions();
                    string userCommand = GetCommand().ToUpper();
                    
                    if (commandMap.ContainsKey(userCommand))
                    {
                        proccessed = commandMap[userCommand]();  
                    }
                    else
                    {
                        Console.WriteLine("Not a valid command!");
                    }
                }

                if (!done)
                {
                    enemyPosition += randomNumberGenerator.Next(7, 14);
                    foreach (Func<bool> func in gameStateCheckArray)
                    {
                        if (func())
                        {
                            done = true;
                            return;
                        }
                    }
                }
            }
        }

        // Maps keys to commands.
        private void SetUpCommandMap()
        {
            commandMap.Add("A", DrinkFromCanteen);
            commandMap.Add("B", AheadModerateSpeed);
            commandMap.Add("C", AheadFullSpeed);
            commandMap.Add("D", StopAndRest);
            commandMap.Add("E", CheckStatus);
            commandMap.Add("Q", Quit);
        }

        // Adds game state checks to array (order of which they are added matters).
        private void SetUpGameStateCheckArray()
        {
            gameStateCheckArray.Add(CheckForCamelTiredness);
            gameStateCheckArray.Add(CheckForThirst);
            gameStateCheckArray.Add(CheckForNatives);
            gameStateCheckArray.Add(CheckForWin);
        }

        // Prints the directions before every player turn.
        private void Directions()
        {
            Console.WriteLine();
            Console.WriteLine("A. Drink from your canteen.");
            Console.WriteLine("B. Ahead moderate speed.");
            Console.WriteLine("C. Ahead full speed.");
            Console.WriteLine("D. Stop and rest.");
            Console.WriteLine("E. Status check.");
            Console.WriteLine("Q. Quit.");
        }

        // Returns the key players press to request a command.
        private string GetCommand()
        {
            Console.Write("What is your command? ");
            string? userCommand = Console.ReadLine();
            Console.WriteLine();
            if (userCommand == null) { return "NULL"; } else { return userCommand; }
        }

        // Moves the player's position given a distance input.
        private bool PlayerTravel(int distance)
        {
            playerPosition += distance;
            thirst += 1;
            if (thirst <= 6)
            {
                Console.WriteLine($"You traveled {distance} miles.");
            }
            return true;
        }

        // COMMAND FUNCTIONS //
        // Functions perform a defined action. 
        // Returns a bool representing if the action should end the players turn or not.
        private bool DrinkFromCanteen()
        {
            if (drinks > 0)
            {
                Console.WriteLine("You drink from your canteen!");
                drinks -= 1;
                thirst = 0;
                return true;
            } 
            else
            {
                Console.WriteLine("Your canteen is empty!");
                return false;
            }
        }

        private bool AheadModerateSpeed()
        {
            PlayerTravel(randomNumberGenerator.Next(5, 12));
            tiredness += 1;
            return true;
        }

        private bool AheadFullSpeed()
        {
            PlayerTravel(randomNumberGenerator.Next(10, 20));
            tiredness += randomNumberGenerator.Next(1, 3);
            return true;
        }

        private bool StopAndRest()
        {
            Console.WriteLine("You stop and rest!");
            tiredness = 0;
            return true;
        }

        private bool Quit()
        {
            Console.WriteLine("Quitter!");
            done = true;
            return true;
        }

        // GAME END CHECK FUNCTIONS //
        // Functions check if the game should end with specific conditions.
        // Returns a bool represents if the game should end or not.
        private bool CheckStatus()
        {
            Console.WriteLine($"Miles Travled: {playerPosition}");
            Console.WriteLine($"Drinks in canteen: {drinks}");
            Console.WriteLine($"The natives are {playerPosition - enemyPosition} miles behind you.");
            return false;
        }

        private bool CheckForCamelTiredness()
        {
            if (tiredness > 8)
            {
                System.Console.WriteLine("Your camel died from tiredness and you were caught!");
                return true;
            }
            else if (tiredness > 5)
            {
                System.Console.WriteLine("Your camel is getting tired.");
            }
            return false;
        }

        private bool CheckForNatives()
        {
            if (playerPosition - enemyPosition <= 0 && !(playerPosition >= FINISH)) 
            {
                System.Console.WriteLine("You were caught by the natives!");
                return true;
            }
            else if (playerPosition - enemyPosition < 15 && !(playerPosition >= FINISH))
            {
                System.Console.WriteLine("The natives are getting close!");
            }
            return false;
        }

        private bool CheckForThirst()
        {
            if (thirst > 6)
            {
                System.Console.WriteLine("You died from thirst!");
                return true;
            } 
            else if (thirst > 4)
            {
                System.Console.WriteLine("You are getting thirsty!");
            }
            return false;
        }

        private bool CheckForWin()
        {
            if (playerPosition >= FINISH)
            {
                Console.WriteLine("You made it accross the desert! You won!");
                return true;
            }
            return false;
        }

        private bool CheckForOasis()
        {
            if (randomNumberGenerator.Next(1, 20) == 10)
            {
                Console.WriteLine("You found an oasis! You are now hydrated, fully rested, and have a full canteen!");
                thirst = 0;
                tiredness = 0;
                drinks = this.MAXIMUM_DRINKS;
            }
            return false;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Camel!");
            Console.WriteLine("You stole a camel, and are now getting chased by natives through the mobi desert.");
            Console.WriteLine("Your goal is to travel across the desert without getting caught!");
           
            ProgramEngine camelGame = new ProgramEngine();
            camelGame.Start();
            
            Console.WriteLine();
            Console.WriteLine("Thanks for playing! Bye!");
        }
    }
}