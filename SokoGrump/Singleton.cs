using System.Threading;

namespace SokoGrump
{
    public abstract class Singleton<T> where T : class, new()
    {
        protected static volatile T instance;
        static readonly Lock syncRoot = new();

        protected static void SetInstance(T value) => instance = value;

        public static T Instance
        {
            get
            {
                if (instance is null)
                {
                    lock (syncRoot)
                    {
                        instance ??= new T();
                    }
                }

                return instance;
            }
        }
    }
}
