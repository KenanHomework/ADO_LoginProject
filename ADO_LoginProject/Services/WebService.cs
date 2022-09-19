using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO_LoginProject.Services
{
    public abstract class WebService
    {
        public static void OpenWebsiteWithUrl(string url)
        {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = url;
            System.Diagnostics.Process.Start(psi);
        }

    }
}
