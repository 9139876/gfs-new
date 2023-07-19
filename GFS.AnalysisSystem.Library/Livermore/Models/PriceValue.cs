namespace GFS.AnalysisSystem.Library.Livermore.Models;

public interface IPriceValue
{
    bool IsMoreOnSixPointsThan(decimal value);
    bool IsMoreOnThreePointsThan(decimal value);
    bool IsLessOnSixPointsThan(decimal value);
    bool IsLessOnThreePointsThan(decimal value);
    bool IsMoreThan(decimal value);
    bool IsLessThan(decimal value);
}

public class PriceValueAbsoluteComparison : IPriceValue
{
    private readonly decimal _price;
    private readonly decimal _kPrice;

    public PriceValueAbsoluteComparison(decimal price, decimal kPrice)
    {
        _price = price;
        _kPrice = kPrice;
    }

    public bool IsMoreOnSixPointsThan(decimal value) => _price - value > 6 / _kPrice;
    public bool IsMoreOnThreePointsThan(decimal value) => _price - value > 3 / _kPrice;
    public bool IsLessOnSixPointsThan(decimal value) => value - _price > 6 / _kPrice;
    public bool IsLessOnThreePointsThan(decimal value) => value - _price > 3 / _kPrice;
    public bool IsMoreThan(decimal value) => _price > value;
    public bool IsLessThan(decimal value) => _price < value;
}

public class PriceValuePercentageComparison : IPriceValue
{
    private readonly decimal _price;
    private readonly decimal _threePercent;
    private readonly decimal _sixPercent;

    public PriceValuePercentageComparison(decimal price)
    {
        _price = price;
        _threePercent = price * 0.03m;
        _sixPercent = price * 0.06m;
    }

    public bool IsMoreOnSixPointsThan(decimal value) => _price > value + _sixPercent;
    public bool IsMoreOnThreePointsThan(decimal value) => _price > value + _threePercent;
    public bool IsLessOnSixPointsThan(decimal value) => _price < value - _sixPercent;
    public bool IsLessOnThreePointsThan(decimal value) => _price < value - _threePercent;
    public bool IsMoreThan(decimal value) => _price > value;
    public bool IsLessThan(decimal value) => _price < value;
}