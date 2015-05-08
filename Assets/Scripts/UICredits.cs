using UnityEngine;
using UnityEngine.UI;

namespace AsteroidBelt
{
    public class UICredits : MonoBehaviour
    {
        // Use this for initialization
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
            gameObject.GetComponent<Text>().text = "Credits:\n" + GameManager.Instance.totalCredits;
        }
    }
}