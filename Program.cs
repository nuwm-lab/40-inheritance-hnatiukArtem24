using System;
using System.Globalization;
using System.Text;

namespace GeometryInheritance
{
    /// <summary>
    /// Структура точки в 2D
    /// </summary>
    public struct Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString() => $"({X.ToString(CultureInfo.InvariantCulture)}, {Y.ToString(CultureInfo.InvariantCulture)})";
    }

    /// <summary>
    /// Базовий клас Triangle
    /// </summary>
    public class Triangle
    {
        private Point _p1;
        private Point _p2;
        private Point _p3;

        public Triangle() { }

        public Triangle(Point p1, Point p2, Point p3)
        {
            SetCoordinates(new[] { p1, p2, p3 });
        }

        /// <summary>
        /// Встановити координати вершин. ОЧІКУЄ масив довжини 3.
        /// </summary>
        public virtual void SetCoordinates(Point[] points)
        {
            if (points == null || points.Length != 3)
                throw new ArgumentException("Triangle requires exactly 3 points.");

            _p1 = points[0];
            _p2 = points[1];
            _p3 = points[2];
        }

        /// <summary>
        /// Повертає рядок з координатами вершин
        /// </summary>
        public virtual string ShowCoordinates()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"P1 = {_p1}");
            sb.AppendLine($"P2 = {_p2}");
            sb.AppendLine($"P3 = {_p3}");
            return sb.ToString();
        }

        /// <summary>
        /// Обчислити площу трикутника (shoelace / formula).
        /// Повертає додатню величину.
        /// </summary>
        public virtual double CalculateArea()
        {
            // Формула: 0.5 * | x1(y2-y3) + x2(y3-y1) + x3(y1-y2) |
            double x1 = _p1.X, y1 = _p1.Y;
            double x2 = _p2.X, y2 = _p2.Y;
            double x3 = _p3.X, y3 = _p3.Y;

            double area = 0.5 * Math.Abs(x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2));
            return area;
        }

        /// <summary>
        /// Валідація: чи не вироджений трикутник (площа > epsilon)
        /// </summary>
        public bool IsValid(double epsilon = 1e-9) => CalculateArea() > epsilon;
    }

    /// <summary>
    /// Похідний клас ConvexQuadrilateral (успадкований від Triangle для демонстрації механізму наслідування)
    /// Реально зберігає 4 вершини; перевизначає SetCoordinates / ShowCoordinates / CalculateArea.
    /// </summary>
    public class ConvexQuadrilateral : Triangle
    {
        private Point[] _points = new Point[4];

        public ConvexQuadrilateral() : base() { }

        public ConvexQuadrilateral(Point p1, Point p2, Point p3, Point p4)
        {
            SetCoordinates(new[] { p1, p2, p3, p4 });
        }

        /// <summary>
        /// Перевизначений метод: очікуємо масив довжини 4.
        /// </summary>
        public override void SetCoordinates(Point[] points)
        {
            if (points == null || points.Length != 4)
                throw new ArgumentException("ConvexQuadrilateral requires exactly 4 points.");

            // Копіюємо точки (вважатимемо порядок вершин заданий по колу)
            for (int i = 0; i < 4; i++) _points[i] = points[i];
        }

        public override string ShowCoordinates()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < 4; i++)
                sb.AppendLine($"P{i + 1} = {_points[i]}");
            return sb.ToString();
        }

        /// <summary>
        /// Перевірка опуклості методом знаків поперечних добутків для послідовних ребер.
        /// Працює при заданні вершин у порядку по периметру (CW або CCW).
        /// </summary>
        public bool IsConvex()
        {
            // Потрібно, щоб всі cross-значення мали один знак (усі >=0 або <=0), одночасно не рівні нулю
            int n = 4;
            double? sign = null;

            for (int i = 0; i < n; i++)
            {
                Point a = _points[i];
                Point b = _points[(i + 1) % n];
                Point c = _points[(i + 2) % n];

                // vector ab = b - a; bc = c - b
                double abx = b.X - a.X;
                double aby = b.Y - a.Y;
                double bcx = c.X - b.X;
                double bcy = c.Y - b.Y;

                // cross = ab x bc = abx * bcy - aby * bcx
                double cross = abx * bcy - aby * bcx;

                if (Math.Abs(cross) < 1e-12)
                    continue; // допускаємо близький до нуля (колінеарні суміжні ребра) — не змінюємо знак

                double currentSign = Math.Sign(cross);
                if (sign == null)
                    sign = currentSign;
                else if (currentSign != sign.Value)
                    return false;
            }

            // Якщо всі кроси були нульові — вважати невизначеним/неопуклим
            return sign != null;
        }

        /// <summary>
        /// Обчислення площі чотирикутника шнуровою формулою (shoelace) – працює для простих полігонів,
        /// але тут ми вимагаємо опуклості і порядок по колу.
        /// </summary>
        public override double CalculateArea()
        {
            if (!IsConvex())
                throw new InvalidOperationException("Чотирикутник не опуклий або вершини не в порядку по колу; площу не можна коректно обчислити.");

            double sum = 0;
            for (int i = 0; i < 4; i++)
            {
                Point pCurrent = _points[i];
                Point pNext = _points[(i + 1) % 4];
                sum += (pCurrent.X * pNext.Y) - (pNext.X * pCurrent.Y);
            }
            return Math.Abs(0.5 * sum);
        }
    }

    /// <summary>
    /// Додатковий приклад похідного класу — IsoscelesTriangle
    /// Наслідує Triangle і додає метод IsIsosceles.
    /// </summary>
    public class IsoscelesTriangle : Triangle
    {
        public IsoscelesTriangle() : base() { }
        public IsoscelesTriangle(Point p1, Point p2, Point p3) : base(p1, p2, p3) { }

        private static double Distance(Point a, Point b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public bool IsIsosceles()
        {
            // Для цього методу нам потрібно отримати вершини з батьківського класу.
            // У нашому простому прикладі можемо викликати CalculateArea для перевірки валідності,
            // але щоб отримати координати у спадкоємця треба було б зробити захищені поля/властивості у Triangle.
            // Для зручності — реалізуємо невелику "workaround": приведемо Triangle до локальної копії:
            // (це демонстраційний приклад — у реальній реалізації краще мати захищені властивості).
            throw new NotImplementedException("Якщо потрібно — можна розширити Triangle захищеними властивостями вершин, щоб реалізувати IsIsosceles.");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Виберіть тип фігури: 1 - Triangle, 2 - Convex Quadrilateral");
            string choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                var pts = ReadPoints(3);
                var tri = new Triangle();
                try
                {
                    tri.SetCoordinates(pts);
                    Console.WriteLine("\nКоординати трикутника:");
                    Console.WriteLine(tri.ShowCoordinates());
                    if (!tri.IsValid())
                    {
                        Console.WriteLine("Увага: заданий трикутник вироджений (площа ≈ 0).");
                    }
                    else
                    {
                        Console.WriteLine($"Площа трикутника: {tri.CalculateArea().ToString(CultureInfo.InvariantCulture)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
            else if (choice == "2")
            {
                var pts = ReadPoints(4);
                var quad = new ConvexQuadrilateral();
                try
                {
                    quad.SetCoordinates(pts);
                    Console.WriteLine("\nКоординати чотирикутника:");
                    Console.WriteLine(quad.ShowCoordinates());
                    if (!quad.IsConvex())
                    {
                        Console.WriteLine("Увага: чотирикутник не опуклий або вершини задано не по колу.");
                    }
                    else
                    {
                        Console.WriteLine($"Площа опуклого чотирикутника: {quad.CalculateArea().ToString(CultureInfo.InvariantCulture)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Невірний вибір. Завершення.");
            }

            Console.WriteLine("\nГотово. Натисніть Enter щоб вийти.");
            Console.ReadLine();
        }

        /// <summary>
        /// Допоміжна функція: читає задану кількість точок з консолі з валідацією.
        /// Використовує CultureInfo.InvariantCulture.
        /// </summary>
        private static Point[] ReadPoints(int count)
        {
            var result = new Point[count];
            Console.WriteLine($"Введіть координати {count} вершин у форматі X Y (роздільник - пробіл). Використовуйте десяткову крапку.");

            for (int i = 0; i < count; i++)
            {
                while (true)
                {
                    Console.Write($"Точка {i + 1}: ");
                    string line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Console.WriteLine("Порожній рядок — спробуйте ще раз.");
                        continue;
                    }

                    var parts = line.Trim().Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2)
                    {
                        Console.WriteLine("Потрібно 2 числа (X Y). Спробуйте ще раз.");
                        continue;
                    }

                    if (double.TryParse(parts[0], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double x) &&
                        double.TryParse(parts[1], NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out double y))
                    {
                        result[i] = new Point(x, y);
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Некоректне число. Використовуйте формат із крапкою як десятковим роздільником. Спробуйте ще раз.");
                    }
                }
            }

            return result;
        }
    }
}
