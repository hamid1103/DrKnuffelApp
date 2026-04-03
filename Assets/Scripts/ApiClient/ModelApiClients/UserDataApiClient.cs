using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class UserDataApiClient : MonoBehaviour
{
    public WebClient webClient;

    public async Awaitable<IWebRequestReponse> ReadUserData()
    {
        string route = "/extrauserdata";

        IWebRequestReponse webRequestResponse = await webClient.SendGetRequest(route);
        return ParseUserDataListResponse(webRequestResponse);
    }

    public async Awaitable<IWebRequestReponse> CreateUserData(UserData user)
    {
        string route = "/extrauserdata";
        string data = JsonConvert.SerializeObject(user, JsonHelper.CamelCaseSettings);

        IWebRequestReponse webRequestResponse = await webClient.SendPostRequest(route, data);
        return ParseUserDataResponse(webRequestResponse);
    }

    private IWebRequestReponse ParseUserDataResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                UserData user = JsonConvert.DeserializeObject<UserData>(data.Data);
                WebRequestData<UserData> parsedWebRequestData = new WebRequestData<UserData>(user);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

    private IWebRequestReponse ParseUserDataListResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                List<UserData> user = JsonConvert.DeserializeObject<List<UserData>>(data.Data);
                WebRequestData<List<UserData>> parsedWebRequestData = new WebRequestData<List<UserData>>(user);
                return parsedWebRequestData;
            default:
                return webRequestResponse;
        }
    }

}

