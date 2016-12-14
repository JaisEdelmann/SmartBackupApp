using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartBackupApp.App.Services
{
    public class StateService
    {
        public bool Sleep(bool force)
        {
            return Application.SetSuspendState(PowerState.Suspend, force, true);
        }        
    }
}
