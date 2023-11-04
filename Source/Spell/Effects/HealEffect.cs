namespace SpellCompiler
{
    [System.Serializable]
    public class HealEffect : IEffect
    {
        public static string TypeName => nameof(HealEffect);
        public int HealingAmount { get; set; }
        public string MainType { get; set; }
        public int ElementIndex { get; set; }

        public HealEffect()
        {
            MainType = GetType().Name;
        }
    }
}