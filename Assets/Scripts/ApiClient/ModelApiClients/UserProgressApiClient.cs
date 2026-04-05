using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class UserProgressApiClient : MonoBehaviour
{
    //Notes for me, webclient handles auth.
    public WebClient webClient;
    public GameManager gameManager;

    //Function handles data collection
    public async Awaitable<IWebRequestReponse> SaveUserProgressData()
    {
        //Just upload whole save data string. Sorting and de-duplication is handled on back-end;
        string route = "/progress/bulk";
        BulkDataUpload RawData = new BulkDataUpload();
        RawData.userData_id = gameManager.userData.Id;
        RawData.steps = gameManager.LocalSaveData.CompletedSteps;
        string dataJson = JsonConvert.SerializeObject(RawData);
        return await webClient.SendPostRequest(route, dataJson);
    }

    public async Awaitable<IWebRequestReponse> LoadUserProgressData()
    {
        string route = "/progress";
        return await webClient.SendGetRequest(route);
    }
}

public class BulkDataUpload
{
    public string userData_id;
    public List<int> steps;
}