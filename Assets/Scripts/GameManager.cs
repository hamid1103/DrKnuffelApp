using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public string UserName = "";
    public GameObject NextScreen;
    public TMP_InputField NameInputField;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UserName = PlayerPrefs.GetString("UserName");
    }

    public void ShowNameInput()
    {
        if (string.IsNullOrEmpty(UserName))
        {
            NameInputField.gameObject.SetActive(true);
        }
        else
        {
            NextScreen.SetActive(true);
        }
    }

    public void SaveNameAndNextScreen()
    {
        UserName = NameInputField.text;
        PlayerPrefs.SetString("UserName", UserName);
        NextScreen.SetActive(true);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
