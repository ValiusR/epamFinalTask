# E-Commerce UI Test Automation

UI test automation framework built for QA Brains Practice E-Commerce site (https://practice.qabrains.com/ecommerce). 

The project is written in C# (.NET 8) using Selenium WebDriver and xUnit. Runs with Chrome and Edge.


## Structure

* `/TestLayer/Tests/` - xUnit test classes
* `/TestLayer/Data/` - test data providers
* `/BusinessLayer/PageObjects/` - Page Object classes
* `/CoreLayer/Driver/` - WebDriver management, factory, and browser setups
* `xunit.runner.json` - xUnit settings

## To run locally

1. Install .NET 8 SDK, new versions of Chrome and Edge
2. Clone repo and restore the NuGet packages
3. Run  tests with Visual Studio 2022 Test Explorer

## Original task:
Task description

Site for testing: https://practice.qabrains.com/ecommerce

UC-1 Test Login form with wrong credentials:

oEnter any credentials that are not listed in “Accepted email” and “Password” sections into "Email" and "Password" fields.

oHit the "Login" button.

oCheck the error messages: "Username is incorrect" and “Password is incorrect.”.

UC-2 Test favorite products:

oEnter any credentials from “Accepted email” and “Password” sections into "Email" and "Password" fields.

oMark a few items as favorites.

oClick on the email in the top right corner and click “Favorites”.

oCheck that previously selected items are displayed on Favorites page.

UC-3 Test products ordering:

oEnter any credentials from “Accepted email” and “Password” sections into "Email" and "Password" fields.

oOrder products by price from low to high.

oCheck that all products are ordered according to selected option.

Provide possibility to execute tests in parallel, add logging to track execution flow and use data-driven testing approach.

Make sure that all tasks are supported by these 3 conditions: UC-1; UC-2; UC-3.

Please, add task description as README.md into your solution!

To perform the task use the various of additional options:

Test Automation tool: Selenium WebDriver;

Browsers: 1) Chrome; 2) Edge;

Locators: XPath;

Test Runner: xUnit;

Assertions: FluentAssertions;

[Optional] Patterns: 1) Builder; 2) Adapter; 3) Bridge;

[Optional] Test automation approach: BDD;

[Optional] Loggers: NLog.
