namespace Project.Scripts.Core.Scene
{
    public static class SceneParameterStore
    {
        static SceneParameter pending;

        public static void Set(SceneParameter parameter) => pending = parameter;

        public static T GetOrDefault<T>(T defaultValue) where T : SceneParameter
        {
            var result = pending as T ?? defaultValue;
            pending = null;
            return result;
        }
    }
}
