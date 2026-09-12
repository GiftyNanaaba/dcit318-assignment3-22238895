DCIT 318 	6	6 PROGRAMMING II
ASSIGNMENT 3

Student Name: Enoch Opoku
Student ID: -22033645
Repository: dcit318-assignment3--22033645

This repository contains five separate C# console applications, each demonstrating specific C# concepts required by the assignment.

Projects

1. Question1FinanceManagement
   - Finance Management System demonstrating: records, interfaces, interface implementations, inheritance, virtual/override methods, sealed class, and List<T>.
   - Run: dotnet run --project Question1FinanceManagement/Question1FinanceManagement.csproj

2. Question2HealthcareSystem
   - Healthcare System demonstrating: generic repository, List<T>, Dictionary<TKey,TValue>, patient and prescription management.
   - Run: dotnet run --project Question2HealthcareSystem/Question2HealthcareSystem.csproj

3. Question3WarehouseInventory
   - Warehouse Inventory Management demonstrating: interface, generic repository, generic methods, Dictionary, custom exceptions, exception handling.
   - Run: dotnet run --project Question3WarehouseInventory/Question3WarehouseInventory.csproj

4. Question4StudentGrading
   - Student Grading File Processor demonstrating: file handling (StreamReader/StreamWriter), lists, custom exceptions, input validation. Includes students.txt input file.
   - Run: dotnet run --project Question4StudentGrading/Question4StudentGrading.csproj

5. Question5InventoryRecords
   - Inventory Records and File Storage demonstrating: C# records, immutable data, interfaces, generic class, JSON file persistence (System.Text.Json).
   - Run: dotnet run --project Question5InventoryRecords/Question5InventoryRecords.csproj

Build

Use the following commands to build each project individually:

dotnet build .\Question1FinanceManagement\Question1FinanceManagement.csproj
dotnet build .\Question2HealthcareSystem\Question2HealthcareSystem.csproj
dotnet build .\Question3WarehouseInventory\Question3WarehouseInventory.csproj
dotnet build .\Question4StudentGrading\Question4StudentGrading.csproj
dotnet build .\Question5InventoryRecords\Question5InventoryRecords.csproj

Testing

Run each project with dotnet run as listed above. Each application prints output to the console and demonstrates the required behaviors. The Question4 project writes report.txt to the project output folder. The Question5 project writes inventory.json to the project output folder.

Git

Commit each project separately as required by the assignment. Example commands for each question:

git add Question1FinanceManagement
git commit -m "Add Finance Management System"
git push origin main

git add Question2HealthcareSystem
git commit -m "Add Healthcare System"
git push origin main

git add Question3WarehouseInventory
git commit -m "Add Warehouse Inventory System"
git push origin main

git add Question4StudentGrading
git commit -m "Add Student Grading System"
git push origin main

git add Question5InventoryRecords
git commit -m "Add Inventory Records System"
git push origin main

To verify recent commits:

git log --oneline -5
git status
