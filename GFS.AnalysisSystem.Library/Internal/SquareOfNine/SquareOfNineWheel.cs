using System.Reflection;
using System.Text;

namespace GFS.AnalysisSystem.Library.Internal.SquareOfNine;

public class SquareOfNineWheel
    {
        internal readonly int Number;
        internal readonly int FirstCellNumber;
        internal readonly int LastCellNumber;
        // internal readonly int _numberOfCells;
        private readonly SquareOfNineWheelLine[] _lines;

        public SquareOfNineWheel(int number)
        {
            Number = number;

            FirstCellNumber = Number == 1 ? 1 : (int)Math.Pow((Number - 1) * 2 - 1, 2) + 1;
            LastCellNumber = (int)Math.Pow(Number * 2 - 1, 2);
            // _numberOfCells = _lastCellNumber - _firstCellNumber + 1;

            var (cardinalNumbers, diagonalNumbers) = GetCrosses();

            _lines = new SquareOfNineWheelLine[]
            {
                new (1, FirstCellNumber, cardinalNumbers[0], diagonalNumbers[0], this),
                new (2, diagonalNumbers[0], cardinalNumbers[1], diagonalNumbers[1], this),
                new (3, diagonalNumbers[1], cardinalNumbers[2], diagonalNumbers[2], this),
                new (4, diagonalNumbers[2], cardinalNumbers[3], diagonalNumbers[3], this),
            };
        }

        public string GetInfo()
        {
            var sb = new StringBuilder(300);

            foreach (var field in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                sb.AppendLine($"{field.Name} = {field.GetValue(this)}");
            }

            return sb.ToString();
        }

        public bool ContainNumber(int number)
        {
            return number >= FirstCellNumber && number <= LastCellNumber;
        }

        public decimal GetNumberAngle(int number)
        {
            if (!ContainNumber(number))
                throw new ArgumentOutOfRangeException(nameof(number), number.ToString());

            return _lines.First(line => line.ContainNumber(number)).GetNumberAngle(number);
        }

        private (int[], int[]) GetCrosses()
        {
            var cardinalNumbers = new int[4];
            for (var i = 0; i < 4; i++)
                cardinalNumbers[i] = FirstCellNumber + Number - 2 + 2 * (Number - 1) * i;

            var diagonalNumbers = new int[4];
            for (var i = 0; i < 4; i++)
                diagonalNumbers[i] = FirstCellNumber + 2 * (Number - 1) - 1 + 2 * (Number - 1) * i;

            return (cardinalNumbers, diagonalNumbers);
        }
    }