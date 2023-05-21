using Microsoft.ReportingServices.DataProcessing;
using System;

namespace Saturn.SSRSSecurityExtension
{
    public class SSRSDbConnection : Microsoft.ReportingServices.DataProcessing.IDbConnectionExtension
    {
        public string Impersonate { set => throw new NotImplementedException(); }
        public string UserName { set => throw new NotImplementedException(); }
        public string Password { set => throw new NotImplementedException(); }
        public bool IntegratedSecurity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int ConnectionTimeout => throw new NotImplementedException();

        public string LocalizedName => throw new NotImplementedException();

        public IDbTransaction BeginTransaction()
        {
            return null;
        }

        public void Close()
        {
            return;
        }

        public IDbCommand CreateCommand()
        {
            return null;
        }

        public void Dispose()
        {
            return;
        }

        public void Open()
        {
            return;
        }

        public void SetConfiguration(string configuration)
        {
            return;
        }
    }
}
