namespace GeologicalProperties.Model
{
    public class ExtrapolationProperties
    {
        /// <summary>
        /// indicates whether extrapolation shall be done or not
        /// </summary>
        public bool Extrapolate { get; set; } = false;
        public double? ExtrapolationStep { get; set; }
        public double? UsedDataRange { get; set; }
        public double? DepthRange { get; set; }

        public ExtrapolationProperties() : base()
        {
        }
    }
}
