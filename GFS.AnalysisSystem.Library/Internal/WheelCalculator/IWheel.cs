namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator;

public interface IWheel
{
    int Number { get; }
    bool ContainNumber(int number);
    decimal GetNumberAngle(int number);
    IWheel Create(IWheel? previous);
    bool IsLastWheel();
}