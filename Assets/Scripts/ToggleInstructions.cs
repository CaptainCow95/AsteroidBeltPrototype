using UnityEngine;

public class ToggleInstructions : MonoBehaviour
{
    public GameObject instructions;

    public void toggleActive()
    {
        if (instructions.activeInHierarchy)
        {
            instructions.SetActive(false);
        }
        else
        {
            instructions.SetActive(true);
        }
    }
}