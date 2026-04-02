using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExampleApp : MonoBehaviour
{
    [Header("Test data")]
    public User user;
    public TMP_InputField username;
    public TMP_InputField password;

    public TMP_InputField REG_username;
    public TMP_InputField REG_password;

    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public UserDataApiClient userDataApiClient;

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
    public async void Register()
    {
        user.Email = REG_username.text;
        user.Password = REG_password.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
                signUpPanel.SetActive(false);
                LoginScreen.SetActive(true);
                addUserDataPanel.SetActive(false);
                pathScreen.SetActive(false);
                // TODO: Handle succes scenario;
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Register error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    [ContextMenu("User/Login")]
    public async void Login()
    {
        user.Email = username.text;
        user.Password = password.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                LoginScreen.SetActive(false);
                pathScreen.SetActive(true);
                // TODO: Todo handle succes scenario.
                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Login error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }
    }

    private UserData CollectUserData()
    {
        UserData userData = new UserData();
        userData.DoctorName = doctorNameInput.text;
        userData.AppointmentType = appointmentTypeInput.text;
        userData.AppointmentDate = appointmentDateInput.text;

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
        IWebRequestReponse response = await userDataApiClient.CreateUserData(newUserData);

        switch (response)
        {
            case WebRequestData<UserData> successResponse:
                Debug.Log("UserData succesvol opgeslagen!");
                addUserDataPanel.SetActive(false);
                pathScreen.SetActive(true);
                // TODO: Navigatie of succesmelding tonen
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

    #endregion
}
