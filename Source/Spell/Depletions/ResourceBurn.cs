using Newtonsoft.Json;
using System;

namespace Spell
{
    /// <summary>
    /// Burn Rate is by how long it takes to reach max consumption or total depetion.
    /// </summary>
    [System.Serializable]
    public class ResourceBurn : IDepletion
    {
        public static string TypeName => nameof(ResourceBurn);
        [Muttable] public float BurnRate { get; set; }
        public string MainType { get; set; }
        public int ElementIndex { get; set; }

        public ResourceBurn()
        {
            MainType = GetType().Name;
        }

        public IDepletion OnCasting() 
        {
            return this;
        }

        public IDepletion OnTranslation()
        {
            return this;
        }

        public override string ToString()
        {
            string burnData = base.ToString() + " ";
            burnData += $"BurnRate: {BurnRate} | ";
            return burnData;
        }
    }
}