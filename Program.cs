using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using System.Media;
namespace The_Game_of_Cheese_Mining
{
    class Program
    {
        static string NumOfPlayersSTR;
        static int NoP;
        public static int[,] CheeseLoc = new int[8, 8];
        static int[,] board = new int[8, 8];
        static int originalLocation;
        static Players[] Player = new Players[4];
        public static bool SixPower;
        public static int AttackerIndex; //The attackers index is used to identify the player. 
        public static int P1CordX;
        public static int P1CordY;
        public static int P2CordX;
        public static int P2CordY;
        public static int P3CordX;
        public static int P3CordY;
        public static int p4CordX;
        public static int p4CordY;
    
        static void Main(string[] args)
        {
            Console.SetWindowSize(50, 33);
          //  introToGame();
            StartGame();
        }

    
        #region "Start and End of game"
        static void StartGame()
        {
            Console.ForegroundColor = ConsoleColor.White;
            playerNum(prompt: "How many players? (Min 1 Max 4)", min: 1, max: 4);
            SettingCheeseMethod();
           
            int pCount = 0;
            int pCount2 = 1;
            Random r = new Random();
            int diceNum;

            for (int i = 0; i < NoP; i++)
            {
                pCount += 1;
                Player[i].Score = 0;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nPlayer " + pCount + "'s Name:");
                Console.ForegroundColor = ConsoleColor.White;
                Player[i].Name = Console.ReadLine();
            }
            playerIcon();
            Console.Clear();
            Console.WriteLine("Use the arrow keys to navigate around the board.");
            Thread.Sleep(1000);
            Player[0].X = 0;
            Player[0].Y = 0;
            Player[1].X = 7;
            Player[1].Y = 0;
            Player[2].X = 7;
            Player[2].Y = 7;
            Player[3].X = 0;
            Player[3].Y = 7;
            int turn = 0;

            while (true)
            {
                turn += 1;
                Console.Write("_________________________________________________________________________\n");
                Console.Write("Turn " + turn + " :");
                Console.Write("\n--------------------------------------------------------------------------------\n");
                for (int b = 0; b < NoP; b++)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("{0} is on {2}X,{3}Y with {1} points.", Player[b].Name, Player[b].Score, Player[b].X, Player[b].Y);
                    Console.WriteLine();
                }

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n_________________________________________________________________________\n");
                SetPlayers();
                PrintBoard();

                for (int i = 0; i < NoP; i++)
                {
                    AttackerIndex = i;
                    diceNum = r.Next(1, 7);
                    Console.Write("_________________________________________________________________________\n");
                    Console.WriteLine(Player[i].Name + "'s turn, Rolled: " + diceNum + ".");
                    
                    pCount2 += 1;
                    if(diceNum == 6) //Checks to see if 6 has been rolled. If so ask player if the want to use the ability
                     sixPower(TPindex: i);
                   if(diceNum <6|| SixPower ==false )
                    {

                    for (int k = 0; k < 1; k++)
                    {

                        var key = Console.ReadKey().Key;
                        Console.WriteLine();
                        switch (key)
                        {
                            case ConsoleKey.DownArrow: //Down
                                movePlayer(direction: 'D', spaces: diceNum, index: i);
                                break;

                            case ConsoleKey.UpArrow: //Up
                                movePlayer(direction: 'U', spaces: diceNum, index: i);

                                break;

                            case ConsoleKey.LeftArrow: //Left
                                movePlayer(direction: 'L', spaces: diceNum, index: i);


                                break;

                            case ConsoleKey.RightArrow: //Right
                               
                                movePlayer(direction: 'R', spaces: diceNum, index: i);
                                //testPlayersOnSquare();
                                break;
                            default:
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Use the arrow keys to navigate.");
                                Console.ForegroundColor = ConsoleColor.White;
                                k--;
                                continue;
                        }
                    }
                    }

                    PlayersOnSquare(Player[i].X, Player[i].Y);

                    if (CheeseLoc[Player[i].X, Player[i].Y] == 2) //Detect if where the player is indicated as a 2. Which means that the player is on a cheese square. 
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine(Player[i].Name + " collected a piece of cheese.");
                        (new SoundPlayer(Properties.Resources.cheesePickup)).Play();
                        Console.ForegroundColor = ConsoleColor.White;

                        Player[i].Score += 1;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("{1}'s New Score: {0}", Player[i].Score, Player[i].Name);
                        Console.ForegroundColor = ConsoleColor.White;
                        CheeseLoc[Player[i].X, Player[i].Y] = 0; // the piece of cheese has been collected, so remove from board. 

                    }
                    if (Player[i].Score > 5) //Set to detect greater than 5 just incase player jumps to a 7. 
                    {
                        while (true)
                        {

                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("{0} Wins the game!", Player[i].Name);
                            Console.ForegroundColor = ConsoleColor.White;
                            EndOfGame();
                            return;
                        }

                    }
                    SetPlayers();
                    PrintBoard();

                }
                Console.WriteLine();
            }
        }
        static private void EndOfGame() //would read the users input and then take action according to what the player has chosen to do. 
        {
            while (true)
            {

                Console.WriteLine("Enter R to restart.\nEnter E to close the game.");
                string decision;
                decision = Console.ReadLine();
                switch (decision)
                {
                    case "r":
                        for (int i = 0; i < NoP; i++)
                        {
                            Player[i].Score = 0;
                        }

                        for (int j = 0; j < 8; j++)
                        {
                            for (int k = 0; k < 8; k++)
                            {
                                CheeseLoc[j, k] = 0;


                            }
                        }
                        originalLocation = 0;
                        resetBoard();
                        Console.Clear();
                        StartGame();
                        return;
                    case "e":
                        Environment.Exit(0);
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid key.");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;

                }
            }


        }
        #endregion
    
        #region "Intro To Game"
        public static void introToGame()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Welcome");
            Thread.Sleep(500);
            Console.WriteLine("To");
            Thread.Sleep(500);
            Console.WriteLine("The");
            Thread.Sleep(500);
            Console.WriteLine("Cheese Mining Game!\n");
            PrintImage2();
            Console.Clear();
        }
        private static void PrintImage2()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.Write(@"
                        ,;`                       
                       ':;#                       
                       :#@'                       
                       #;'@   ");
            Thread.Sleep(300);
            Console.Write(@"
                       ;+:                       
                      ;'''+@`                     
                     :;'':`'@                     
                     ;'+++:`'@");
            Thread.Sleep(300);
            Console.Write(@"
                    #'';'' ``#                    
                    ;''''',  ;'                   
                   :;++';;;,: @                   
                   @''';;;;   @ ");
            Thread.Sleep(300);
            Console.Write(@"
                   @;;';;';  ;+                   
                   @+++ ';,.;++                   
                   ##@@#++##@;:                   
                   #:::::::`  ,");
            Thread.Sleep(300);
            Console.Write(@"
                   #:::'';+`  `                   
                   #:::'#@''  ``                  
                   @:::'#@''  ``                  
                   @:::'''+:  ``");
            Thread.Sleep(300);
            Console.Write(@"
                   @::::,:::  .`                  
                   @::::'+::  .                   
                   #:::#'';:  ,                   
                   +:::'@@'+  ;");
            Thread.Sleep(300);
            Console.Write(@"
                    ::::++@;;  '                   
                   .::::+;+:  #                   
                    ;:::::::  @                   
                    +:::+'':  @ ");
            Thread.Sleep(300);
            Console.Write(@"
                    @::++#':  ;                   
                    @::'#@'; ..                   
                   `#';+'+'`;#                    
                    :'#:++; #':     ");
            Thread.Sleep(300);
            Console.Write(@"
                    +'#:::: ''@                   
                    ''':::: ;''                   
                   .;''::::,;;;                   
                  `+'''+:::+'''; ");
            Thread.Sleep(300);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(@"
                 +@#;;'@::.+'''@+@                
                 '+';;'+:: ;'';#'+                
                 '';;;#@::.+@;;+;;                
                .'';':`':; # @;+';");
            Thread.Sleep(300);
            Console.Write(@"
                :''''  @::..  '+';`               
                ;''+,  #:,;   @#''`               
                '';'    +.@   ,#';.               
                '';#    @,#    #';.");
            Thread.Sleep(300);
            Console.Write(@"
                ;';:    +##    @';`               
                ,;;     ';+    ;''                
                 ''`    #':     '+                
                 '#     @'`    `'#     ");

            Thread.Sleep(300);

            for (int i = 0; i < 12; i++)
            {
                Console.Write("\n\n");
                Thread.Sleep(300);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        #endregion
        #region "Player Info"
  private static void playerIcon()
        {
      for(int i = 0; i<NoP; i++)
      {
          int pNum = i+1;
          Console.ForegroundColor = ConsoleColor.Magenta;
          Console.WriteLine("\nPlayer {0}'s icon: ", pNum);
          Player[i].icon = Console.ReadKey().KeyChar;
          Console.ForegroundColor = ConsoleColor.White;

      }
      
  }
        
        
        struct Players
        {
            public int X;
            public int Y;

            public int Score;
            public string Name;
            public char icon;
        }
        private static string playerNum(string prompt, int min, int max)
        {
         bool ValidInput = false;

            while (ValidInput == false)
            {
                int maxNum = max;
                int MinNum = min;
                Console.WriteLine(prompt);
                NumOfPlayersSTR = Console.ReadLine();
                switch (NumOfPlayersSTR)
                {
                    case "1":
                        NoP = 1;
                        ValidInput = true;
                        break;
                    case "2":
                        NoP = 2;
                        ValidInput = true;
                        break;
                    case "3":
                        NoP = 3;
                        ValidInput = true;
                        break;
                    case "4":
                        ValidInput = true;
                        NoP = 4;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Input must be digits between 1 and 4.");
                        Console.ForegroundColor = ConsoleColor.White;
                        ValidInput = false;
                        break;
                }
            }
            return null;
        }
        #endregion
        #region "Player Movement"
        /// <summary>
        /// Player rolled a 6. They can have the choice to use a cheese worm hole to teleport to another player. 
        /// </summary>
        /// <param name="TplayerIndex">Index of the player that is teleporting.</param>
        /// 

        /// <returns></returns>
        private static int sixPower(int TPindex)
        {
            bool validInput = false;
            while (validInput == false)
            {
                string input;
                string pName;

                Console.WriteLine("Use a cheese worm hole?(Y/N)");
                input = Console.ReadLine();
                if (input == "y")
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("Who should {0} teleport to? ", Player[TPindex].Name);
                    Console.ForegroundColor = ConsoleColor.White;

                    for (int i = 0; i < NoP; i++)
                    {
                        if (i != TPindex)
                        {
                            Console.WriteLine(Player[i].Name);

                        }
                    }

                    pName = Console.ReadLine();

                    for (int j = 0; j < NoP; j++)
                    {
                        if (j != TPindex)
                        {
                            if (Player[j].Name == pName)
                            {
                                resetBoard();
                                Player[TPindex].X = Player[j].X;
                                Player[TPindex].Y = Player[j].Y;
                                validInput = true;
                                SixPower = true;

                                //   PrintBoard();
                                //   break;
                            }
                        }

                    }

                    /*  Console.ForegroundColor = ConsoleColor.Red;
                      Console.WriteLine("Player does not exist.\nCannot be teleported to.");
                      Console.ForegroundColor = ConsoleColor.White;*/

                }
                else if (input == "n")
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("Worm Hole not used, resume with move.");
                    Console.ForegroundColor = ConsoleColor.White;
                    
                    validInput = true;

                    SixPower = false;
                }
                else
                {
                    Console.WriteLine("y for yes, n for no.");
                }
            }


            //six power




            return 0;
        }
       
        static private int movePlayer(char direction, int spaces, int index)
        {


            switch (direction)
            {
                case 'U':
                    
                    Player[index].X -= spaces;
                                resetBoard();
                                if (Player[index].X < 0) //checks if ends up below the playing board. 
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(Player[index].Name + " warped from above the map!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Player[index].X += 8;
                                }
                    break;

                case 'D':
                    Player[index].X += spaces;
                    resetBoard();
                    if (Player[index].X > 7) //Checks to see if the player left the board. 
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(Player[index].Name + " warped from below the map!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Player[index].X -= 8;
                    }
                    break;

                case 'L':
                    Player[index].Y -= spaces;
                                resetBoard();
                                if (Player[index].Y < 0) //Checks to see if the player left the board. 
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(Player[index].Name + " warped from across the map!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Player[index].Y += 8;
                                }
                    break;
                case 'R':
                    Player[index].Y += spaces;
                                resetBoard();

                                if (Player[index].Y > 7) //Checks to see if the player left the board. 
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine(Player[index].Name + " warped across the map!");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Player[index].Y -= 8;
                                }


                    break;
            }
            return 0;
        }


        #endregion

        #region "Tests"
        private static void testPlayersOnSquare()
        {
            for (int i = 1; i < NoP; i++)
            {
                Player[i].X = 3;
                Player[i].Y = 3;
            }
        }


        #endregion
        #region "Placing Cheese and On The Board"
        public static int NumOfCheese = 1;
        private static void setCheeseOnBoard() //Void is started at the begining of the program. 
        {
            int cheeseTurns = 0;
            if (NoP == 1)
            {
                cheeseTurns = 16;
            }
            else if (NoP == 2)
            {
                cheeseTurns = 8;
            }
            else if (NoP == 3)
            {
                cheeseTurns = 6;
            }
            else if (NoP == 4)
            {
                cheeseTurns = 4;

            }
            int playerTurn = 1;
            for (int c = 0; c < NoP; c++)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Player " + playerTurn + "'s turn!");
                Console.ForegroundColor = ConsoleColor.White;
                for (int i = 1; i < cheeseTurns; i++)
                {
                    Console.WriteLine("X location of piece " + i + ".");

                    int x = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Y location of piece " + i + ".");
                    int y = Convert.ToInt32(Console.ReadLine());

                    try
                    {
                        //  CheeseLoc[x, y] = 2;
                        if (x == 0 && y == 0 || x == 7 && y == 0 || x == 7 && y == 7 || x == 0 && y == 7)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid Cheese Possition.");
                            Console.ForegroundColor = ConsoleColor.White;

                            i--;
                        }

                        if (Original(x, y) == true)
                        {
                            Original(x, y);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Cheese Space occupied! Try again");
                            Console.ForegroundColor = ConsoleColor.White;
                            i--;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("The Cheese was lost in space... Try again");
                        i--;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                playerTurn += 1;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Space now has the cheese!");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(2000);
            Console.Clear();
        }
   
        
        public static void SettingCheeseMethod()
        {
            bool valid = false;
            string choice;
            while (valid == false)
            {

                Console.WriteLine("Auto Set The Cheese? (Y/N)");
                choice = Console.ReadLine();

                switch (choice)
                {
                    case "y":
                        Console.WriteLine("Auto Set The Cheese.");
                        valid = true;
                        AutoCheese();


                        break;
                    case "n":
                        Console.WriteLine("Input Your Cheese Locations.");
                        valid = true;

                        setCheeseOnBoard();
                        break;
                    case "Y":

                        Console.WriteLine("Auto Set The Cheese.");
                        valid = true;

                        AutoCheese();

                        break;
                    case "N":
                        Console.WriteLine("Input Your Cheese Locations.");
                        valid = true;
                        setCheeseOnBoard();
                        break;
                    default:
                        valid = false;
                        break;
                }
            }
        }


        public static bool Original(int x, int y) //
        {
            if (CheeseLoc[x, y] == 2)
            {
                return false; //Cheese is not original
            }
            else
            {
                CheeseLoc[x, y] = 2;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Piece " + NumOfCheese + " Set.");
                Console.ForegroundColor = ConsoleColor.White;
                NumOfCheese += 1;
                return true; //Cheese is original 
            }
        }

        public static void AutoCheese()
        {
            do
            {
                Random r = new Random();
                int x = r.Next(0, 7);
                int y = r.Next(0, 7);
                if (x == 0 && y == 0 || x == 7 && y == 0 || x == 7 && y == 7 || x == 0 && y == 7)
                {
                    //Do nothing as the cheese is illegal on these possitions
                }

                else
                {
                    if (CheeseLoc[x, y] == 0)
                    {

                        CheeseLoc[x, y] = 2;

                        originalLocation += 1;

                        Console.WriteLine("Cheese set on: Y {0} X {1} ", x, y);

                    }
                }
            }
            while (originalLocation != 16);
        }
        #endregion


        #region "Player on Player and Battles"


        private static int PlayersOnSquare(int x, int y)
        {

            bool listed = false;

            List<int> Indexes = new List<int>();
            int total = 0;
            for (int i = 0; i < NoP; i++)
            {
                if (Player[i].X == x && Player[i].Y == y)
                {
                    total += 1;
                    Indexes.Add(i);
                }
            }
            total -= 1;
            Indexes.Remove(AttackerIndex);

            for (int i = 0; i < NoP; i++)
            {
                if (Player[i].X == x && Player[i].Y == y)
                {
                    if (total == 1)
                    {
                        if (AttackerIndex != i) //One on one cheese stealing battles
                        {
                            Console.WriteLine("{1} is on top of {0} ", Player[i].Name, Player[AttackerIndex].Name);  // i is the index of the defending player

                            if (Player[i].Score == 0) //The rocket has no cheese so the turn will end. 
                            {
                                Console.WriteLine("opponent {0} has no cheese! No reason to battle!", Player[i].Name);
                            }
                            else
                            {

                            OneOnOne(i, AttackerIndex);   // attacker is the index of the attacking player

                            }
                        }
                    }
                }

                //The following code is related to more than 2 players on a single square. 
                if (listed == false)
                {
                    if (total > 1)
                    {
                        string p;
                        Console.ForegroundColor = ConsoleColor.Red;
                        bool validP = false;
                        while(validP == false)
                        {
                        Console.WriteLine("Who should " + Player[AttackerIndex].Name + " battle?");
                        listed = true;
                        Console.ForegroundColor = ConsoleColor.White;
                        foreach (int index in Indexes)
                        {
                            Console.WriteLine(Player[index].Name);
                        }
                        p = Console.ReadLine();

                        foreach (int index in Indexes)
                        {
                            if (p == Player[index].Name)
                            {
                                OneOnOne(index, AttackerIndex);
                             //   Player[AttackerIndex].Score += Player[index].Score;
                                p = null;
                                validP = true;
                            }
                        }
                   
                        Console.ForegroundColor = ConsoleColor.White;
                        
                        }

                    }
                }
            }

            return total;
        }
     
        private static int OneOnOne(int defender, int attacker)
        {
            bool notAdraw = false;
            do
            {

                Random r = new Random();
                int rolledA = r.Next(1, 7);
                int rolledB = r.Next(1, 7);
                Console.WriteLine("Defender {0} rolled a {1}.", Player[defender].Name, rolledA);
                Console.WriteLine("Attacker {0} rolled a {1}.", Player[attacker].Name, rolledB);

                if (rolledA < rolledB) //Scenario where the attacker wins
                {
                    notAdraw = true;

                    if (rolledA == 2) //Poor win, attacker gives the defender a piece of cheese. 
                    {
                       

                        if (Player[attacker].Score > 0) //Checks if the attacker has any cheese to give. 
                        {
                            Player[attacker].Score -= 1;
                            Player[defender].Score += 1;

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Attacker {0} wins, however its a poor win.\n{1}'s new score is: {2}. {3}'s new score is: {4}.", Player[attacker].Name, Player[attacker].Name, Player[attacker].Score, Player[defender].Name, Player[defender].Score);
                            (new SoundPlayer(Properties.Resources.winBattle)).Play();
                         
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else if(Player[attacker].Score == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Attacker {0} wins with a poor win, but defender has no cheese to give", Player[attacker].Name);
                            (new SoundPlayer(Properties.Resources.winBattle)).Play();
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }

                    else if (rolledA == 3)  //honourable defeat
                    {
                        Console.WriteLine("Attacker {0} wins! The defender is honourably defeated.", Player[attacker].Name);
                        (new SoundPlayer(Properties.Resources.winBattle)).Play();
                
                        movePlayer(direction: 'L', spaces: 1, index: defender);

                    }
                    else if (rolledA == 4) //Solid Victory
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;

                        Console.WriteLine("Attacker {0} wins! They are moved adjacent to their current possition.", Player[attacker].Name);
                        (new SoundPlayer(Properties.Resources.winBattle)).Play();
                   
                        movePlayer(direction: 'L', spaces: 1, index: attacker);

                        Console.ForegroundColor = ConsoleColor.White;




                    }
                    else if (rolledA == 5) //a glorious victory
                    {
                        Console.WriteLine("Attacker {0} wins! Its a glorios victory", Player[attacker].Name);
                        (new SoundPlayer(Properties.Resources.winBattle)).Play();
                  
                        movePlayer(direction: 'L', spaces: 1, index: attacker);


                    }
                    else if (rolledA == 6) // an infamous victory.
                    {
                       

                        if (Player[defender].Score > 1) //Checks if the defender has any cheese to give. 
                        {
                           
                            Player[defender].Score -= 2;
                            Player[attacker].Score += 2;

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Attacker {0} wins! It's a infamous victory\n{1}'s new score is: {2}. {3} new score is: {4}.", Player[attacker].Name, Player[attacker].Name, Player[attacker].Score, Player[defender].Name, Player[defender].Score);
                            (new SoundPlayer(Properties.Resources.winBattle)).Play();
                            Console.ForegroundColor = ConsoleColor.White;
                            movePlayer(direction: 'L', spaces: 2, index: attacker);


                        }
                        else if(Player[defender].Score  >= 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Attacker {0} wins! It's a infamous victory but {1} does not have the cheese to give...", Player[attacker].Name, Player[defender].Name);
                            (new SoundPlayer(Properties.Resources.winBattle)).Play();
                         
                            Console.ForegroundColor = ConsoleColor.White;

                        }

                    }


                }

                else if (rolledA > rolledB) //Scenario where the defender wins. 
                {
                    Console.WriteLine("{0} defended themselves! ", Player[defender].Name);
                    (new SoundPlayer(Properties.Resources.winBattle)).Play();

                    notAdraw = true;

                }

                else if (rolledA == rolledB)
                {
                    Console.WriteLine("Both players rolled the same. Its a draw\nRolling again..");
                    notAdraw = false;
                }
            }
            while (notAdraw == false);
            
                return 0;
           

            }
         

        #endregion


       
        #region "Print Board"
        private static void PrintBoard()
        {

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (CheeseLoc[i, j] == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("|C|");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (board[i, j] == 1)
                    {

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("|{0}|", Player[0].icon);
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else if (board[i, j] == 2)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("|{0}|", Player[1].icon);
                        Console.ForegroundColor = ConsoleColor.White;

                    }
                    else if (board[i, j] == 3)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("|{0}|", Player[2].icon);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (board[i, j] == 4)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write("|{0}|", Player[3].icon);
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.Write("| |");
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    //   Console.WriteLine("X is: " + i + " And Y is: " + j);

                }
                Console.Write("\n");
            }
        }

        private static void resetBoard() //Reset the board back to empty. 
        {
            for (int k = 0; k < board.GetLength(0); k++)
            {
                for (int l = 0; l < board.GetLength(1); l++)
                {
                    var val = board[k, l] = 0;
                }
            }
        }
       
        private static void SetPlayers()
        {
            P1CordX = Player[0].X;
            P1CordY = Player[0].Y;
            P2CordX = Player[1].X;
            P2CordY = Player[1].Y;
            P3CordX = Player[2].X;
            P3CordY = Player[2].Y;
            p4CordX = Player[3].X;
            p4CordY = Player[3].Y;

            switch (NoP)
            {
                case 1:
                    board[P1CordX, P1CordY] = 1; //Green Player
                    break;
                case 2:
                    board[P1CordX, P1CordY] = 1; //Green Player
                    board[P2CordX, P2CordY] = 2; //Yellow Player
                    break;
                case 3:
                    board[P1CordX, P1CordY] = 1; //Green Player
                    board[P2CordX, P2CordY] = 2; //Yellow Player
                    board[P3CordX, P3CordY] = 3; // Blue Player


                    break;
                case 4:
                    board[P1CordX, P1CordY] = 1; //Green Player
                    board[P2CordX, P2CordY] = 2; //Yellow Player
                    board[P3CordX, P3CordY] = 3; // Blue Player
                    board[p4CordX, p4CordY] = 4; // purple player
                    break;

            }
        }
        #endregion


    }
}














