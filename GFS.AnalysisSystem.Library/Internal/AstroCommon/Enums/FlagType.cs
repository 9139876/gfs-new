using System.Diagnostics.CodeAnalysis;

namespace GFS.AnalysisSystem.Library.Internal.AstroCommon.Enums
{
    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum FlagType
    {
        ///Use JPL ephemeris
        SEFLG_JPLEPH = 1,

        ///Use SWISSEPH ephemeris, default
        SEFLG_SWIEPH = 2,

        ///Use Moshier ephemeris
        SEFLG_MOSEPH = 4,

        
        ///Return heliocentric position
        SEFLG_HELCTR = 8,

        ///Return true positions, not apparent
        SEFLG_TRUEPOS = 16,

        ///No precession, i.e. give J2000 equinox
        SEFLG_J2000 = 32,

        //No nutation, i.e. mean equinox of date
        SEFLG_NONUT = 64,

        ///Speed from 3 positions (do not use it, SEFLG_SPEED is faster and preciser.)
        SEFLG_SPEED3 = 128,

        ///High precision speed (analyt. comp.)
        SEFLG_SPEED = 256,

        ///Turn off gravitational deflection
        SEFLG_NOGDEFL = 512,

        ///Turn off 'annual' aberration of light
        SEFLG_NOABERR = 1024,

        ///Astrometric positions
        SEFLG_ASTROMETRIC = SEFLG_NOABERR | SEFLG_NOGDEFL,

        ///Equatorial positions are wanted
        SEFLG_EQUATORIAL = 2048,

        ///Cartesian, not polar, coordinates
        SEFLG_XYZ = 4096,

        ///Coordinates in radians, not degrees
        SEFLG_RADIANS = 8192,

        ///Barycentric positions
        SEFLG_BARYCTR = 16384,

        ///Topocentric positions
        SEFLG_TOPOCTR = 32 * 1024,

        ///Sidereal positions
        SEFLG_SIDEREAL = 64 * 1024,

        ///ICRS (DE406 reference frame)
        SEFLG_ICRS = 128 * 1024,

        ///Reproduce JPL Horizons
        SEFLG_DPSIDEPS_1980 = 256 * 1024
    }
}