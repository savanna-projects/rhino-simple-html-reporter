using Gravity.Services.DataContracts;

using Rhino.Api.Contracts.Configuration;
using Rhino.Api.Extensions;

using System;
using System.Collections.Generic;
using System.IO;

namespace ReportClient
{
    internal static class Program
    {
        private static void Main()
        {
            var configuration = new RhinoConfiguration
            {
                Name = "Test Configuration",
                TestsRepository = new[]
                {
                    File.ReadAllText("001_google_positive_test.txt"),
                    File.ReadAllText("002_google_negative_test.txt"),
                },
                DriverParameters = new[]
                {
                    new Dictionary<string, object>
                    {
                        ["driver"] = "ChromeDriver",
                        ["driverBinaries"] = "."
                    }
                },
                Authentication = new Authentication
                {
                    UserName = "",
                    Password = ""
                },
                ConnectorConfiguration = new RhinoConnectorConfiguration
                {
                    Connector = "connector_text"
                },
                EngineConfiguration = new RhinoEngineConfiguration
                {
                    MaxParallel = 5,
                    RetrunExceptions = true,
                    ReturnPerformancePoints = true,
                    ReturnEnvironment = true
                },
                ScreenshotsConfiguration = new RhinoScreenshotsConfiguration
                {
                    ReturnScreenshots = true,
                    ScreenshotsOut = Path.Combine(Environment.CurrentDirectory, "Reports", "Images"),
                    KeepOriginal = true
                },
                ReportConfiguration = new RhinoReportConfiguration
                {
                    ReportOut = Path.Combine(Environment.CurrentDirectory, "Reports", "rhino"),
                    Reporters = new[] { "reporter_basic", "reporter_simple_html" },
                    LogsOut = Path.Combine(Environment.CurrentDirectory, "Reports", "Logs"),
                    AddGravityData = true
                }
            };
            configuration.Execute(Utilities.Types);
        }
    }
}