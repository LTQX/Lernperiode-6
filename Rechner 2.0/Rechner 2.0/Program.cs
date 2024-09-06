using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

class Program
{
    static void Main(string[] args)
    {
        // Main loop to allow user to calculate multiple averages without restarting the program
        while (true)
        {
            // Dictionary to store subjects and their corresponding grades
            var subjects = new Dictionary<string, List<double>>
            {
                {"Swir", new List<double>()},
                {"Swfr", new List<double>()},
                {"Englisch", new List<double>()},
                {"Deutsch", new List<double>()},
                {"Mathematik", new List<double>()},
                {"Französisch", new List<double>()},
                {"Sport", new List<double>()},
                {"Ila", new List<double>()}
            };

            double penaltyPoints = 0; // Variable to track penalty points

            // Loop through each subject to collect grades
            foreach (var subject in subjects.Keys)
            {
                while (true)
                {
                    // Get the grade from the user
                    double grade = GetGrade(subject);
                    subjects[subject].Add(grade);

                    // Update penalty points based on the grade
                    penaltyPoints += GetPenaltyPoints(grade);

                    // Ask if the user wants to add another grade for the current subject
                    Console.WriteLine("Add another grade for {0}? (y/n)", subject);
                    string input = Console.ReadLine().ToLower();
                    if (input != "y")
                    {
                        break; // Exit loop if user doesn't want to add another grade
                    }
                }
            }

            // Calculate the total and average grades
            double total = 0;
            int totalGradesCount = 0;
            foreach (var subject in subjects)
            {
                double subjectAverage = CalculateSubjectAverage(subject.Value);
                total += (subject.Key == "Ila") ? subjectAverage * 2 : subjectAverage;
                totalGradesCount += (subject.Key == "Ila") ? 2 : 1;
            }

            // Calculate and round the overall average grade
            double totalGrades = total / totalGradesCount;
            double roundedTotalGrades = RoundGrade(totalGrades);

            // Generate the result text
            string resultText = "Grades:\n";
            foreach (var subject in subjects)
            {
                double subjectAverage = CalculateSubjectAverage(subject.Value);
                resultText += $"{subject.Key}: {RoundGrade(subjectAverage):0.00}\n";
            }
            resultText += $"\nAverage Grade: {roundedTotalGrades:0.00}";
            resultText += $"\nPenalty Points: {penaltyPoints:0.00}";

            Console.WriteLine(resultText);

            // Check if the user is provisionally due to penalty points
            if (penaltyPoints <= -3)
            {
                Console.WriteLine("Du bist provisorisch");
            }

            // Ask if the user wants to save as PDF
            Console.WriteLine("This is your whole average: {0:0.00}", roundedTotalGrades);
            Console.WriteLine("Save as PDF? (y/n)");
            string saveAsPdf = Console.ReadLine().ToLower();

            if (saveAsPdf == "y")
            {
                // Save to .txt file
                string txtFilePath = "GradesReport.txt";
                File.WriteAllText(txtFilePath, resultText);
                Console.WriteLine($"Grades and averages saved to {txtFilePath}");

                // Convert to PDF and save to desktop
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string pdfFilePath = Path.Combine(desktopPath, "GradesReport.pdf");
                ConvertTxtToPdf(txtFilePath, pdfFilePath);
                Console.WriteLine($"PDF generated at {pdfFilePath}");
            }

            // Ask if the user wants to start over or exit
            Console.WriteLine("Do you want to calculate another average? (y/n)");
            string startOver = Console.ReadLine().ToLower();
            if (startOver != "y")
            {
                break; // Exit program if the user chooses not to calculate another average
            }
        }
    }

    // Function to get a valid grade from the user
    static double GetGrade(string subject)
    {
        double grade;
        Console.Write($"Enter your grade for {subject}: ");
        while (!double.TryParse(Console.ReadLine(), out grade) || grade < 1 || grade > 6)
        {
            Console.WriteLine("Invalid input. Please enter a grade between 1 and 6.");
        }
        return grade;
    }

    // Function to calculate the average grade for a subject
    static double CalculateSubjectAverage(List<double> grades)
    {
        double sum = 0;
        foreach (var grade in grades)
        {
            sum += grade;
        }
        return sum / grades.Count;
    }

    // Function to round grades to the nearest 0.25
    static double RoundGrade(double grade)
    {
        return Math.Round(grade * 4, MidpointRounding.AwayFromZero) / 4.0;
    }

    // Function to determine penalty points based on the grade
    static double GetPenaltyPoints(double grade)
    {
        if (grade == 3.5) return -0.5;
        if (grade == 3) return -1;
        if (grade == 2.5) return -1.5;
        if (grade == 2) return -2;
        if (grade == 1.5) return -2.5;
        if (grade == 1) return -3;
        return 0; // No penalty for grades above 3.5
    }

    // Function to convert a text file to a PDF
    static void ConvertTxtToPdf(string txtFilePath, string pdfFilePath)
    {
        Document doc = new Document();
        PdfWriter.GetInstance(doc, new FileStream(pdfFilePath, FileMode.Create));
        doc.Open();
        string text = File.ReadAllText(txtFilePath);
        doc.Add(new Paragraph(text));
        doc.Close();
    }
}