using System;
using System.Collections.Generic;
using System.IO;

namespace Question4StudentGrading
{
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80) return "A";
            if (Score >= 70) return "B";
            if (Score >= 60) return "C";
            if (Score >= 50) return "D";
            return "F";
        }
    }

    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using var reader = new StreamReader(inputFilePath);
            string? line;
            int lineNumber = 0;
            while ((line = reader.ReadLine()) != null)
            {
                lineNumber++;
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');
                if (parts.Length != 3)
                {
                    throw new MissingFieldException($"Line {lineNumber}: Missing required fields.");
                }

                if (!int.TryParse(parts[0], out var id))
                {
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid student ID format.");
                }

                var name = parts[1].Trim();
                if (string.IsNullOrEmpty(name))
                    throw new MissingFieldException($"Line {lineNumber}: Missing student name.");

                if (!int.TryParse(parts[2], out var score))
                {
                    throw new InvalidScoreFormatException($"Line {lineNumber}: Invalid score format.");
                }

                students.Add(new Student(id, name, score));
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using var writer = new StreamWriter(outputFilePath);
            foreach (var s in students)
            {
                writer.WriteLine($"{s.FullName} (ID: {s.Id}): Score = {s.Score}, Grade = {s.GetGrade()}");
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var inputPath = Path.Combine(AppContext.BaseDirectory, "students.txt");
            var outputPath = Path.Combine(AppContext.BaseDirectory, "report.txt");

            var processor = new StudentResultProcessor();

            try
            {
                var students = processor.ReadStudentsFromFile(inputPath);
                processor.WriteReportToFile(students, outputPath);
                Console.WriteLine($"Report written to: {outputPath}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Invalid score format: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Missing field: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
