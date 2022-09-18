using GFS.BalanceCalculation.BL.Auxiliary.Astro;
using GFS.BalanceCalculation.BL.Auxiliary.Astro.Enums;
using Microsoft.AspNetCore.Mvc;

namespace GFS.BalanceCalculation.WebApp.Controller
{
    [Route(nameof(AstroController))]
    public class AstroController : ControllerBase
    {
        [HttpGet("calc-julday")]
        public double CalcJulday(DateTime dateTime)
        {
            return EphemCalculation.JulianDay(dateTime);
        }

        [HttpGet("calc-longitude")]
        public double CalcLongitude(DateTime dateTime, bool helio, PlanetType planetType)
        {
            var result = EphemCalculation.CalcLongitude(dateTime, helio, planetType);

            if (!result.IsSuccess)
                throw new InvalidOperationException(result.Error);

            return result.Longitude;
        }
    }
}