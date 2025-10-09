using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using OSDC.DotnetLibraries.General.Statistics;
using System;
using System.Collections.Generic;

namespace GeologicalProperties.Model
{
    public class GeologicalPropertiesInterpolationCase : GeologicalPropertiesInterpolationCaseLight
    {
  
        /// <summary>
        /// Interpolated table with geological properties along measured depth
        /// </summary>
        public List<GeologicalPropertyEntry>? GeologicalPropertyInterpolatedTable { get; set; } = null;

        /// <summary>
        /// Extrapolated table with geological properties along measured depth
        /// </summary>
        public List<GeologicalPropertyEntry>? GeologicalPropertyExtrapolatedTable { get; set; } = null;

        /// <summary>
        /// Holds interpolation properties of the data
        /// </summary>
        public InterpolationProperties? InterpolationProperties { get; set; } = null;

        /// <summary>
        /// Holds extrapolation properties of the data
        /// </summary>
        public ExtrapolationProperties? ExtrapolationProperties { get; set; } = null;

        /// <summary>
        /// default constructor required for JSON serialization
        /// </summary>
        public GeologicalPropertiesInterpolationCase() : base()
        {
        }

        /// <summary>
        /// main calculation method of the GeologicalPropertiesInterpolationCase
        /// </summary>
        /// <returns></returns>
        public bool Calculate(GeologicalProperties? geologicalProperties)
        {
            bool success = false;
            if (geologicalProperties != null && geologicalProperties.GeologicalPropertyTable != null)
            {
                try
                {
                    //Set all data from table to measured
                    for (int i = 0; i < geologicalProperties.GeologicalPropertyTable.Count; i++)
                    {
                        geologicalProperties.GeologicalPropertyTable[i].DataType = GeologicalPropertyTableOrigin.Measured;
                    }
                    #region CCS Calculation
                    for (int i = 0; i < geologicalProperties.GeologicalPropertyTable.Count; i++)
                    {
                        double? ucsValue = geologicalProperties.GeologicalPropertyTable[i].UnconfinedCompressiveStrength?.GaussianValue.Mean;
                        double? internalFrictionAngleValue = geologicalProperties.GeologicalPropertyTable[i].InternalFrictionAngle?.GaussianValue.Mean;
                        double? pressureDifferential = geologicalProperties.GeologicalPropertyTable[i].PressureDifferential?.GaussianValue.Mean;
                        if (ucsValue != null && internalFrictionAngleValue != null && pressureDifferential != null)
                        {
                            double phi = (double)internalFrictionAngleValue;
                            //  Equation extracted from:
                            //  Confined compressive strength model of rock for drilling optimization, doi:https://doi.org/10.1016/j.petlm.2015.03.002                        
                            double ccsValue = (double)(ucsValue + pressureDifferential + 2 * pressureDifferential * Math.Sin(phi / (1.0 - Math.Sin(phi))));
                            if (geologicalProperties.GeologicalPropertyTable[i].ConfinedCompressiveStrength != null)
                            {
                                geologicalProperties.GeologicalPropertyTable[i].ConfinedCompressiveStrength!.GaussianValue.Mean = ccsValue;
                            }
                        }
                        else
                        {
                            if (geologicalProperties.GeologicalPropertyTable[i].ConfinedCompressiveStrength != null)
                            {
                                geologicalProperties.GeologicalPropertyTable[i].ConfinedCompressiveStrength!.GaussianValue.Mean = null;
                            }
                        }
                    }
                    #endregion
                    #region Interpolation
                    if (InterpolationProperties != null && InterpolationProperties.Interpolate && this.InterpolationProperties.InterpolationStep != null)
                    {
                        List<GeologicalPropertyEntry> geologicalPropertyTable = new();
                        int oldIndex = 0;

                        double step = (double)InterpolationProperties.InterpolationStep;
                        double initialDepth = (double)geologicalProperties.GeologicalPropertyTable[0].MeasuredDepth?.GaussianValue.Mean!;
                        double finalDepth = (double)geologicalProperties.GeologicalPropertyTable[geologicalProperties.GeologicalPropertyTable.Count - 1].MeasuredDepth?.GaussianValue.Mean!;
                        int newSize = (int)Math.Round((finalDepth - initialDepth) / step);
                        for (int newIndex = 0; newIndex < newSize; newIndex++)
                        {
                            double currentPosition = newIndex * step + initialDepth;

                            //Check if it is within limits
                            if (oldIndex + 1 <= geologicalProperties.GeologicalPropertyTable.Count)
                            {
                                //Check if the current position is within the right table elements
                                if (currentPosition > geologicalProperties.GeologicalPropertyTable[oldIndex + 1].MeasuredDepth?.GaussianValue.Mean)
                                {
                                    oldIndex += 1;
                                }
                                //Calculate interpolations ratio
                                double initialMeasuredDepth = (double)geologicalProperties.GeologicalPropertyTable[oldIndex].MeasuredDepth?.GaussianValue.Mean!;
                                double finalMeasuredDepth = (double)geologicalProperties.GeologicalPropertyTable[oldIndex + 1].MeasuredDepth?.GaussianValue.Mean!;
                                double linearRatio = (currentPosition - initialMeasuredDepth) / (finalMeasuredDepth - initialMeasuredDepth);
                                //Create initial and final properties variables                            
                                double? initialInternalFrictionAngle = geologicalProperties.GeologicalPropertyTable[oldIndex].InternalFrictionAngle?.GaussianValue.Mean;
                                double? initialUCS = geologicalProperties.GeologicalPropertyTable[oldIndex].UnconfinedCompressiveStrength?.GaussianValue.Mean;
                                double? initialCCS = geologicalProperties.GeologicalPropertyTable[oldIndex].ConfinedCompressiveStrength?.GaussianValue.Mean;
                                double? initialPressure = geologicalProperties.GeologicalPropertyTable[oldIndex].PressureDifferential?.GaussianValue.Mean;
                                double? initialPorosity = geologicalProperties.GeologicalPropertyTable[oldIndex].Porosity?.GaussianValue.Mean;
                                double? initialPermeability = geologicalProperties.GeologicalPropertyTable[oldIndex].Permeability?.GaussianValue.Mean;

                                double? finalInternalFrictionAngle = geologicalProperties.GeologicalPropertyTable[oldIndex + 1].InternalFrictionAngle?.GaussianValue.Mean;
                                double? finalUCS = geologicalProperties.GeologicalPropertyTable[oldIndex + 1].UnconfinedCompressiveStrength?.GaussianValue.Mean;
                                double? finalCCS = geologicalProperties.GeologicalPropertyTable[oldIndex + 1].ConfinedCompressiveStrength?.GaussianValue.Mean;
                                double? finalPorosity = geologicalProperties.GeologicalPropertyTable[oldIndex + 1].Porosity?.GaussianValue.Mean;
                                double? finalPressure = geologicalProperties.GeologicalPropertyTable[oldIndex + 1].PressureDifferential?.GaussianValue.Mean;
                                double? finalPermeability = geologicalProperties.GeologicalPropertyTable[oldIndex + 1].Permeability?.GaussianValue.Mean;

                                //Check if it is null and interpolate value
                                double? currentInternalFrictionAngle = null;
                                double? currentUCS = null;
                                double? currentCCS = null;
                                double? currentPorosity = null;
                                double? currentPressure = null;
                                double? currentPermeability = null;


                                if (initialInternalFrictionAngle != null && finalInternalFrictionAngle != null)
                                {

                                    currentInternalFrictionAngle = initialInternalFrictionAngle + linearRatio *
                                        (finalInternalFrictionAngle - initialInternalFrictionAngle);
                                }
                                if (initialUCS != null && finalUCS != null)
                                {
                                    currentUCS = initialUCS + linearRatio * (finalUCS - initialUCS);
                                }
                                if (finalCCS != null && initialCCS != null)
                                {
                                    currentCCS = initialCCS + linearRatio * (finalCCS - initialCCS);
                                }
                                if (finalPorosity != null && initialPorosity != null)
                                {
                                    currentPorosity = initialPorosity + linearRatio * (finalPorosity - initialPorosity);
                                }
                                if (finalPressure != null && initialPressure != null)
                                {
                                    currentPressure = initialPressure + linearRatio * (finalPressure - initialPressure);
                                }
                                if (finalPermeability != null && initialPermeability != null)
                                {
                                    currentPermeability = initialPermeability + linearRatio * (finalPermeability - initialPermeability);
                                }
                                //Add to the new table
                                geologicalPropertyTable.Add(
                                    new GeologicalPropertyEntry
                                    {
                                        InternalFrictionAngle = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = currentInternalFrictionAngle
                                            }
                                        },
                                        UnconfinedCompressiveStrength = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = currentUCS
                                            }
                                        },
                                        ConfinedCompressiveStrength = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = currentCCS
                                            }
                                        },
                                        Porosity = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = currentPorosity
                                            }
                                        },
                                        Permeability = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = currentPermeability
                                            }
                                        },
                                        MeasuredDepth = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = currentPosition
                                            }
                                        },
                                        PressureDifferential = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = currentPressure
                                            }
                                        },
                                        DataType = GeologicalPropertyTableOrigin.Interpolated
                                    }
                                );//Close Add
                            }//Close if inside actual loop
                        }//Close loop through new steps
                         //Check if there are any values in the new table
                        if (geologicalPropertyTable.Count > 0)
                        {
                            //Update instance with new table
                            this.GeologicalPropertyInterpolatedTable = geologicalPropertyTable;
                        }
                    }//Close if interpolation active
                    #endregion
                    #region Extrapolation
                    try
                    {
                        if (ExtrapolationProperties != null &&
                            ExtrapolationProperties.Extrapolate &&
                            ExtrapolationProperties.ExtrapolationStep != null &&
                            ExtrapolationProperties.DepthRange != null &&
                            ExtrapolationProperties.UsedDataRange != null
                            )
                        {
                            //Create new table for geological properties
                            List<GeologicalPropertyEntry> geologicalPropertyTable = new();
                            List<double> measuredDepthList = new();
                            List<double> internalAngleList = new();
                            List<double> porosityList = new();
                            List<double> permeabilityList = new();
                            List<double> ucsList = new();
                            List<double> pressureList = new();
                            List<double> ccsList = new();

                            int tableSize = geologicalProperties.GeologicalPropertyTable.Count - 1;
                            int i = geologicalProperties.GeologicalPropertyTable.Count - 1;
                            //Populate list with all the values that are necessary to calculate the LSQM
                            while (geologicalProperties.GeologicalPropertyTable[i].MeasuredDepth?.GaussianValue.Mean >= ExtrapolationProperties.UsedDataRange)
                            {
                                measuredDepthList.Add((double)geologicalProperties.GeologicalPropertyTable[i].MeasuredDepth?.GaussianValue.Mean!);
                                internalAngleList.Add((double)geologicalProperties.GeologicalPropertyTable[i].InternalFrictionAngle?.GaussianValue.Mean!);
                                porosityList.Add((double)geologicalProperties.GeologicalPropertyTable[i].Porosity?.GaussianValue.Mean!);
                                permeabilityList.Add((double)geologicalProperties.GeologicalPropertyTable[i].Permeability?.GaussianValue.Mean!);
                                ucsList.Add((double)geologicalProperties.GeologicalPropertyTable[i].UnconfinedCompressiveStrength?.GaussianValue.Mean!);
                                pressureList.Add((double)geologicalProperties.GeologicalPropertyTable[i].PressureDifferential?.GaussianValue.Mean!);
                                ccsList.Add((double)geologicalProperties.GeologicalPropertyTable[i].ConfinedCompressiveStrength?.GaussianValue.Mean!);
                                i -= 1;
                            }
                            //If no values are available in the range, use the last 2 values. 
                            if (measuredDepthList.Count < 2)
                            {
                                measuredDepthList = new List<double>
                            {
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize - 1].MeasuredDepth?.GaussianValue.Mean!,
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize].MeasuredDepth?.GaussianValue.Mean!
                            };
                                internalAngleList = new List<double>
                            {
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize - 1].InternalFrictionAngle?.GaussianValue.Mean!,
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize].InternalFrictionAngle?.GaussianValue.Mean!
                            };
                                porosityList = new List<double>
                            {
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize - 1].Porosity?.GaussianValue.Mean!,
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize].Porosity?.GaussianValue.Mean!
                            };
                                permeabilityList = new List<double>
                            {
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize - 1].Permeability?.GaussianValue.Mean!,
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize].Permeability?.GaussianValue.Mean!
                            };
                                ucsList = new List<double>
                            {
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize - 1].UnconfinedCompressiveStrength?.GaussianValue.Mean!,
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize].UnconfinedCompressiveStrength?.GaussianValue.Mean!
                            };
                                pressureList = new List<double>
                            {
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize - 1].PressureDifferential?.GaussianValue.Mean!,
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize].PressureDifferential?.GaussianValue.Mean!
                            };
                                ccsList = new List<double>
                            {
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize - 1].PressureDifferential?.GaussianValue.Mean!,
                                (double)geologicalProperties.GeologicalPropertyTable[tableSize].PressureDifferential?.GaussianValue.Mean!
                            };
                            }
                            double[] coefInternalAngle = LSQM.CalculateCoefficientsLine(measuredDepthList, internalAngleList);
                            double[] coefPorosity = LSQM.CalculateCoefficientsLine(measuredDepthList, porosityList);
                            double[] coefPermeability = LSQM.CalculateCoefficientsLine(measuredDepthList, permeabilityList);
                            double[] coefUCS = LSQM.CalculateCoefficientsLine(measuredDepthList, ucsList);
                            double[] coefPressure = LSQM.CalculateCoefficientsLine(measuredDepthList, pressureList);
                            double[] coefCCS = LSQM.CalculateCoefficientsLine(measuredDepthList, ccsList);

                            double intialDepth = (double)geologicalProperties.GeologicalPropertyTable[tableSize].MeasuredDepth?.GaussianValue.Mean!;
                            double finalDepth = (double)ExtrapolationProperties.DepthRange;
                            double step = (double)ExtrapolationProperties.ExtrapolationStep;

                            for (double depth = intialDepth; depth <= finalDepth; depth += step)
                            {
                                //Add to the new table
                                geologicalPropertyTable.Add(
                                    new GeologicalPropertyEntry
                                    {
                                        InternalFrictionAngle = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = coefInternalAngle[0] * depth + coefInternalAngle[1]
                                            }
                                        },
                                        UnconfinedCompressiveStrength = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = coefUCS[0] * depth + coefUCS[1]
                                            }
                                        },
                                        ConfinedCompressiveStrength = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = coefCCS[0] * depth + coefCCS[1]
                                            }
                                        },
                                        Porosity = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = coefPorosity[0] * depth + coefPorosity[1]
                                            }
                                        },
                                        Permeability = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = coefPermeability[0] * depth + coefPermeability[1]
                                            }
                                        },
                                        MeasuredDepth = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = depth
                                            }
                                        },
                                        PressureDifferential = new GaussianDrillingProperty
                                        {
                                            GaussianValue = new GaussianDistribution
                                            {
                                                Mean = coefPressure[0] * depth + coefPressure[1]
                                            }
                                        },
                                        DataType = GeologicalPropertyTableOrigin.Extrapolated
                                    }
                                );//Close Add                        
                            }//Close populate loop
                            if (geologicalPropertyTable.Count > 0)
                            {
                                //Update instance with new table
                                this.GeologicalPropertyExtrapolatedTable = geologicalPropertyTable;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Least Squares Method failed:" + ex);
                    }
                    #endregion
                    success = true;
                }
                catch (Exception)
                {

                }
            }
            return success;
        }
    }
}

