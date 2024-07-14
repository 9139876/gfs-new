using System.Drawing;

namespace GFS.AnalysisSystem.Library.Calculation.Models;

public class ForecastWindow
{
    private readonly Rectangle _rectangle;

    public ForecastWindow(int x1, int y1, int x2, int y2, Size sheetSize)
    {
        if (x1 < 0)
            throw new InvalidOperationException("Окно прогнозирования не может начинаться левее начала листа");

        if (x2 > sheetSize.Width)
            throw new InvalidOperationException("Окно прогнозирования не может заканчиваться правее конца листа");

        if (x2 <= x1)
            throw new InvalidOperationException("Ширина окна прогнозирования должна быть больше 0");

        if (y1 < 0)
            throw new InvalidOperationException("Окно прогнозирования не может начинаться ниже начала листа");

        if (y2 > sheetSize.Height)
            throw new InvalidOperationException("Окно прогнозирования не может заканчиваться выше конца листа");

        if (y2 <= y1)
            throw new InvalidOperationException("Высота окна прогнозирования должна быть больше 0");

        _rectangle = new Rectangle(x1, y1, x2 - x1, y2 - y1);
    }

    public bool InWindow(Point point) => _rectangle.Contains(point);
}