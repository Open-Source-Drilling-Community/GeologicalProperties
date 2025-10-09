using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.DotnetLibraries.General.DataManagement;
using OSDC.DotnetLibraries.General.Statistics;
using System;
using System.Collections.Generic;

namespace GeologicalProperties.Model
{
    public class GeologicalProperties : GeologicalPropertiesLight
    {
        /// <summary>
        /// Table with geological properties along measured depth
        /// </summary>
        public List<GeologicalPropertyEntry>? GeologicalPropertyTable { get; set; } = null;

        /// <summary>
        /// default constructor required for JSON serialization
        /// </summary>
        public GeologicalProperties() : base()
        {
        }

    }
}
