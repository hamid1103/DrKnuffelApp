using UnityEngine;

public class ColorBlindToggle : MonoBehaviour
{

    public GameObject colorFilter;

    public void ColorToggle()
    {
        bool isActive = colorFilter.activeSelf;
        colorFilter.SetActive(!isActive);
    }
}
