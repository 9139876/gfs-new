using GFS.Common.Attributes;

namespace GFS.AnalysisSystem.Library.Calculation.Methods.Astro;

public enum HarmonicTypeEnum
{
    /// <summary>
    /// Точное положение
    /// </summary>
    [Description("Точное положение")]
    Exact = 0,

    /// <summary>
    /// Гармоника 60
    /// </summary>
    [Description("Гармоника 60")]
    Harmonic60 = 60,

    /// <summary>
    /// Гармоника 90
    /// </summary>
    [Description("Гармоника 90")]
    Harmonic90 = 90,

    /// <summary>
    /// Гармоника 120
    /// </summary>
    [Description("Гармоника 120")]
    Harmonic120 = 120,

    /// <summary>
    /// Гармоника 180
    /// </summary>
    [Description("Гармоника 180")]
    Harmonic180 = 180,

    /// <summary>
    /// Гармоника 240
    /// </summary>
    [Description("Гармоника 240")]
    Harmonic240 = 240,

    /// <summary>
    /// Гармоника 270
    /// </summary>
    [Description("Гармоника 270")]
    Harmonic270 = 270,

    /// <summary>
    /// Гармоника 300
    /// </summary>
    [Description("Гармоника 300")]
    Harmonic300 = 300
}