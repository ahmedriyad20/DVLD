🚗 Driving & Vehicle License Department (DVLD) System
Desktop Application | 3-Tier Architecture | C# WinForms
📌 Project Overview

The Driving & Vehicle License Department (DVLD) System is a comprehensive desktop application designed to manage all operations related to driver licensing and vehicle services.
It automates processes such as:

Issuing new driver licenses

Renewing existing licenses

Replacing lost/damaged licenses

Suspending & restoring licenses

Managing applicants, tests, and user accounts

The application is built using 3-Tier Architecture (Presentation → Business Logic → Data Access), following clean Object-Oriented Programming principles to ensure scalability, structure, maintainability, and strong separation of concerns.

🛠️ Technologies & Tools
Programming & Framework

C# – .NET Framework

Windows Forms (WinForms)

Database & Backend

Microsoft SQL Server

ADO.NET for database connectivity

Stored Procedures (optional depending on your implementation)

App.config for secure connection string handling

Architecture

3-Tier Architecture

Presentation Layer (WinForms)

Business Logic Layer (BLL)

Data Access Layer (DAL)

Programming Concepts

OOP: Classes, Inheritance, Polymorphism, Encapsulation, Abstraction

Async/Await for responsive UI

Multithreading via Task.Run (e.g., People List Form operations)

Exception handling + event logging

Separation of concerns for clean and maintainable code

🔐 Security Enhancements (Custom Features Added)

This project includes multiple security-focused upgrades:

✔ Password Security

SHA-256 Hashing + Salt

Custom Salt column added to Users table

Secure password verification logic in BLL

✔ Credential Storage

AES Symmetric Encryption for storing saved login credentials

Uses Windows Registry instead of plain text storage

“Remember Me” implemented securely

✔ Application Logging

Windows Event Log integration for:

Error tracking

Unauthorized access attempts

System diagnostics

✔ Configuration Security

Connection string stored and managed in App.config, not hardcoded

⚙️ Key Features

User management (create, edit, delete, activate/deactivate)

Applicant and driver management

License issuance workflow

License renewal & replacement forms

License suspension & tracking

Transaction history

Filtering, searching, pagination (People List)

Modern and responsive UI using Guna UI 2

Background data loading to avoid UI freezing

Full error and activity logging
