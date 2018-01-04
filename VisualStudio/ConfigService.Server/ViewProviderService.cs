﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Diagnostics;

using ViewProvision.Contract;
using ConfigService.Contract;
using System.ServiceModel.Description;

namespace ConfigService.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
    public class ViewProviderService : IViewProviderService
    {
        private static readonly string EndpointName = "Config";

        private readonly IProcessedViewProvider processedViewProvider;

        public static ViewProviderService Create(IProcessedViewProvider provider, string port = "56719")
        {
            ServiceHost host = null;

            ViewProviderService serviceInstance = new ViewProviderService(provider);
            try
            {
                Uri uri = new Uri($"http://localhost:{port}/OculusAR");
                host = new ServiceHost(serviceInstance, uri);

                var binding = new BasicHttpBinding();              

                host.AddServiceEndpoint(
                    typeof(IViewProviderService),
                    binding,
                    EndpointName);

                host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });

                host.Open();
                return serviceInstance;
            }
            catch (Exception ex)
            {
                host = null;
                Debug.WriteLine("Error hosting config service: " + ex.Message);
                return null;
            }
        }


        public ViewProviderService(IProcessedViewProvider processedViewProvider)
        {
            this.processedViewProvider = processedViewProvider;
        }
        
        #region IViewProviderService implementation

        public ViewDataBitmap GetCurrentViewAsBitmaps()
        {
            return processedViewProvider.GetCurrentViewAsBitmaps();
        }

#if DEBUG
        //TODO remove - only for development
        public void UpdateFrames()
        {
            processedViewProvider.UpdateFrames();
        }
#endif

        #region IViewCalibrator implementation

        public void RotateImage(CaptureSide captureSide, RotateSide rotateSide)
        {
            processedViewProvider.RotateImage(captureSide, rotateSide);
        }

        #endregion

        #region ICaptureManagerService implementation

        public void SetCapture(CaptureSide captureSide, int cameraIndex)
        {
            processedViewProvider.SetCapture(captureSide, cameraIndex);
        }

        public CaptureDetails GetCaptureDetails()
        {
            return processedViewProvider.GetCaptureDetails();
        }

        #endregion

        #endregion

        public List<Tuple<string,bool>> GetAllImageProcessors()
        {
            return processedViewProvider.GetAllImageProcessors();
        }

        public void SetProcessorState(string processorName, bool state)
        {
            processedViewProvider.SetProcessorState(processorName, state);
        }

        public void ChangeProcessorPriority(string processorName, bool increase)
        {
            processedViewProvider.ChangeProcessorPriority(processorName, increase);
        }
    }
}
