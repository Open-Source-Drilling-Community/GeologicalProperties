using OSDC.DotnetLibraries.Drilling.WebAppUtils;

namespace NORCE.Drilling.GeologicalProperties.WebPages;

public interface IGeologicalPropertiesWebPagesConfiguration :
    IFieldHostURL,
    IClusterHostURL,
    IWellHostURL,
    IWellBoreHostURL,
    IGeologicalPropertiesHostURL,
    IUnitConversionHostURL
{
}
