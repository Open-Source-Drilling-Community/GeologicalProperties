using OSDC.UnitConversion.DrillingRazorMudComponents;

public static class APIUtils
{
    // API parameters
    public static readonly string HostNameGeologicalProperties = GeologicalPropertiesApp.GeologicalProperties.WebApp.Configuration.GeologicalPropertiesHostURL!;
    public static readonly string HostBasePathGeologicalProperties = "GeologicalProperties/api/";
    public static readonly HttpClient HttpClientGeologicalProperties = APIUtils.SetHttpClient(HostNameGeologicalProperties, HostBasePathGeologicalProperties);
    public static readonly GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client ClientGeologicalProperties = new GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client(APIUtils.HttpClientGeologicalProperties.BaseAddress!.ToString(), APIUtils.HttpClientGeologicalProperties);
    // Field api
    public static readonly string HostNameField = GeologicalPropertiesApp.GeologicalProperties.WebApp.Configuration.FieldHostURL!;
    public static readonly string HostBasePathField = "Field/api/";
    public static readonly HttpClient HttpClientField = APIUtils.SetHttpClient(HostNameField, HostBasePathField);
    public static readonly GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client ClientField = new GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client(APIUtils.HttpClientField.BaseAddress!.ToString(), APIUtils.HttpClientField);
    // Cluster api
    public static readonly string HostNameCluster = GeologicalPropertiesApp.GeologicalProperties.WebApp.Configuration.ClusterHostURL!;
    public static readonly string HostBasePathCluster = "Cluster/api/";
    public static readonly HttpClient HttpClientCluster = APIUtils.SetHttpClient(HostNameCluster, HostBasePathCluster);
    public static readonly GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client ClientCluster = new GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client(APIUtils.HttpClientCluster.BaseAddress!.ToString(), APIUtils.HttpClientCluster);
    // Well api
    public static readonly string HostNameWell = GeologicalPropertiesApp.GeologicalProperties.WebApp.Configuration.WellHostURL!;
    public static readonly string HostBasePathWell = "Well/api/";
    public static readonly HttpClient HttpClientWell = APIUtils.SetHttpClient(HostNameWell, HostBasePathWell);
    public static readonly GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client ClientWell = new GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client(APIUtils.HttpClientWell.BaseAddress!.ToString(), APIUtils.HttpClientWell);
    // WellBore api
    public static readonly string HostNameWellBore = GeologicalPropertiesApp.GeologicalProperties.WebApp.Configuration.WellBoreHostURL!;
    public static readonly string HostBasePathWellBore = "WellBore/api/";
    public static readonly HttpClient HttpClientWellBore = APIUtils.SetHttpClient(HostNameWellBore, HostBasePathWellBore);
    public static readonly GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client ClientWellBore = new GeologicalPropertiesApp.GeologicalProperties.ModelShared.Client(APIUtils.HttpClientWellBore.BaseAddress!.ToString(), APIUtils.HttpClientWellBore);

    public static readonly string HostNameUnitConversion = GeologicalPropertiesApp.GeologicalProperties.WebApp.Configuration.UnitConversionHostURL!;
    public static readonly string HostBasePathUnitConversion = "UnitConversion/api/";

    // API utility methods
    public static HttpClient SetHttpClient(string host, string microServiceUri)
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }; // temporary workaround for testing purposes: bypass certificate validation (not recommended for production environments due to security risks)
        HttpClient httpClient = new(handler)
        {
            BaseAddress = new Uri(host + microServiceUri)
        };
        httpClient.DefaultRequestHeaders.Accept.Clear();
        httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        return httpClient;
    }
}
public class GroundMudLineDepthReferenceSource : IGroundMudLineDepthReferenceSource
{
    public double? GroundMudLineDepthReference { get; set; } = null;
}

public class RotaryTableDepthReferenceSource : IRotaryTableDepthReferenceSource
{
    public double? RotaryTableDepthReference { get; set; } = null;
}

public class SeaWaterLevelDepthReferenceSource : ISeaWaterLevelDepthReferenceSource
{
    public double? SeaWaterLevelDepthReference { get; set; } = null;
}
public class WellHeadDepthReferenceSource : IWellHeadDepthReferenceSource
{
    public double? WellHeadDepthReference { get; set; } = null;
}
