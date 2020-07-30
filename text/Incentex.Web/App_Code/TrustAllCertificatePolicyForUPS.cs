using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

public class TrustAllCertificatePolicyForUPS : System.Net.ICertificatePolicy
{
    public TrustAllCertificatePolicyForUPS()
    { }

    public bool CheckValidationResult(ServicePoint sp,
     System.Security.Cryptography.X509Certificates.X509Certificate cert, WebRequest req, int problem)
    {
        return true;
    }
}
