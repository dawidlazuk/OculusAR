namespace ViewVisualization.Controls
{

    /// <summary>
    /// Details of a image processor
    /// </summary>
    public class ProcessorInfo
    {
        /// <summary>
        /// Processor's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Value determining if the processor is applied to image.
        /// </summary>
        public bool Active { get; set; }
    }
}