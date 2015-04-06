using UnityEngine;

namespace AsteroidBelt
{
    public class LoadOnClick : MonoBehaviour
    {
        public void LoadScene(int level)
        {
            Application.LoadLevel(level);
        }
    }
}