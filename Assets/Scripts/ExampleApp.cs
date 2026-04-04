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
    public TMP_Text LOGIN_ERRORS;

    public TMP_InputField REG_username;
    public TMP_InputField REG_password;
    public TMP_Text REG_ERRORS;

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
        user.Email = username.text;
        user.Password = password.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Login(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Login succes!");
                if (ToPath)
                {
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
        UserData userData = new UserData();
        userData.DoctorName = doctorNameInput.text;
        userData.AppointmentType = appointmentTypeInput.text;

        if (IsValidDate(appointmentDateInput.text, out string formattedDate))
        {
            userData.AppointmentDate = formattedDate;
        }
        else
        {
            Debug.LogError("Ongeldige datum! Gebruik formaat: dd/MM/yyyy");
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
                addUserDataPanel.SetActive(false);
                signUpPanel.SetActive(false);
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
