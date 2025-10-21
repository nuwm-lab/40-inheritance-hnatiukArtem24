using System;

namespace Variant10
{
    // Батьківський клас "Дріб" виду 1/(a*x)
    class Drib
    {
        protected double a;

        // Конструктор
        public Drib(double a)
        {
            this.a = a;
            Console.WriteLine("Створено об’єкт класу Drib");
        }

        // Метод для задання коефіцієнта
        public virtual void SetCoefficient(double a)
        {
            this.a = a;
        }

        // Метод для виведення коефіцієнта
        public virtual void ShowCoefficient()
        {
            Console.WriteLine($"a = {a}");
        }

        // Метод для обчислення значення дробу у точці x
        public virtual double Calculate(double x)
        {
            return 1 / (a * x);
        }

        // Деструктор
        ~Drib()
        {
            Console.WriteLine("Знищено об’єкт класу Drib");
        }
    }

    // Похідний клас "Тривимірний дріб"
    class ThreeDimDrib : Drib
    {
        protected double a2, a3;

        // Конструктор
        public ThreeDimDrib(double a1, double a2, double a3) : base(a1)
        {
            this.a2 = a2;
            this.a3 = a3;
            Console.WriteLine("Створено об’єкт класу ThreeDimDrib");
        }

        // Перевизначення методу для задання коефіцієнтів
        public override void SetCoefficient(double a)
        {
            Console.Write("Введіть a2: ");
            a2 = double.Parse(Console.ReadLine());
            Console.Write("Введіть a3: ");
            a3 = double.Parse(Console.ReadLine());
            this.a = a;
        }

        // Перевизначення методу для виведення коефіцієнтів
        public override void ShowCoefficient()
        {
            Console.WriteLine($"a1 = {a}, a2 = {a2}, a3 = {a3}");
        }

        // Перевизначення обчислення дробу
        public override double Calculate(double x)
        {
            return 1 / (a * x + 1 / (a2 * x + 1 / (a3 * x)));
        }

        // Деструктор
        ~ThreeDimDrib()
        {
            Console.WriteLine("Знищено об’єкт класу ThreeDimDrib");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Клас Drib ===");
            Drib d1 = new Drib(2);
            d1.ShowCoefficient();
            Console.Write("Введіть x: ");
            double x1 = double.Parse(Console.ReadLine());
            Console.WriteLine($"Значення дробу: {d1.Calculate(x1)}");

            Console.WriteLine("\n=== Клас ThreeDimDrib ===");
            ThreeDimDrib d2 = new ThreeDimDrib(1, 2, 3);
            d2.ShowCoefficient();
            Console.Write("Введіть x: ");
            double x2 = double.Parse(Console.ReadLine());
            Console.WriteLine($"Значення тривимірного дробу: {d2.Calculate(x2)}");

            // Демонстрація роботи Garbage Collector
            d1 = null;
            d2 = null;
            Console.WriteLine("\nЗапускаємо збірку сміття...");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Роботу завершено.");
        }
    }
}
