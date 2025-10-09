using OSDC.DotnetLibraries.General.DataManagement;
using System;

namespace GeologicalProperties.Model
{
    /// <summary>
    /// Light weight version of a GeologicalProperties
    /// Used to avoid loading the complete GeologicalProperties (heavy weight data) each time we only need contextual info on the data
    /// Typically used for listing, sorting and filtering purposes
    /// </summary>
    public class GeologicalPropertiesLight
    {
        /// <summary>
        /// a MetaInfo for the GeologicalPropertiesLight
        /// </summary>
        public MetaInfo? MetaInfo { get; set; }

        /// <summary>
        /// name of the data
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// a description of the data
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// the date when the data was created
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; }

        /// <summary>
        /// the date when the data was last modified
        /// </summary>
        public DateTimeOffset? LastModificationDate { get; set; }

        /// <summary>
        /// ID of the WellBore
        /// </summary>
        public Guid? WellBoreID { get; set; } = null;

        /// <summary>
        /// ID of the Trajectory
        /// </summary>
        public Guid? TrajectoryID { get; set; } = null;

        /// <summary>
        /// indicates whether these are prognosed geological properties
        /// </summary>
        public bool IsPrognosed { get; set; } = false;

        /// <summary>
        /// default constructor required for parsing the data model as a json file
        /// </summary>
        public GeologicalPropertiesLight() : base()
        {
        }

        /// <summary>
        /// base constructor
        /// </summary>
        public GeologicalPropertiesLight(MetaInfo? metaInfo, string? name, string? descr, DateTimeOffset? creationDate, DateTimeOffset? modifDate, Guid? wellBoreID, Guid? trajectoryID, bool isPrognosed)
        {
            MetaInfo = metaInfo;
            Name = name;
            Description = descr;
            CreationDate = creationDate;
            LastModificationDate = modifDate;
            WellBoreID = wellBoreID;
            TrajectoryID = trajectoryID;
            IsPrognosed = isPrognosed;
        }
    }
}
