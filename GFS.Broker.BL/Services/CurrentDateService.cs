namespace GFS.Broker.BL.Services;

public interface ICurrentDateService
{
    void SetCurrentDate(DateTime dateTime);

    DateTime GetCurrentDate();
}

internal class TestDateService : ICurrentDateService
{
    private DateTime _currentDate = DateTime.MinValue;
    
    public void SetCurrentDate(DateTime dateTime)
    {
        _currentDate = dateTime;
    }

    public DateTime GetCurrentDate()
    {
        return _currentDate != DateTime.MinValue
            ? _currentDate
            : throw new InvalidOperationException("Текущая дата не была задана");
    }
}

internal class RealDateService : ICurrentDateService
{
    public void SetCurrentDate(DateTime dateTime)
    {
        throw new InvalidOperationException("Нельзя назначить текущую дату для реального брокера");
    }

    public DateTime GetCurrentDate()
    {
        return DateTime.UtcNow;
    }
}