using DefaultNamespace.Models;
using Newtonsoft.Json;
using UnityEngine;

public class UserApiClient : MonoBehaviour
{
    public WebClient webClient;
    
    public async Awaitable<IWebRequestReponse> Register(User user)
    {
        string route = "/account/register";
        string data = JsonConvert.SerializeObject(user, JsonHelper.CamelCaseSettings);

        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Refresh(string token)
    {
        RefreshData rd = new RefreshData();
        rd.RefreshToken = token;
        string route = "/account/refresh";
        string data = JsonConvert.SerializeObject(rd);

        IWebRequestReponse reponse = await webClient.SendPostRequest(route, data);
        return ProcessLoginResponse(reponse);
    }

    public async Awaitable<IWebRequestReponse> Login(User user)
    {
        string route = "/account/login";
        string data = JsonConvert.SerializeObject(user, JsonHelper.CamelCaseSettings);

        IWebRequestReponse response = await webClient.SendPostRequest(route, data);
        return ProcessLoginResponse(response);
    }

    private IWebRequestReponse ProcessLoginResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                string token = JsonHelper.ExtractToken(data.Data);
                string refreshToken = JsonHelper.ExtractRefreshToken(data.Data);
                webClient.SetToken(token);
                webClient.SetRefreshToken(refreshToken);
                PlayerPrefs.SetString("RefreshToken",refreshToken);
                GameObject.Find("GameManager").GetComponent<GameManager>().LoggedIn = true;
                return new WebRequestData<string>("Succes");
            default:
                return webRequestResponse;
        }
    }

}

