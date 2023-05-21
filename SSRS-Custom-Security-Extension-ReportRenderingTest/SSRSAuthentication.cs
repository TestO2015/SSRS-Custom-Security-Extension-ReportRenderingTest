using Microsoft.ReportingServices.Interfaces;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace SSRSSecurityExtension
{
    public class SSRSAuthentication : IAuthenticationExtension2
    {
        public string LocalizedName => null;

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword,
                                    int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        // Close token handle
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        static object lockObject = new object();

        public static ClaimsPrincipal LogonUserInternal(string username, string domain, string password)
        {
            bool success = LogonUser(username, domain, password, 2, 0, out IntPtr token3);

            if (success)
            {
                // Create WindowsIdentity object based on the authenticated token
                var identity = new System.Security.Principal.WindowsIdentity(token3);
                var principal = new System.Security.Principal.WindowsPrincipal(identity);

                // Set authenticated principal for current thread
                System.Threading.Thread.CurrentPrincipal = principal;

                // Retrieve authenticated user identity
                var authenticatedIdentity = System.Threading.Thread.CurrentPrincipal.Identity;
                Console.WriteLine("Authenticated user: " + authenticatedIdentity.Name);

                // Close the authenticated token handle
                CloseHandle(token3);
                return principal;
            }
            else
            {
                int error = Marshal.GetLastWin32Error();
                Console.WriteLine("Logon failed with error code: " + error);
                return null;
            }
        }

        public void GetUserInfo(out IIdentity userIdentity, out IntPtr userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Username.Name),
            };
            userIdentity = new ClaimsIdentity(claims, "CustomAuthenticationType");
            userId = IntPtr.Zero;
            return;
        }

        /// <remarks>
        /// Called for requests made to the SSRS Report Portal.
        /// If it fails it falls back to the <see cref="GetUserInfo(out IIdentity, out IntPtr)"/>.
        /// </remarks>
        public void GetUserInfo(IRSRequestContext requestContext, out IIdentity userIdentity, out IntPtr userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Username.Name),
            };
            userIdentity = new ClaimsIdentity(claims, "CustomAuthenticationType");
            userId = IntPtr.Zero;
            return;
        }

        /// <summary>
        /// Checks to see if the user is valid
        /// </summary>
        public bool IsValidPrincipalName(string principalName)
        {
            return true;
        }

        /// <summary>
        /// Supports WebService Logins (via SSMS or calling the .asmx Web Services)
        /// </summary>
        /// <param name="username">Username for the User</param>
        /// <param name="password">Password to verify with the Open Id Provider</param>
        /// <param name="authority">Not used if password is provided.  If no password is empty, a valid OIDC AccessToken can be sent here</param>
        public bool LogonUser(string username, string password, string authority)
        {
            if (!string.IsNullOrWhiteSpace(username) || !string.IsNullOrWhiteSpace(password))
                throw new NotSupportedException($"Username and password parameters not supported. Please specify an Authentication Token in the {nameof(authority)} parameter instead.");

            return false;
        }

        /// <summary>
        /// Called at Startup to Configure the Extension
        /// 
        /// Configuration Items are read from the rsreportserver.config file
        /// </summary>
        public void SetConfiguration(string configuration)
        {
            return;
        }
    }
}
