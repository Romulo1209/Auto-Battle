using System;
using System.Collections.Generic;
using System.Text;

namespace AutoBattle
{
    public enum Team {None, Player, Enemy }
    public class Types
    {
        public struct CharacterClassSpecific
        {
            CharacterClass CharacterClass;
            float hpModifier;
            float ClassDamage;
            CharacterSkills[] skills;

        }

        public struct GridBox
        {
            public Team team;
            public int xIndex;
            public int yIndex;
            public bool ocupied;
            public int Index;

            public GridBox(Team team, int x, int y, bool ocupied, int index)
            {
                this.team = team;
                xIndex = x;
                yIndex = y;
                this.ocupied = ocupied;
                this.Index = index;
            }

        }

        public struct CharacterSkills
        {
            string Name;
            float damage;
            float damageMultiplier;
        }

        public enum CharacterClass : uint
        {
            Paladin = 1,
            Warrior = 2,
            Cleric = 3,
            Archer = 4
        }

    }
}
