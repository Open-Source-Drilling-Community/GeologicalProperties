using NORCE.Drilling.GeologicalProperties.WebPages;

namespace NORCE.Drilling.GeologicalProperties.WebApp;

public class WebPagesHostConfiguration : IGeologicalPropertiesWebPagesConfiguration
{
    public string? FieldHostURL { get; set; } = string.Empty;
    public string? ClusterHostURL { get; set; } = string.Empty;
    public string? WellHostURL { get; set; } = string.Empty;
    public string? WellBoreHostURL { get; set; } = string.Empty;
    public string? GeologicalPropertiesHostURL { get; set; } = string.Empty;
    public string? UnitConversionHostURL { get; set; } = string.Empty;
}
