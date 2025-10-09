using OSDC.UnitConversion.DrillingRazorMudComponents;

public static class DataUtils
{
    // default values
    public const double DEFAULT_VALUE = 999.25;
    public static string DEFAULT_NAME_GeologicalProperties = "Default GeologicalProperties Name";
    public static string DEFAULT_DESCR_GeologicalProperties = "Default GeologicalProperties Description";

    public static string FLOATING_COLOUR = "rgba(70, 50, 240, 0.86)";
    public static string FLOATING_COLOUR_DEEP = "rgba(232, 230, 241, 0.86)";
    public static string FLOATING_POSITION = "absolute; top: 45%; left: 20%; width: 75%";

    // unit management
    public static class UnitAndReferenceParameters
    {
        public static string? UnitSystemName { get; set; } = "Metric";
        public static string? DepthReferenceName { get; set; }
        public static string? PositionReferenceName { get; set; }
        public static string? AzimuthReferenceName { get; set; }
        public static string? PressureReferenceName { get; set; }
        public static string? DateReferenceName { get; set; }
        public static GroundMudLineDepthReferenceSource GroundMudLineDepthReferenceSource { get; set; } = new GroundMudLineDepthReferenceSource();
        public static RotaryTableDepthReferenceSource RotaryTableDepthReferenceSource { get; set; } = new RotaryTableDepthReferenceSource();
        public static SeaWaterLevelDepthReferenceSource SeaWaterLevelDepthReferenceSource { get; set; } = new SeaWaterLevelDepthReferenceSource();
        public static WellHeadDepthReferenceSource WellHeadDepthReferenceSource { get; set; } = new WellHeadDepthReferenceSource();
    }

    public static void UpdateUnitSystemName(string val)
    {
        UnitAndReferenceParameters.UnitSystemName = (string)val;
    }

    // units and labels
    public static readonly string GeologicalPropertiesPermeabilityListLabel = "PermeabilityList";
    public static readonly string GeologicalPropertiesOutputParamLabel = "GeologicalPropertiesOutputParam";
    public static readonly string GeologicalPropertiesNameLabel = "GeologicalProperties name";
    public static readonly string GeologicalPropertiesDescrLabel = "GeologicalProperties description";
    public static readonly string GeologicalPropertiesOutputParamQty = "DepthDrilling";

    public static readonly string GeologicalPropertiesInterpolationCaseNameLabel = "Geological Properties Interpolation Case name";
    public static readonly string GeologicalPropertiesInterpolationCaseDescrLabel = "Geological Properties Interpolation Case description";

    public static readonly string PermeabilityNameLabel = "Permeability name";
    public static readonly string PermeabilityParamLabel = "PermeabilityParam";
    public static readonly string PermeabilityParamQty = "DepthDrilling";

    public static readonly string PermeabilityTypeLabel = "Permeability type";

    public static readonly string InputXValuesTitle = "X value";
    public static readonly string InputXValuesQty = "DepthDrilling";
    public static readonly string InputYValuesTitle = "Y value";
    public static readonly string InputYValuesQty = "Length";
    public static readonly string OutputXValuesTitle = "X value";
    public static readonly string OutputXValuesQty = "DepthDrilling";
    public static readonly string OutputYValuesTitle = "Y value";
    public static readonly string OutputYValuesQty = "Length";
}