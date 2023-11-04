namespace Spell
{
    [System.Serializable]
    public class DamageEffect : IEffect
    {
        public static string TypeName => nameof(DamageEffect);
        [Muttable] public int DamageAmount { get; set; }
        public string MainType { get; set; }
        public int ElementIndex { get; set; }

        public DamageEffect()
        {
            MainType = GetType().Name;
        }
    }
}