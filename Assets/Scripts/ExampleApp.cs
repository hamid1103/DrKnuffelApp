using System;
using System.Collections.Generic;
using DefaultNamespace.Models;
using TMPro;
using UnityEngine;

public class ExampleApp : MonoBehaviour
{
    [Header("Test data")]
    public User user;
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text LOGIN_ERRORS;

    public TMP_InputField REG_username;
    public TMP_InputField REG_password;
    public TMP_Text REG_ERRORS;
    public TMP_Text USD_ERRORS;
    
    public GameManager gameManager;
    
    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public UserDataApiClient userDataApiClient;
    public UserProgressApiClient userProgressApiClient;

    [Header("Game objects")]
    public GameObject signUpPanel;
    public GameObject addUserDataPanel;
    public GameObject LoginScreen;
    public GameObject pathScreen;

    [Header("UserData Input Fields")]
    public TMP_InputField doctorNameInput;
    public TMP_InputField appointmentTypeInput;
    public TMP_InputField appointmentDateInput;
    public TMP_InputField userAgeInput;

    #region Login

    [ContextMenu("User/Register")]
    public async void Refresh(string token)
    {
        IWebRequestReponse webRequestResponse = await userApiClient.Refresh(token);
        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                //For some reason, Login doesn't fetch userdata in webClient... Gonna do it here instead
                IWebRequestReponse userData = await userDataApiClient.ReadUserData();
                switch (userData)
                {
                    case WebRequestData<UserData> dataUser:
                        gameManager.userData.AppointmentDate = dataUser.Data.AppointmentDate;
                        gameManager.userData.Id = dataUser.Data.Id;
                        gameManager.userData.UserID = dataUser.Data.UserID;
                        break;
                    case WebRequestData<List<UserData>> dataList:
                        UserData data = dataList.Data[0];
                        gameManager.userData.AppointmentDate = data.AppointmentDate;
                        gameManager.userData.Id = data.Id;
                        gameManager.userData.UserID = data.UserID;
                        break;
                    default:
                        Debug.LogError("Unknown / Unhandled response type: "+userData);
                        break;
                }
                SyncProgressData();
                //Nothing else. Calling Refresh function on ApiClient also handles updating token and refreshtoken
                break;
            case WebRequestError requestError:
                Debug.LogError(requestError.ErrorMessage);
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }
    
