using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.DotnetLibraries.General.DataManagement;
using OSDC.DotnetLibraries.General.Statistics;
using GeologicalProperties.Model;

namespace GeologicalProperties.ModelTest
{
    public class Tests
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
        }
        public Model.GeologicalProperties ConstructGeologicalProperties(MetaInfo metaInfo, Model.GeologicalPropertyEntry geologicalPropertyTableRow1, Model.GeologicalPropertyEntry geologicalPropertyTableRow2)
        {
            DateTimeOffset creationDate = DateTimeOffset.UtcNow;                     
            return new Model.GeologicalProperties()
            {
                MetaInfo = metaInfo,
                Name = "My test GeologicalProperties",
                Description = "My test GeologicalProperties",
                CreationDate = creationDate,
                LastModificationDate = creationDate,
                GeologicalPropertyTable = [geologicalPropertyTableRow1, geologicalPropertyTableRow2]
            };
        }
        public GaussianDrillingProperty ConstructGaussianDrillingProperty(double? val)
        {                        
            return new GaussianDrillingProperty
            {
                GaussianValue = new GaussianDistribution
                {
                    Mean = val
                }
            };
        }
        public InterpolationProperties ConstructInterpolationProperties()
        {
            return new InterpolationProperties
            {
                Interpolate = false,
                InterpolationStep = 1.0
            };
        }
        public ExtrapolationProperties ConstructExtrapolationProperties()
        {
            return new ExtrapolationProperties
            {
                Extrapolate = false,
                ExtrapolationStep = null,
                UsedDataRange = null,
                DepthRange = null
            };
        }
        public Model.GeologicalPropertyEntry ConstructGeologicalPropertiesTable()
        {
            return new Model.GeologicalPropertyEntry()
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

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
        }
    }
}