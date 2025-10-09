using GeologicalPropertiesApp.GeologicalProperties.ModelShared;
class PseudoConstructors
{
    public static GeologicalProperties ConstructGeologicalProperties()
    {
        MetaInfo metaInfo = new MetaInfo { ID = Guid.NewGuid() };
        DateTimeOffset creationDate = DateTimeOffset.UtcNow;
        return new GeologicalProperties()
        {
            MetaInfo = metaInfo,
            Name = "Geological Properties",
            Description = "Geological Properties Description",
            CreationDate = creationDate,
            LastModificationDate = creationDate,
            GeologicalPropertyTable = [ConstructGeologicalPropertyEntry(), ConstructGeologicalPropertyEntry()]
        };
    }

    public static GeologicalPropertiesInterpolationCase ConstructGeologicalPropertiesInterpolationCase()
    {
        MetaInfo metaInfo = new MetaInfo { ID = Guid.NewGuid() };
        DateTimeOffset creationDate = DateTimeOffset.UtcNow;
        return new GeologicalPropertiesInterpolationCase()
        {
            MetaInfo = metaInfo,
            Name = "Geological Properties Interpolation Case",
            Description = "Geological Properties Interpolation Case Description",
            CreationDate = creationDate,
            LastModificationDate = creationDate,
            InterpolationProperties = ConstructInterpolationProperties(),
            ExtrapolationProperties = ConstructExtrapolationProperties(),
        };
    }

    public static InterpolationProperties ConstructInterpolationProperties()
    {
        return new InterpolationProperties
        {
            Interpolate = false,
            InterpolationStep = null
        };
    }
    public static ExtrapolationProperties ConstructExtrapolationProperties()
    {
        return new ExtrapolationProperties
        {
            Extrapolate = false,
            ExtrapolationStep = null,
            UsedDataRange = null,
            DepthRange = null
        };
    }

    public static  GaussianDrillingProperty ConstructGaussianDrillingProperty(double? val)
    {
        return new GaussianDrillingProperty
        {
            GaussianValue = new GaussianDistribution
            {
                Mean = val
            }
        };
    }
    public static GeologicalPropertyEntry ConstructGeologicalPropertyEntry()
    {
        return new GeologicalPropertyEntry()
        {
            InternalFrictionAngle = ConstructGaussianDrillingProperty(null),
            UnconfinedCompressiveStrength = ConstructGaussianDrillingProperty(null),
            ConfinedCompressiveStrength = ConstructGaussianDrillingProperty(null),
            Porosity = ConstructGaussianDrillingProperty(null),
            Permeability = ConstructGaussianDrillingProperty(null),
            MeasuredDepth = ConstructGaussianDrillingProperty(null),
            PressureDifferential = ConstructGaussianDrillingProperty(null),
            DataType = GeologicalPropertyTableOrigin.Measured
        };
    }
    public static List<GeologicalPropertyEntry> ConstructGeologicalPropertiesTableList()
    {
        return new List<GeologicalPropertyEntry>
        {   
            ConstructGeologicalPropertyEntry()
        };
    }
}