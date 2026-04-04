using System;
using UnityEngine.Serialization;

namespace DefaultNamespace.Models
{
    [Serializable]
    public class RefreshData
    {
        [FormerlySerializedAs("refreshToken")] public string RefreshToken;
    }
}