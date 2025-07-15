
// This code snippet is part of a console application that connects to a Plant4D server.

// Declare the server object and specify the server type and version:
Plant4D.Server server = new Plant4D.Server(Plant4D.ServerType.Development, Plant4D.P4DVersion.Rome);

// Create a PCE (Plant-4D Central Environment) object using the server:
Plant4D.PCE pce = new Plant4D.PCE(server);

// Assign a project using the name of the project:
Plant4D.Project project = new Plant4D.Project(pce, "TestProject");

// Retrieve the project number and query a settings table:
var projno = project.GetProjectNumber();
var a = project.QuerySettingsTable("rootName", "sectionName", "keyName");


Console.WriteLine($"The project number is {projno}");
Console.WriteLine($"The settings table value is {a}");
Console.WriteLine($"The project year is {project.Year}");