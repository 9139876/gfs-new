// ReSharper disable VirtualMemberCallInConstructor

namespace GFS.AnalysisSystem.Library.Internal.WheelCalculator;

public abstract class AbstractWheel
{
    protected readonly uint FirstCellNumber;
    protected readonly uint LastCellNumber;

    protected AbstractWheel(AbstractWheel? previous)
    {
        Number = (ushort)((previous?.Number ?? 0) + 1);
        FirstCellNumber = GetFirstCellNumber(Number);
        LastCellNumber = GetLastCellNumber(Number);
    }

    public ushort Number { get; }

    public bool ContainNumber(int number)
        => number >= FirstCellNumber && number <= LastCellNumber;

    public virtual bool IsLastWheel()
        => LastCellNumber >= ushort.MaxValue;

    public decimal GetNumberAngle(int number)
    {
        if (number < 1)
            throw new ArgumentOutOfRangeException(nameof(number), "Число должно быть больше 0");

        if (!ContainNumber(number))
            throw new ArgumentOutOfRangeException(nameof(number), $"Число {number} не попадает в диапазон {Number} круга от {FirstCellNumber} до {LastCellNumber}");

        return GetNumberAngleInternal(number);
    }

    protected virtual uint GetFirstCellNumber(ushort number)
        => number == 1 ? 1 : GetLastCellNumber((ushort)(number - 1)) + 1;

    protected abstract uint GetLastCellNumber(ushort number);
    protected abstract decimal GetNumberAngleInternal(int number);
}