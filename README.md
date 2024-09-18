# Introduction 
This project is the QA take home exercise for Fetch

# Getting Started
Repository Overview:
Both the command-line utility and the test project are located in the same repository.



# Build and Test
Build the solution in Visual Studio 2022, alternatively MSBuild took can be downloaded here https://visualstudio.microsoft.com/downloads/



# Instructions for Command Line Utility and Test Project
# Repository Overview:
Both the command-line utility and the test project are located in the same repository.

# Command Line Utility
- Utility Name: geocodeUtility.exe
- Framework: Written in C#, .NET Framework 4.7.2.

- Parameters:

    The utility accepts inputs in the following formats, where the zip codes and location names can appear in any order:
      - "Charlotte, NC"
      - "Charlotte, NC", "Tampa, FL"
      - "28277"
      - "Charlotte, NC" "91011" "Tampa, FL"
- Note: The API key is hardcoded within the program to minimize complexity.

- Example Usage:
      geocodeUtility "Charlotte, NC"

# Test Project
- Project Name: GeoCodeUtilityTestsProject

- Framework: Written in C#, .NET Framework 4.7.2.

- Test Data:
  The test project accepts test data from an Excel file located in the same folder as the DLL.

  Test Data File Name: GeoCodeTestData.xlsx (file name is hardcoded).
  Number of Test Cases: 13.

# How to Execute the Tests:

You will need vstest.console.exe to run the test cases.
This file is typically located in the following folder:
C:\Program Files (x86)\Microsoft Visual Studio\<version>\TestAgent\Common7\IDE\CommonExtensions\Microsoft\TestWindow
Alternatively, refer to this blog for more details on obtaining vstest.console.exe:
 https://visualstudio.microsoft.com/downloads/
 ![image](https://github.com/user-attachments/assets/4d85d92c-3985-42ff-9a19-fdec471de56e)

 - Example Usage to Run Tests:
   "C:\Program Files (x86)\Microsoft Visual Studio\2022\TestAgent\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console" "C:\1MyFolder\TEMP\WeatherGeoCodeForFetch\WeatherGeoCodeForFetch\GeoCodeUtilityTestsProject\bin\Debug\net472\GeoCodeUtilityTestsProject.dll"

# Test Data Template
A test data template (TestDataTemplate.xlsx) is provided for reference.
 - Note: The actual code expects the test data file to be named GeoCodeTestData.xlsx (hardcoded for simplicity).
          The test file reads data from the Excel sheet and writes the outcome or output from the API call back to the same file

##Important Notes:
There are known, intentionally unfixed issues in the geocodeUtility.exe to demonstrate failed test cases. For example, the code does not handle invalid location names as expected.

