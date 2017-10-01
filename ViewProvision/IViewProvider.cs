using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewProvision
{
    interface IViewProvider
    {
        bool IsCalibrated { get; }

        ViewData GetCurrentView();
    }
}
