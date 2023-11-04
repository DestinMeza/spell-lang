using System;
using Spell.IO;

namespace Spell
{
    /// <summary>
    /// Magic types are aspects that manipulate spell's core stat values.
    /// </summary>
    public enum EMagicType 
    {
        Air,
        Fire,
        Water,
        Earth,
        Arcane,
        Light,
        Darkness,
        Life,
        Ethereal,
        Astral,
    }

    /// <summary>
    /// A spell is holds alot of data for how an abstract "magic spell" would work. In the game a player will create spells
    /// using a UI menu / IDE in the game similar to crafting in sandbox games. Once a spell is complete the data will be saved to a spel file
    /// to be referenced later within the SpellEntity Script where the properties of the spell will be read and acted upon. This is a prototype
    /// template which will drive how spells all function in the actual simulation.
    /// </summary>
    [Serializable]
    public class Spell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SpellFile SpellLoaderPath { get; set; }

        public Spell() { }

        /// <summary>
        /// This constructor is required for the JSON deserializer to be able
        /// to identify concrete classes to use when deserializing the interface properties.
        /// </summary>

        /// <summary>
        /// This constructor helps with cloning skills. Useful for when a skill is being referenced and we want to preserve Spell.
        /// </summary>
        /// <param name="instance"></param>
        public Spell(Spell instance) 
        {
            Name = instance.Name;
        }

        public override string ToString()
        {
            string spellData = "";
            spellData += $"Name: {Name} | ";
            return spellData;
        }
    }
}