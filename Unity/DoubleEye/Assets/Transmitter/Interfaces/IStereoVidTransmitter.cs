namespace Assets.Transmitter
{
    /// <summary>
    /// Responsible for providing textures to display on scene
    /// </summary>
    public interface IStereoVidTransmitter
    {
        /// <summary>
        /// Returns object containing two textures, each for one eye.
        /// </summary>
        StereoView GetStereoView();
    }
}
