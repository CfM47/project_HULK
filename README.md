# Project_H.U.L.K.
Second project of Programming course of 1st year of Computer Science at Faculty of Math and Computers, University of Havana.

## Context

For project orientation, go to [this link](https://github.com/matcom/programming/blob/main/projects/hulk). It defines the subset of the language being implemented. For the complete language documentation (the full language, not the subset), go to [this link](https://github.com/matcom/hulk).

## Requirements
- .Net Core 6.0
- MAUI for building AppInterface project

## Installation

1. Clone the repository:
  ```bash
  git clone https://github.com/matcom/hulk.git
  cd hulk
  ```

2. Restore the NuGet packages:
  ```bash
  dotnet restore
  ```

## How to Run the Project

This solution consists of four projects:

### - Hulk

A class library that contains the compiler logic.

### Interface

A .NET Console Application. This is the project to run if you are not on Windows.

To run the console application:
```bash
cd Interface
dotnet run
```

### FormInterface

A .NET Windows Form Application, which performs the same functionality as Interface but in a slightly more user-friendly way. It is recommended to start this project if you are using Windows.

To run the Windows Form application:
```bash
cd FormInterface
dotnet run
```

### AppInterface

A .NET MAUI cross-platform application.

To run the MAUI application:
```bash
cd AppInterface
dotnet build
dotnet run
```
