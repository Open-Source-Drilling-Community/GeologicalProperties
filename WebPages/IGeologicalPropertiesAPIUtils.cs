using GeologicalPropertiesApp.GeologicalProperties.ModelShared;

namespace NORCE.Drilling.GeologicalProperties.WebPages;

public interface IGeologicalPropertiesAPIUtils
{
    string HostNameGeologicalProperties { get; }
    string HostBasePathGeologicalProperties { get; }
    HttpClient HttpClientGeologicalProperties { get; }
    Client ClientGeologicalProperties { get; }

    string HostNameField { get; }
    string HostBasePathField { get; }
    HttpClient HttpClientField { get; }
    Client ClientField { get; }

    string HostNameCluster { get; }
    string HostBasePathCluster { get; }
    HttpClient HttpClientCluster { get; }
    Client ClientCluster { get; }

    string HostNameWell { get; }
    string HostBasePathWell { get; }
    HttpClient HttpClientWell { get; }
    Client ClientWell { get; }

    string HostNameWellBore { get; }
    string HostBasePathWellBore { get; }
    HttpClient HttpClientWellBore { get; }
    Client ClientWellBore { get; }

    string HostNameUnitConversion { get; }
    string HostBasePathUnitConversion { get; }
}
