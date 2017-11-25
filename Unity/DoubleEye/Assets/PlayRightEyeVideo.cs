using System.Collections;
using System.Collections.Generic;
using Assets.Transmitter;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using ViewProvision;

[RequireComponent(typeof(AudioSource))]

public class PlayRightEyeVideo : MonoBehaviour
{

    public VideoPlayer VideoPlayer;

    private RawImage rightImage;

    // Use this for initialization
    void Start()
    {
        VideoPlayer = GetComponent<VideoPlayer>();
        rightImage = GetComponent<RawImage>();
        rightImage.texture = VideoPlayer.texture;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && VideoPlayer.isPlaying)
        {
            VideoPlayer.Pause();
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !VideoPlayer.isPlaying)
        {
            VideoPlayer.Play();

            rightImage.texture = VideoPlayer.texture;

        }
    }
}
