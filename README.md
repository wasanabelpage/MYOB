## Introduction
This is a console application that can be extended to a total Human Resource Management System.
After running the console app - it allows to enter the functionality name with required input params to output the desired outcome.

## Features
* Contains Unit testing
* Contains an appsetting file to store configuration settings
* Decouple dependancies with .NET Core Dependancy Injection
* Can unit test with mock objects of the interfaces provided
* Accounting  calculation abstracted and encapsulated behind the interface - use of OOP concepts
* Easily extendable : To a web project with a UI or can add a Data Access layer with Entity Framework Core due to the architecture of the solution


## Future Enhancements Proposed
* Use of database to store Tax Bands
* Use logger functionality to generate logs
* Domain Modelling - use seperate DTO's for returning result instead of [PayrollResult.cs] shared DTO
* Domain Modelling - Pass a Message field with more info when [PayrollResultStatusCode] is failed - (useful to display to a user interface)
* Upgrate to handle and validate inputs arguments/parameters with a regex
* Add a constraint to check if employee exists
* Styling of console output to a table for better visual representation
* Use of environment variables and relavent appsettings*.json file

## Assumptions
### Business Requirement Assumptions
1. The first input parameter for the console application is always the function name to be used
1. The payslip generation function will always have 3 parts in the input
	1. function name - this will be a const - GenerateMonthlyPayslip
	1. employee name - this will be provided in double quotes - not white space or empty
	1. annual salary - this will be provided as numeric value - non negative

## Technologies
* .NET Core 2.2
* C# 7.3
* Visual Studio 2017

## Solution Architecture
![Solution Diagram] (/Diagram.jpg "Solution Diagram.")

## SOLID Design Principles

### Single Responsibility Principle - SRP
	MYOB.HumanResource.Core project consists of independant methods which has one and only one responsibility

### Open/Closed Principle - OCP
	The arrange of Program.cs class to allow extended funtions
	Use of Helper classes and interfaces that are open to extend for new functions to come
	MYOB.HumanResource.Core project consists of independant methods which are closed for modifications

### Dependancy Inversion Principle - DIP
    Use of interfaces and dependancy injection from .NET Core and interfaces are providing abstractions	
	This can be used to mock interfaces for unit testing.

## How to run the program

Option 1
```
Go to MYOB.HumanResource Project Properties
Go to Debug
Go to Application Arguements text box and enter <function name> "<employee name>" <annual salary>
>> ex: GenerateMonthlyPayslip "Mary Song" 60000

```

Option 2
```
Set the MYOB.HumanResource as the start up project
Run the application by pressing F5
Once Command Promt allows for the user input enter <function name> "<employee name>" <annual salary> 
>> ex: GenerateMonthlyPayslip "Mary Song" 60000
```

## How to execute Unit Tests
* Open TestExplorer for the Visual Studio Windows
* Build the solution
* Run All Tests










