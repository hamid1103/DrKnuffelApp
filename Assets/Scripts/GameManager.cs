using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public string UserName = "";
    public GameObject PathScreen;
    public GameObject NextScreen;
    public GameObject CollectionLogin;
    public GameObject WelcomeScreen;
    //Don't ask me why this is here. Unity's timeline feature has some FUNKY ass behaviour, causing
    //DR bear to show up even if the gameobject is deactivated.
    public GameObject WelcomeScreenActual;
    public TMP_InputField NameInputField;

    public LocalSave LocalSaveData = new();
    
    private SignalReceiver _signalReceiver;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _signalReceiver = gameObject.GetComponent<SignalReceiver>();
        UserName = PlayerPrefs.GetString("UserName");
        LoadSave();
    }
    
    public void ShowNameInput()
    {
        if (string.IsNullOrEmpty(UserName))
        {
            CollectionLogin.SetActive(true);
        }
        else
        {
            NextScreen.SetActive(true);
            CollectionLogin.SetActive(false);
            WelcomeScreenActual.GetComponent<PlayableDirector>().Stop();
            WelcomeScreenActual.SetActive(false);
            WelcomeScreen.SetActive(false);
        }
    }

    public void SaveNameAndNextScreen()
    {
        UserName = NameInputField.text;
        PlayerPrefs.SetString("UserName", UserName);
        NextScreen.SetActive(true);
        CollectionLogin.SetActive(false);
        WelcomeScreenActual.GetComponent<PlayableDirector>().Stop();
        WelcomeScreenActual.SetActive(false);
        WelcomeScreen.SetActive(false);
    }

    public void StepComplete(int step)
    {
        PathScreen.SetActive(true);
        LocalSaveData.CompleteStep(step);
        StoreSaveData();
    }

    public void LoadSave()
    {
        string SaveDataString = PlayerPrefs.GetString("saveData");
        if(!string.IsNullOrEmpty(SaveDataString))
            LocalSaveData = JsonUtility.FromJson<LocalSave>(SaveDataString);
    }

    public void StoreSaveData()
    {
        PlayerPrefs.SetString("saveData",JsonUtility.ToJson(LocalSaveData));
    }
    
}
