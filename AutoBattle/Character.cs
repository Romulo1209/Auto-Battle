using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Character
    {
        public Team Team;
        public string Name { get; set; }
        public float Health;
        public float BaseDamage;
        public float DamageMultiplier { get; set; }
        public GridBox currentBox;
        public int PlayerIndex;
        public Character Target { get; set; } 
        public Character(CharacterClass characterClass)
        {

        }


        public bool TakeDamage(float amount)
        {
            if ((Health -= (BaseDamage * amount)) <= 0)
            {
                Die();
                return true;
            }
            return false;
        }

        public void Die()
        {
            //TODO >> maybe kill him?
            Console.WriteLine("Morreu");
            Health = 0;
        }

        public void StartTurn(Grid battlefield)
        {

            if (CheckCloseTargets(battlefield)) 
            {
                Attack(Target);
                return;
            }
            else
            {   // if there is no target close enough, calculates in wich direction this character should move to be closer to a possible target
                //Console.WriteLine($"Minha posição X {currentBox.xIndex}, posição do inimigo X {Target.currentBox.xIndex}");
                //Console.WriteLine($"Minha posição Y {currentBox.yIndex}, posição do inimigo Y {Target.currentBox.yIndex}");
                if (this.currentBox.xIndex > Target.currentBox.xIndex)
                {
                    Console.WriteLine("Posição X Maior");

                    if ((battlefield.grids.Exists(x => x.Index == currentBox.Index - 1)))
                    {
                        currentBox.team = (Team)0;
                        currentBox.ocupied = false;
                        battlefield.grids[currentBox.Index] = currentBox;
                        currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1));
                        currentBox.ocupied = true;
                        currentBox.team = Team;
                        battlefield.grids[currentBox.Index] = currentBox;
                        Console.WriteLine($"Player {PlayerIndex} walked left\n");
                        battlefield.drawBattlefield(battlefield.yLength, battlefield.xLenght, battlefield, Team);
                        return;
                    }
                } 
                else if(currentBox.xIndex < Target.currentBox.xIndex)
                {
                    Console.WriteLine("Posição X Menor");

                    currentBox.team = (Team)0;
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1));
                    currentBox.ocupied = true;
                    currentBox.team = Team;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked right\n");
                    battlefield.drawBattlefield(battlefield.yLength, battlefield.xLenght , battlefield, Team);
                    return;
                }

                if (currentBox.yIndex > Target.currentBox.yIndex)
                {
                    Console.WriteLine("Posição Y Maior");

                    currentBox.team = (Team)0;
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght));
                    currentBox.ocupied = true;
                    currentBox.team = Team;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked up\n");
                    battlefield.drawBattlefield(battlefield.yLength, battlefield.xLenght, battlefield, Team);
                    return;
                }
                else if(currentBox.yIndex < Target.currentBox.yIndex)
                {
                    Console.WriteLine("Posição Y Menor");

                    currentBox.team = (Team)0;
                    currentBox.ocupied = false;
                    battlefield.grids[currentBox.Index] = this.currentBox;
                    currentBox = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght));
                    currentBox.ocupied = true;
                    currentBox.team = Team;
                    battlefield.grids[currentBox.Index] = currentBox;
                    Console.WriteLine($"Player {PlayerIndex} walked down\n");
                    battlefield.drawBattlefield(battlefield.yLength, battlefield.xLenght, battlefield, Team);
                    return;
                }
            }
        }

        // Check in x and y directions if there is any character close enough to be a target.
        bool CheckCloseTargets(Grid battlefield)
        {
            bool left = (battlefield.grids.Find(x => x.Index == currentBox.Index - 1).ocupied);
            bool right = (battlefield.grids.Find(x => x.Index == currentBox.Index + 1).ocupied);
            bool up = (battlefield.grids.Find(x => x.Index == currentBox.Index + battlefield.xLenght).ocupied);
            bool down = (battlefield.grids.Find(x => x.Index == currentBox.Index - battlefield.xLenght).ocupied);

            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine($"Left {left}, Right {right}, Up {up}, Down {down}");
            //Console.ForegroundColor = ConsoleColor.White;

            if (left || right || up || down) 
            {
                return true;
            }
            return false; 
        }

        public void Attack (Character target)
        {
            var rand = new Random();
            DamageMultiplier = rand.Next(0, 5);
            target.TakeDamage(DamageMultiplier);

            Console.WriteLine(BaseDamage);
            Console.WriteLine(DamageMultiplier);

            //Visual
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"{Team} Attack Turn");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"Player {PlayerIndex} is attacking the player {Target.PlayerIndex}\n");
            Console.WriteLine($"Player {target.PlayerIndex} recieved {BaseDamage * DamageMultiplier} DAMAGE");
            Console.WriteLine($"Player {target.PlayerIndex} Total Life {target.Health} HP");
            Console.WriteLine("-----------------------------------------------");
        }
    }
}
