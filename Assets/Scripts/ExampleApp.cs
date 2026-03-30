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
    public TMP_InputField doctor;
    public TMP_InputField type_treatment;
    public TMP_InputField date_treatment;

    [Header("Dependencies")]
    public UserApiClient userApiClient;



    #region Login

    [ContextMenu("User/Register")]
    public async void Register()
    {
        user.Email = REG_username.text;
        user.Password = REG_password.text;
        user.DoctorName = doctor.text;
        user.AppointmentType = type_treatment.text;
        user.AppointmentDate = date_treatment.text;
        IWebRequestReponse webRequestResponse = await userApiClient.Register(user);

        switch (webRequestResponse)
        {
            case WebRequestData<string> dataResponse:
                Debug.Log("Register succes!");
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

    #endregion
}
