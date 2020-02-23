﻿using System.Security.Cryptography.X509Certificates;
 using System.Threading.Tasks;

 namespace Navtrack.Web.Services.LetsEncrypt
{
    public interface ICertificateDataService
    {
        Task<X509Certificate2> GetCertificate();
        Task SaveAsync(X509Certificate2 certificate);
    }
}