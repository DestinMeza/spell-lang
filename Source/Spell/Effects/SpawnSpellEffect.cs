namespace Spell
{
    [System.Serializable]
    public class SpawnSpellEffect : IEffect
    {
        public static string TypeName => nameof(SpawnSpellEffect);
        public Spell Spell { get; set; }
        public string MainType { get; set; }
        public int ElementIndex { get; set; }

        public SpawnSpellEffect(Spell spell)
        {
            Spell = spell;
            MainType = GetType().Name;
        }
    }
}