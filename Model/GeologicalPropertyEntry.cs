using OSDC.DotnetLibraries.Drilling.DrillingProperties;

namespace GeologicalProperties.Model
{
    /// <summary>
    /// a base class other classes may derive from
    /// </summary>
    public class GeologicalPropertyEntry
    {
        /// <summary>
        /// Internal friction angle along the measured depth
        /// </summary>
        public GaussianDrillingProperty? InternalFrictionAngle { get; set; } = null;
        /// <summary>
        /// Unconfined Compressive Strength (USC) along measured depth
        /// </summary>
        public GaussianDrillingProperty? UnconfinedCompressiveStrength { get; set; } = null;
        /// <summary>
        /// Confined Compressive Strength (CCS) along measured depth
        /// </summary>
        public GaussianDrillingProperty? ConfinedCompressiveStrength { get; set; } = null;
        /// <summary>
        /// Rock porosity along measured depth
        /// </summary>        
        public GaussianDrillingProperty? PressureDifferential { get; set; } = null;
        /// <summary>
        /// Rock porosity along measured depth
        /// </summary>        
        public GaussianDrillingProperty? Porosity { get; set; } = null;
        /// <summary>
        /// Rock permeability scalar along measured depth
        /// </summary>    
        public GaussianDrillingProperty? Permeability { get; set; } = null;
        /// <summary>
        /// Measured depth
        /// </summary>    
        public GaussianDrillingProperty? MeasuredDepth { get; set; } = null;
        /// <summary>
        /// Type of data set on the table
        /// </summary>    
        public GeologicalPropertyTableOrigin DataType { get; set; }                                   
        /// <summary>
        /// default constructor required for JSON serialization
        /// </summary>
        public GeologicalPropertyEntry() : base()
        {
        }
    }
}
