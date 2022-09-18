using GFS.BalanceCalculation.BL.Auxiliary.Astro.Enums;
using GFS.BalanceCalculation.BL.Auxiliary.Astro.Models;
using SwissEphNet;

namespace GFS.BalanceCalculation.BL.Auxiliary.Astro
{
    public static class EphemCalculation
    {
        public static double JulianDay(DateTime dateTimeUtc)
        {
            using var ephem = new SwissEph();

            var h = dateTimeUtc.Hour + (double)dateTimeUtc.Minute / 60 + (double)dateTimeUtc.Second / 3600;
            return ephem.swe_julday(dateTimeUtc.Year, dateTimeUtc.Month, dateTimeUtc.Day, h, 1);
        }

        public static AstroCalculationResult CalcLongitude(DateTime date, Planet planet)
            => CalcPlanetParameter(date, planet.Helio, (int) planet.PlanetType);

        public static AstroCalculationResult CalcLongitude(DateTime date, bool helio, PlanetType planetType)
            => CalcPlanetParameter(date, helio, (int) planetType);

        private static AstroCalculationResult CalcPlanetParameter(DateTime date, bool helio, int numPl, PlanetParameterType parameter = PlanetParameterType.Longitude)
        {
            var iflag = helio
                ? (int) (FlagType.SEFLG_MOSEPH | FlagType.SEFLG_HELCTR)
                : (int) FlagType.SEFLG_MOSEPH;

            double[] res = new double[6];
            string sErr = null;

            using var ephem = new SwissEph();

            ephem.swe_calc_ut(JulianDay(date), numPl, iflag, res, ref sErr);

            return sErr is {Length: > 0}
                ? new AstroCalculationResult {IsSuccess = false, Error = sErr}
                : new AstroCalculationResult() {IsSuccess = true, Longitude = res[(int) parameter]};
        }
    }
}