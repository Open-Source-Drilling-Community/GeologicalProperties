using GeologicalPropertiesApp.GeologicalProperties.ModelShared;

namespace NORCE.Drilling.GeologicalProperties.WebPages.Shared;

public class ConversionsFromOSDC
{
    public static double? GaussianToDouble(GaussianDrillingProperty? val)
    {
        if (val == null)
        {
            return null;
        }
        else
        {
            if (val.GaussianValue == null)
            {
                return null;
            }
            else
            {
                double? valOut = val.GaussianValue.Mean;
                if (valOut != null)
                {
                    return (double)valOut;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    public static GaussianDrillingProperty DoubleToGaussian(double? val)
    {
        GaussianDrillingProperty gaussValue = new GaussianDrillingProperty()
        {
            GaussianValue = new GaussianDistribution() { Mean = val }
        };
        return gaussValue;
    }
}
