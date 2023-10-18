using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConfigComparer.CompareLineResult;

namespace ConfigComparer
{
    public class ConsoleCompareWriter
    {
        public bool HideSame { get; set; } = false;
        public bool HideValues { get; set; } = false;

        public void Write(IEnumerable<CompareLineResult> compareLines)
        {
            foreach (var line in compareLines)
            {
                Write(line);
            }
        }

        public void WriteStatsToConsole()
        {
            Console.WriteLine("Statistics:");
            Console.WriteLine($" - Same items: {countSame}");
            Console.WriteLine($" - Different items: {countDiff}");
            Console.WriteLine($" - Left/right only: {countLeftOnly}/{countRightOnly}");
        }

        private int countSame, countDiff, countLeftOnly, countRightOnly;

        public void Write(CompareLineResult line)
        {
            IncreaseStats(line);
            WriteToConsole(line);
        }

        private void IncreaseStats(CompareLineResult line)
        {
            switch (line)
            {
                case Same:
                    countSame++;
                    break;
                case Different:
                    countDiff++;
                    break;
                case LeftOnly:
                    countLeftOnly++;
                    break;
                case RightOnly:
                    countRightOnly++;
                    break;
                default:
                    throw new InvalidOperationException("Unknown type.");
            }
        }

        private void WriteToConsole(CompareLineResult line)
        {
            var baseColor = Console.ForegroundColor;

            const ConsoleColor colorAdd = ConsoleColor.Green;
            const ConsoleColor colorRemove = ConsoleColor.Red;

            switch (line)
            {
                case Same when HideSame:
                    break;
                case Same s:
                    Console.Write($"    {s.Left.Key}");
                    if (!HideValues)
                    {
                        Console.Write($" = {s.Left.Value}");
                    }
                    Console.WriteLine();
                    break;
                case Different d:
                    Console.ForegroundColor = colorRemove;
                    Console.Write($"-   {d.Left.Key}");
                    if (!HideValues)
                    {
                        Console.Write($" = {d.Left.Value}");
                    }
                    Console.WriteLine();

                    Console.ForegroundColor = colorAdd;
                    Console.Write($"+   {d.Right.Key}");
                    if (!HideValues)
                    {
                        Console.Write($" = {d.Right.Value}");
                    }
                    Console.WriteLine();
                    break;
                case LeftOnly l:
                    Console.ForegroundColor = colorRemove;
                    Console.Write($"-   {l.Left.Key}");
                    if (!HideValues)
                    {
                        Console.Write($" = {l.Left.Value}");
                    }
                    Console.WriteLine();
                    break;
                case RightOnly r:
                    Console.ForegroundColor = colorAdd;
                    Console.Write($"+   {r.Right.Key}");
                    if (!HideValues)
                    {
                        Console.Write($" = {r.Right.Value}");
                    }
                    Console.WriteLine();
                    break;
                default:
                    throw new InvalidOperationException("Unknown type.");
            }

            Console.ForegroundColor = baseColor;
        }
    }
}
