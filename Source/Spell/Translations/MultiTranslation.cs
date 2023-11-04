using Newtonsoft.Json;

namespace Spell
{
    [System.Serializable]
    public class MultiTranslation : IStateBasedTranslation
    {
        public static string TypeName => nameof(MultiTranslation);
        public SerializableVector3[] Directions { get; set; }
        public float[] TimeSteps { get; set; }
        public bool[] LerpSmoothly { get; set; }
        public string MainType { get; set; }
        public int ElementIndex { get; set; }

        public MultiTranslation(SerializableVector3[] directions, float[] timeSteps, bool[] lerpSmoothly)
        {
            Directions = directions;
            TimeSteps = timeSteps;
            LerpSmoothly = lerpSmoothly;
            MainType = GetType().Name;
        }

        public MultiTranslation(SerializableVector3[] directions) : this(directions, DefaultTimeSteps(directions.Length), DefaultLerps(directions.Length))
        {

        }

        #region Defaults
        private static float[] DefaultTimeSteps(int count) 
        {
            const float DEFAULT_TIMESTEP = 1.0f;

            float[] defaultTimeSteps = new float[count];

            for (int i = 0; i < defaultTimeSteps.Length; i++) 
            {
                defaultTimeSteps[i] = DEFAULT_TIMESTEP;
            }

            return defaultTimeSteps;
        }
        private static bool[] DefaultLerps(int count)
        {
            const bool DEFAULT_LERP_STATE = true;

            bool[] defaultLerps = new bool[count];

            for (int i = 0; i < defaultLerps.Length; i++)
            {
                defaultLerps[i] = DEFAULT_LERP_STATE;
            }

            return defaultLerps;
        }
        #endregion
    }
}