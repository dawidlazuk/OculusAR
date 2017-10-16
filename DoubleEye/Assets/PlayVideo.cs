using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]

public class PlayVideo : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    private RawImage eyeImage;

	// Use this for initialization
	void Start ()
	{
	    _videoPlayer = GetComponent<VideoPlayer>();
	    eyeImage = GetComponent<RawImage>();
	    eyeImage.texture = _videoPlayer.texture;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space) && _videoPlayer.isPlaying)
	    {
	        _videoPlayer.Pause();
	    }
        else if (Input.GetKeyDown(KeyCode.Space) && !_videoPlayer.isPlaying)
	    {
	        _videoPlayer.Play();
	    }
	    eyeImage.texture = _videoPlayer.texture;

    }
}
