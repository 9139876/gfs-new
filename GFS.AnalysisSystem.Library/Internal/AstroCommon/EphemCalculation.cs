using GFS.AnalysisSystem.Library.Internal.AstroCommon.Enums;
using GFS.AnalysisSystem.Library.Internal.AstroCommon.Models;
using SwissEphNet;

namespace GFS.AnalysisSystem.Library.Internal.AstroCommon
{
    internal static class EphemCalculation
    {
        public static AstroParameterCalculationResult CalcPlanetLongitude(DateTime date, bool helio, PlanetType planetType)
        {
            var astroCalculationResult = CalcPlanetParametersInternal(date, helio, planetType);

            if (!astroCalculationResult.IsSuccess)
                return new AstroParameterCalculationResult { IsSuccess = false, Date = date, Error = astroCalculationResult.Error };

            return astroCalculationResult.Result!.TryGetValue(PlanetParameterType.Longitude, out var longitude)
                ? new AstroParameterCalculationResult { IsSuccess = true, Date = date, Result = longitude }
                : new AstroParameterCalculationResult { IsSuccess = false, Date = date, Error = "Долгота не была посчитана" };
        }

        public static AstroParameterCalculationResult CalcPlanetLongitude(DateTime date, Planet planet)
        {
            return CalcPlanetLongitude(date, planet.Helio, planet.PlanetType);
        }

        public static AstroCalculationResult CalcPlanetParameters(DateTime date, bool helio, PlanetType planetType)
            => CalcPlanetParametersInternal(date, helio, planetType);

        public static AstroCalculationResult CalcPlanetParameters(DateTime date, Planet planet)
            => CalcPlanetParametersInternal(date, planet.Helio, planet.PlanetType);

        private static double JulianDay(DateTime dateTimeUtc)
        {
            using var ephem = new SwissEph();

            var h = dateTimeUtc.Hour + (double)dateTimeUtc.Minute / 60 + (double)dateTimeUtc.Second / 3600;
            return ephem.swe_julday(dateTimeUtc.Year, dateTimeUtc.Month, dateTimeUtc.Day, h, 1);
        }

        private static AstroCalculationResult CalcPlanetParametersInternal(DateTime date, bool helio, PlanetType planetType)
        {
            var iflag = helio
                ? (int)(FlagType.SEFLG_MOSEPH | FlagType.SEFLG_HELCTR)
                : (int)FlagType.SEFLG_MOSEPH;

            var res = new double[6];
            string? sErr = null;

            using var ephem = new SwissEph();

            ephem.swe_calc_ut(JulianDay(date), (int)planetType, iflag, res, ref sErr);

            if (sErr is { Length: > 0 })
                return new AstroCalculationResult { IsSuccess = false, Date = date, Error = sErr };

            var result = new AstroCalculationResult { IsSuccess = true, Date = date, Result = new Dictionary<PlanetParameterType, double>() };

            foreach (var planetParameterType in Enum.GetValues<PlanetParameterType>())
            {
                if ((int)planetParameterType < res.Length)
                    result.Result.Add(planetParameterType, res[(int)PlanetParameterType.Longitude]);
            }

            return result;
        }
    }
}