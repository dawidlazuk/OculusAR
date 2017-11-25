using System.Collections;
using System.Collections.Generic;
using Assets.Transmitter;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using ViewProvision;

[RequireComponent(typeof(AudioSource))]

public class PlayCameraVideos : MonoBehaviour
{


    public RawImage LeftImage;
    public RawImage RightImage;

    private IStereoVidTransmitter _stereoVidTransmitter;

    // Use this for initialization
    void Start()
    {
        var viewProvider = new ViewProvider();
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
