using UnityEngine.Networking;

namespace DefaultNamespace.ApiClient
{
    public class BypassCert: CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            // Always returns true, indicating that the certificate is valid
            return true;
        }
    }
}