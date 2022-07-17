using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            int Width;
            int Height;
            int EnemiesCount = 3;

            Grid grid;
            GridBox PlayerCurrentLocation;
            Character PlayerCharacter;
            List<Character> EnemyCharacter = new List<Character>();
            List<Character> AllPlayers = new List<Character>();
            List<Character> PlayerAlive = new List<Character>();
            int currentTurn = 0;
            int numberOfPossibleTiles;

            Setup();
            #region Setup
            void Setup()
            {
                SetupGame();
            }
            void SetupGame()
            {
                Console.Clear();
                Console.WriteLine("Configure Table Sizes:\n");
                Console.WriteLine("Width:\n");

                string width = Console.ReadLine();
                if(!Int32.TryParse(width, out Width)) {
                    SetupGame();
                }

                Console.Clear();
                Console.WriteLine("Configure Table Sizes:\n");
                Console.WriteLine("Height:\n");

                string height = Console.ReadLine();
                if (!Int32.TryParse(height, out Height)) {
                    SetupGame();
                }

                SetupGrid();
            }

            void SetupGrid()
            {
                Console.Clear();
                grid = new Grid(Width, Height);
                numberOfPossibleTiles = grid.grids.Count;
                grid.drawBattlefield(Height, Width, grid);

                Console.WriteLine("Confirm Table: (Y/N)\n");
                string confirm = Console.ReadLine();
                switch (confirm)
                {
                    case "Y": case "y":
                        GetPlayerChoice();
                        break;
                    case "N": case "n":
                        SetupGame();
                        break;
                    default:
                        SetupGrid();
                        break;
                }
            }

            void GetPlayerChoice()
            {
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose Between One of this Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                //store the player choice in a variable
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }
            }
            #endregion

            #region Create Characters
            void CreatePlayerCharacter(int classIndex)
            {
               
                CharacterClass characterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {characterClass}");
                PlayerCharacter = new Character(characterClass);
                PlayerCharacter.Team = (Team)1;
                PlayerCharacter.Health = 100;
                PlayerCharacter.BaseDamage = 4;
                PlayerCharacter.PlayerIndex = 0;
                
                CreateEnemyCharacter();

            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
                for(int i = 0; i < EnemiesCount; i++)
                {
                    var rand = new Random();
                    int randomInteger = rand.Next(1, 4);

                    CharacterClass enemyClass = (CharacterClass)randomInteger;
                    Console.WriteLine($"Enemy Class Choice: {enemyClass}");
                    Character enemyCharacter = new Character(enemyClass);
                    enemyCharacter.Team = (Team)2;
                    enemyCharacter.Health = 100;
                    enemyCharacter.BaseDamage = 4;
                    enemyCharacter.PlayerIndex = 1 + i;

                    EnemyCharacter.Add(enemyCharacter);
                }
                StartGame();
            }
            #endregion

            #region Gameplay Turns
            void StartGame()
            {
                //populates the character variables and targets
                var rand = new Random();
                PlayerCharacter.Target = EnemyCharacter[rand.Next(0, EnemyCharacter.Count)];
                
                AllPlayers.Add(PlayerCharacter);
                foreach(Character character in EnemyCharacter) {
                    character.Target = AllPlayers[AllPlayers.Count - 1];
                    AllPlayers.Add(character);
                }

                AlocatePlayers();
                StartTurn();

            }

            List<Character> CharacterSort(List<Character> oldList)
            {
                List<Character> newList = new List<Character>();
                var rand = new Random();
                while(oldList.Count > 0)
                {
                    int id = rand.Next(0, oldList.Count);
                    newList.Add(oldList[id]);
                    oldList.RemoveAt(id);
                }
                return newList;
            }

            void StartTurn(){
                var rand = new Random();

                if (currentTurn == 0)
                {
                    AllPlayers = CharacterSort(AllPlayers);
                }

                Console.Clear();
                int alives = CheckAlivePlayers();
                Console.WriteLine($"Players Vivos {alives}");
                foreach (Character alive in PlayerAlive)
                    Console.WriteLine($"Player {alive.PlayerIndex} HP {alive.Health} Target Player {alive.Target.PlayerIndex} TargetLife {alive.Target.Health}");

                foreach (Character character in AllPlayers)
                {
                    if(character.Health > 0) {
                        if(character.Target.Health > 0) {
                            character.StartTurn(grid);
                        }
                        else {
                            if (PlayerAlive.Count > 1)
                            {
                                character.Target = ChangeTarget(character);
                            }
                        }
                    }
                }

                currentTurn++;
                HandleTurn();
            }

            Character ChangeTarget(Character character)
            {
                foreach(Character alive in PlayerAlive) {
                    if (alive == character)
                        continue;
                    return alive;

                }
                return null;
            }
            int CheckAlivePlayers()
            {
                PlayerAlive.Clear();
                foreach (Character aliveCharacter in AllPlayers)
                {
                    if (aliveCharacter.Health > 0)
                        PlayerAlive.Add(aliveCharacter);
                }
                return PlayerAlive.Count;
            }

            void HandleTurn()
            {


                if(PlayerCharacter.Health <= 0)
                {
                    Console.Clear();
                    Console.WriteLine(" __   __  _______  __   __    ___      _______  _______  _______ ");                                       
                    Console.WriteLine("|  | |  ||       ||  | |  |  |   |    |       ||       ||       |");                                       
                    Console.WriteLine("|  |_|  ||   _   ||  | |  |  |   |    |   _   ||  _____||    ___|");                                       
                    Console.WriteLine("|       ||  | |  ||  |_|  |  |   |    |  | |  || |_____ |   |___ ");                                       
                    Console.WriteLine("|_     _||  |_|  ||       |  |   |___ |  |_|  ||_____  ||    ___|");                                       
                    Console.WriteLine("  |   |  |       ||       |  |       ||       | _____| ||   |___ ");                                       
                    Console.WriteLine("  |___|  |_______||_______|  |_______||_______||_______||_______|");                                       
                    return;
                } 
                else if (PlayerAlive.Count == 1)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    Console.Clear();
                    Console.WriteLine(" __   __  _______  __   __    _     _  ___   __    _ ");
                    Console.WriteLine("|  | |  ||       ||  | |  |  | | _ | ||   | |  |  | |");
                    Console.WriteLine("|  |_|  ||   _   ||  | |  |  | || || ||   | |   |_| |");
                    Console.WriteLine("|       ||  | |  ||  |_|  |  |       ||   | |       |");
                    Console.WriteLine("|_     _||  |_|  ||       |  |       ||   | |  _    |");
                    Console.WriteLine("  |   |  |       ||       |  |   _   ||   | | | |   |");
                    Console.WriteLine("  |___|  |_______||_______|  |__| |__||___| |_|  |__|");

                    Console.Write(Environment.NewLine + Environment.NewLine);

                    return;
                } else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }
            #endregion

            #region Allocate Characters
            void AlocatePlayers()
            {
                foreach (Character character in AllPlayers)
                    AllocatePlayer(character);

                grid.drawBattlefield(Height, Width, grid);
            }

            void AllocatePlayer(Character character)
            {
                int random = GetRandomInt(0, grid.grids.Count - 1);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    RandomLocation.team = character.Team;
                    grid.grids[random] = RandomLocation;
                    character.currentBox = grid.grids[random];
                }
                else
                {
                    AllocatePlayer(character);
                }
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                int index = rand.Next(min, max);
                return index;
            }
            #endregion
        }
    }
}
