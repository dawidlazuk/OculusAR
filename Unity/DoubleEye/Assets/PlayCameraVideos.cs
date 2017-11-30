using System.Collections;
using System.Collections.Generic;
using Assets.Transmitter;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using ViewProvision;

public class PlayCameraVideos : MonoBehaviour
{
    public RawImage LeftImage;
    public RawImage RightImage;

    private IStereoVidTransmitter _stereoVidTransmitter;

    // Use this for initialization
    void Start()
    {
        var viewProvider = new ViewProvider();

        /* 
         * Below part is used for setting proper camera for each channel before we'll develop the proper connection with the config app.
         * Change the hardcoded indexes regard to needs.
         * TODO Delete / Review during future developement
         */
        viewProvider.SetCapture(ViewProvision.Contract.CaptureSide.Left, 1);
        viewProvider.SetCapture(ViewProvision.Contract.CaptureSide.Right, 2);

        var converter = new TextureConverter();

        _stereoVidTransmitter = new StereoVidTransmitter(converter, viewProvider);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // VideoPlayer.Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            //VideoPlayer.Play();
        }

        var view = _stereoVidTransmitter.GetStereoView();
        RightImage.texture = view.LeftEye;
        LeftImage.texture = view.RightEye;
    }
}
