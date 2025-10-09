namespace GeologicalProperties.Model
{
    public class InterpolationProperties
    {
        /// <summary>
        /// indicates whether interpolation shall be done or not
        /// </summary>
        public bool Interpolate { get; set; } = false;
        public double? InterpolationStep { get; set; }        
        public InterpolationProperties() : base()
        {
        }
    }
}
