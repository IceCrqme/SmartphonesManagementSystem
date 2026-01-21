# Smartphones Management System

This is a Windows Forms C# application for managing smartphones.

## Requirements
- Visual Studio 2022 or later
- SQL Server 2022 Express
- SQL Server Management Studio (SSMS)

## Setup
1. Restore the database from `SmartphonesDB.bak` using SSMS:
   - Right-click Databases → Restore Database → Device → Browse → Select `SmartphonesDB.bak`.
2. Open `SmartphonesApp.sln` in Visual Studio.
3. Make sure the connection string in `Form1.cs` points to your SQL Server instance:
   string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=SmartphonesDB;Integrated Security=True";
