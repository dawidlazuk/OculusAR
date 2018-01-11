using System.Collections;
using System.Collections.Generic;
using Assets.Transmitter;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using ViewProvision;
using ViewProvision.Contract;
using ViewProvision.Processors;
using System;

public class PlayCameraVideos : MonoBehaviour
{
	public RawImage LeftImage;
	public RawImage RightImage;

	private IStereoVidTransmitter _stereoVidTransmitter;

	// Use this for initialization
	void Start()
	{
		frameCounter = 1000;
		ViewProvider viewProvider = new ViewProvider(true);
        var processedProvider = new ProcessedViewProvider(viewProvider, new List<IImageProcessor>
        {
           new SmoothBilateralProcessor(7,255,34),
           new SobelProcessor(),
           new GrayImageProcessor()
        });
		ConfigService.Server.ViewProviderService.Create(processedProvider);        

		/* 
		 * Below part is used for setting proper camera for each channel before we'll develop the proper connection with the config app.
		 * Change the hardcoded indexes regard to needs.
		 */
	   //viewProvider.SetCapture(ViewProvision.Contract.CaptureSide.Left, 0);
	   // viewProvider.SetCapture(ViewProvision.Contract.CaptureSide.Right, 1);

		var converter = new TextureConverter();

		_stereoVidTransmitter = new StereoVidTransmitter(converter, processedProvider);
	}

	int frameCounter;

	// Update is called once per frame
	void Update()
	{
		if (frameCounter == 1000) {
			Debug.Log (DateTime.Now);
			frameCounter = 0;
		}

		var view = _stereoVidTransmitter.GetStereoView();
		RightImage.texture = view.RightEye;
		LeftImage.texture = view.LeftEye;

		frameCounter++;
	}
}
