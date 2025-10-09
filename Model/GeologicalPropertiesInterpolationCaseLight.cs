using OSDC.DotnetLibraries.General.DataManagement;
using System;

namespace GeologicalProperties.Model
{
    public class GeologicalPropertiesInterpolationCaseLight
    {
        /// <summary>
        /// a MetaInfo for the GeologicalProperties
        /// </summary>
        public MetaInfo? MetaInfo { get; set; } = null;

        /// <summary>
        /// name of the data
        /// </summary>
        public string? Name { get; set; } = null;

        /// <summary>
        /// a description of the data
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// the date when the data was created
        /// </summary>
        public DateTimeOffset? CreationDate { get; set; } = null;

        /// <summary>
        /// the date when the data was last modified
        /// </summary>
        public DateTimeOffset? LastModificationDate { get; set; } = null;

        /// <summary>
        /// The ID of the GeologicalProperties
        /// </summary>
        public Guid? GeologicalPropertiesID { get; set; } = null;

        /// <summary>
        /// default constructor required for parsing the data model as a json file
        /// </summary>
        public GeologicalPropertiesInterpolationCaseLight() : base()
        {
        }

        /// <summary>
        /// base constructor
        /// </summary>
        public GeologicalPropertiesInterpolationCaseLight(MetaInfo? metaInfo, string? name, string? descr, DateTimeOffset? creationDate, DateTimeOffset? modifDate, Guid? geologicalPropertiesID)
        {
            MetaInfo = metaInfo;
            Name = name;
            Description = descr;
            CreationDate = creationDate;
            LastModificationDate = modifDate;
            GeologicalPropertiesID = geologicalPropertiesID;
        }
    }
}
