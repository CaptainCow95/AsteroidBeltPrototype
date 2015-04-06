using UnityEngine;

namespace AsteroidBelt
{
    namespace Assets.Scripts
    {
        // Found at http://devmag.org.za/2012/07/12/50-tips-for-working-with-unity-best-practices/
        public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
        {
            private static T instance;

            public static T Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = (T)FindObjectOfType(typeof(T));

                        if (instance == null)
                        {
                            Debug.LogError("An instance of " + typeof(T) + " was used, but there wasn't one in the scene.");
                        }
                    }

                    return instance;
                }
            }
        }
    }
}