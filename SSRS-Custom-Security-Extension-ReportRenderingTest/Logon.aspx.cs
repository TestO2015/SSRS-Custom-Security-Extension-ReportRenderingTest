using System;
using System.Web.Security;

namespace SSRSSecurityExtension
{
    public class Logon : System.Web.UI.Page
    {
        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);
        }
        #endregion

        public string Authority { get; set; }

        void Page_Load(object sender, System.EventArgs e)
        {
            FormsAuthentication.RedirectFromLoginPage("Performa-PC\\User2", false);
            return;
        }
    }
}
