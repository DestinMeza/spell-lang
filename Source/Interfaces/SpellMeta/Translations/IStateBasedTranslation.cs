namespace Spell
{
    public interface IStateBasedTranslation : ITranslation
    {
        SerializableVector3[] Directions { get; set; }
        float[] TimeSteps { get; set; }
        bool[] LerpSmoothly { get; set; }
    }
}