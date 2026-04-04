using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

public static class JsonHelper
{
    public static readonly JsonSerializerSettings CamelCaseSettings = new JsonSerializerSettings()
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
    };

    public static string ExtractToken(string data)
    {
        Token token = JsonConvert.DeserializeObject<Token>(data);
        return token.accessToken;
    }

    public static string ExtractRefreshToken(string data)
    {
        RefreshToken refreshToken = JsonConvert.DeserializeObject<RefreshToken>(data);
        return refreshToken.refreshToken;
    }
}