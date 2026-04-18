using GeologicalPropertiesApp.GeologicalProperties.ModelShared;
using OSDC.DotnetLibraries.Drilling.WebAppUtils;

namespace NORCE.Drilling.GeologicalProperties.WebPages;

public class GeologicalPropertiesAPIUtils : APIUtils, IGeologicalPropertiesAPIUtils
{
    public GeologicalPropertiesAPIUtils(IGeologicalPropertiesWebPagesConfiguration configuration)
    {
        HostNameGeologicalProperties = Require(configuration.GeologicalPropertiesHostURL, nameof(configuration.GeologicalPropertiesHostURL));
        HostBasePathGeologicalProperties = "GeologicalProperties/api/";
        HttpClientGeologicalProperties = SetHttpClient(HostNameGeologicalProperties, HostBasePathGeologicalProperties);
        ClientGeologicalProperties = new Client(HttpClientGeologicalProperties.BaseAddress!.ToString(), HttpClientGeologicalProperties);

        HostNameField = Require(configuration.FieldHostURL, nameof(configuration.FieldHostURL));
        HostBasePathField = "Field/api/";
        HttpClientField = SetHttpClient(HostNameField, HostBasePathField);
        ClientField = new Client(HttpClientField.BaseAddress!.ToString(), HttpClientField);

        HostNameCluster = Require(configuration.ClusterHostURL, nameof(configuration.ClusterHostURL));
        HostBasePathCluster = "Cluster/api/";
        HttpClientCluster = SetHttpClient(HostNameCluster, HostBasePathCluster);
        ClientCluster = new Client(HttpClientCluster.BaseAddress!.ToString(), HttpClientCluster);

        HostNameWell = Require(configuration.WellHostURL, nameof(configuration.WellHostURL));
        HostBasePathWell = "Well/api/";
        HttpClientWell = SetHttpClient(HostNameWell, HostBasePathWell);
        ClientWell = new Client(HttpClientWell.BaseAddress!.ToString(), HttpClientWell);

        HostNameWellBore = Require(configuration.WellBoreHostURL, nameof(configuration.WellBoreHostURL));
        HostBasePathWellBore = "WellBore/api/";
        HttpClientWellBore = SetHttpClient(HostNameWellBore, HostBasePathWellBore);
        ClientWellBore = new Client(HttpClientWellBore.BaseAddress!.ToString(), HttpClientWellBore);

        HostNameUnitConversion = Require(configuration.UnitConversionHostURL, nameof(configuration.UnitConversionHostURL));
        HostBasePathUnitConversion = "UnitConversion/api/";
    }

    public string HostNameGeologicalProperties { get; }
    public string HostBasePathGeologicalProperties { get; }
    public HttpClient HttpClientGeologicalProperties { get; }
    public Client ClientGeologicalProperties { get; }

    public string HostNameField { get; }
    public string HostBasePathField { get; }
    public HttpClient HttpClientField { get; }
    public Client ClientField { get; }

    public string HostNameCluster { get; }
    public string HostBasePathCluster { get; }
    public HttpClient HttpClientCluster { get; }
    public Client ClientCluster { get; }

    public string HostNameWell { get; }
    public string HostBasePathWell { get; }
    public HttpClient HttpClientWell { get; }
    public Client ClientWell { get; }

    public string HostNameWellBore { get; }
    public string HostBasePathWellBore { get; }
    public HttpClient HttpClientWellBore { get; }
    public Client ClientWellBore { get; }

    public string HostNameUnitConversion { get; }
    public string HostBasePathUnitConversion { get; }

    private static string Require(string? value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Missing required host URL configuration: {propertyName}");
        }

        return value;
    }
}