    public async void Register()
    {
        user.Email = REG_username.text + "@arcadianflame.nl";
        user.Password = REG_password.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                REG_ERRORS.text = "";
                //Login in automatically. Having the user login first to continue registering data is against UX principles.
                username.text = REG_username.text;
                password.text = REG_password.text;
                Login(false);
                addUserDataPanel.SetActive(true);
                
                // TODO: Handle succes scenario;
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Register error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                REG_ERRORS.text = errorMessage;
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("User/Login")]
    public async void Login(bool ToPath =true)
    {
        LOGIN_ERRORS.text = "";
        user.Email = username.text + "@arcadianflame.nl";
        user.Password = password.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                //incase username locally differs from the username for login
                gameManager.UserName = username.text;
                gameManager.LoggedIn = true;
                if (ToPath)
                {
                    IWebRequestReponse userData = await userDataApiClient.ReadUserData();
                    switch (userData)
                    {
                        case WebRequestData<UserData> dataUser:
                            gameManager.userData.AppointmentDate = dataUser.Data.AppointmentDate;
                            gameManager.userData.Id = dataUser.Data.Id;
                            gameManager.userData.UserID = dataUser.Data.UserID;
                            //If user is registering, there is no userData object
                            //Trying to sync save data without UserId present will return 500 error.
                            SyncProgressData();
                            break;
                        case WebRequestData<List<UserData>> dataList:
                            UserData data = dataList.Data[0];
                            gameManager.userData.AppointmentDate = data.AppointmentDate;
                            gameManager.userData.Id = data.Id;
                            gameManager.userData.UserID = data.UserID;
                            //If user is registering, there is no userData object
                            //Trying to sync save data without UserId present will return 500 error.
                            SyncProgressData();
                            break;
                        default:
                            Debug.LogError("Unknown / Unhandled response type: "+userData);
                            break;
                    }
                    LoginScreen.SetActive(false);
                    pathScreen.SetActive(true);
                }
                
                // TODO: Todo handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                LOGIN_ERRORS.text = errorMessage;
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    private bool IsValidDate(string input, out string formattedDate)
    {
        formattedDate = "";
        input = input.Replace("-", "/");

        if (DateTime.TryParseExact(input, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime result))
        {
            formattedDate = result.ToString("yyyy-MM-dd");
            return true;
        }

        return false;
    }

    private UserData CollectUserData()
    {
        USD_ERRORS.text = "";
        UserData userData = new UserData();
        userData.DoctorName = doctorNameInput.text;
        userData.AppointmentType = appointmentTypeInput.text;
        
        if (IsValidDate(appointmentDateInput.text, out string formattedDate))
        {
            gameManager.userData.AppointmentDate = formattedDate;
            PlayerPrefs.SetString("AppointmentDate", formattedDate);
            userData.AppointmentDate = formattedDate;
        }
        else
        {
            Debug.LogError("Ongeldige datum! Gebruik formaat: dd/MM/yyyy");
            USD_ERRORS.text = "Ongeldige datum! Gebruik formaat: dd/MM/yyyy";
            return null;
        }

        if (int.TryParse(userAgeInput.text, out int age))
        {
            userData.UserAge = age;
        }
        else
        {
            userData.UserAge = 0;
        }
        return userData;
    }

    public async void SaveUserData()
    {
        UserData newUserData = CollectUserData();

        if (newUserData == null)
        {
            Debug.LogError("UserData ongeldig, request gestopt!");
            return;
        }

        IWebRequestReponse response = await userDataApiClient.CreateUserData(newUserData);

        switch (response)
        {
            case WebRequestData<UserData> successResponse:
                Debug.Log("UserData succesvol opgeslagen!");
                //After creating userData, syncing save data should be safe
                addUserDataPanel.SetActive(false);
                signUpPanel.SetActive(false);
                pathScreen.SetActive(true);
                //TODO: Save UserData Response
                PlayerPrefs.SetString("UserDataId",successResponse.Data.Id);
                Debug.Log($"UDId = {successResponse.Data.Id}");
                gameManager.userData.Id = successResponse.Data.Id;
                SyncProgressData();
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Error bij opslaan UserData: " + errorResponse.ErrorMessage);
                // TODO: Foutmelding tonen
                break;
            default:
                Debug.LogError("Onbekende response type bij UserData opslaan.");
                break;
        }
    }

    public async void SyncProgressData()
    {
        if (gameManager.LocalSaveData.CompletedSteps.Count != 0)
        {
            //Save data regardless of however many steps are completed locally. It won't override progress 
            //"Thank god." - Jesus

            IWebRequestReponse response = await userProgressApiClient.SaveUserProgressData();
            switch (response)
            {
                case WebRequestData<string> success:
                    Debug.Log("Opgeslagen");
                    break;
                case WebRequestError errorResponse:
                    Debug.LogError("Error bij opslaan progress: " + errorResponse.ErrorMessage);
                    break;
                default:
                    Debug.LogError("Onbekende response type bij UserData opslaan.");
                    break;
            }

        }
        
        //Then load
        IWebRequestReponse loadResponse = await userProgressApiClient.LoadUserProgressData();
        switch (loadResponse)
        {
            case WebRequestData<string> dataResponseString:
                string wrappedJson = "{\"items\":" + dataResponseString.Data + "}";

                ProgressList progresLoaded = JsonUtility.FromJson<ProgressList>(wrappedJson);
                
                //Use List Count to load progress data...
                //Really should've included index order into Progress model
                //But too late to do anything about it rn :P
                int index = 0;
                if (progresLoaded.items.Count > 0)
                {
                    while (index < progresLoaded.items.Count)
                    {
                        //using complete step also prevents locally double saving
                        gameManager.LocalSaveData.CompleteStep(index);
                        index++;
                    }
                }
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Error bij laden progress: " + errorResponse.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende response type bij UserData laden.");
                Debug.Log($"{loadResponse}");
                break;
        }
    }
    
    #endregion
}

[Serializable]
public class ProgressList
{
    public List<Progress> items;
}