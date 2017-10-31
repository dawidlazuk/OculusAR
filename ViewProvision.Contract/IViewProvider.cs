using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewProvision.Contract
{
    public interface IViewProvider
    {
        bool IsCalibrated { get; }
        IEnumerable<int> AvailableCameraIndexes { get; }

        void ResetCalibration();
        void SetCapture(CaptureSide captureSide, int cameraIndex);

        ViewData GetCurrentView();
    }
}
