using UnityEngine;

public partial class InfoToggle : MonoBehaviour
{
    public GameObject InfoBackground;

    public void Info()
    {
        bool isActive = InfoBackground.activeSelf;
        InfoBackground.SetActive(!isActive);
    }
}
