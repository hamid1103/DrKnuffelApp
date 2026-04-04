using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public string UserName = "";
    public User user;
    public UserData userData;
    public bool LoggedIn = false;
    
    public GameObject PathScreen;
    public GameObject NextScreen;
    public GameObject CollectionLogin;
    public GameObject WelcomeScreen;
    //Don't ask me why this is here. Unity's timeline feature has some FUNKY ass behaviour, causing
    //DR bear to show up even if the gameobject is deactivated.
    public GameObject WelcomeScreenActual;
    public TMP_InputField NameInputField;

    [SerializeField]public LocalSave LocalSaveData = new();
    
    private SignalReceiver _signalReceiver;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _signalReceiver = gameObject.GetComponent<SignalReceiver>();
        UserName = PlayerPrefs.GetString("UserName");
        LoadSave();
        userData.AppointmentDate = PlayerPrefs.GetString("AppointmentDate");
        userData.Id = PlayerPrefs.GetString("UserDataId");
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
        if (!string.IsNullOrEmpty(SaveDataString))
        {
            Debug.Log($"Loading Save Data. String: {SaveDataString}");
            LocalSaveData = JsonUtility.FromJson<LocalSave>(SaveDataString);
        }
        else
        {
            Debug.Log($"Failure. Data string is empty.");
        }
    }

    public void StoreSaveData()
    {
        Debug.Log($"Present steps in list: {string.Join(", ", LocalSaveData.CompletedSteps)}");
        Debug.Log($"Saving data string: {JsonUtility.ToJson(LocalSaveData)}");
        PlayerPrefs.SetString("saveData",JsonUtility.ToJson(LocalSaveData));
    }
    
}
