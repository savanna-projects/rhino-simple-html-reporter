using Gravity.Abstraction.Logging;
using Gravity.Extensions;

using Rhino.Api.Contracts.Attributes;
using Rhino.Api.Contracts.AutomationProvider;
using Rhino.Api.Contracts.Configuration;
using Rhino.Api.Reporter.SimpleHtml.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rhino.Api.Reporter.SimpleHtml
{
    [Reporter("reporter_simple_html", Description = "Rhino plugin for creating a flat, single file HTML report using HTML and CSS only.")]
    public class SimpleHtmlReporter : RhinoReporter
    {
        // members: fields
        private static readonly string htmlReportTemplate = Assembly.GetExecutingAssembly().ReadEmbeddedResource("ReportTemplate.html");
        private static readonly string htmlTestCaseTemplate = Assembly.GetExecutingAssembly().ReadEmbeddedResource("TestCaseTemplate.html");
        private static readonly string htmlTestsSummaryTemplate = Assembly.GetExecutingAssembly().ReadEmbeddedResource("TestsSummaryTemplate.html");
        private const string TestsSummaryHtmlTableRowTemplate =
            "<tr>" +
            "   <td class=\"index_cell\">${index}</td>" +
            "   <td><a href=\"#${test_id}\">${test_id}</a></td>" +
            "   <td>${test_name}</td>" +
            "   <td class=\"status_cell ${test_status_class}\">${test_status}</td>" +
            "   <td class=\"duration_cell\">${run_duration}</td>" +
            "</tr>\n";

        public SimpleHtmlReporter(RhinoConfiguration configuration)
            : base(configuration)
        { }

        public SimpleHtmlReporter(RhinoConfiguration configuration, ILogger logger)
            : base(configuration, logger)
        { }

        public override void OnGenerate(RhinoTestRun testRun)
        {
            // setup
            var testsSummary = new List<IDictionary<string, string>>();
            var testCasesHtmlBuilder = new StringBuilder();
            var reportOut = $"{Configuration.ReportConfiguration.ReportOut}-{testRun.Key}";

            // iterate
            foreach (var testCase in testRun.TestCases)
            {
                testCasesHtmlBuilder
                    .Append(testCase.ToHtml(htmlTestCaseTemplate, testsSummary))
                    .Append("\n\n");
            }

            // build HTML
            var testsSummaryHtml = GenerateTestsSummaryHtmlTable(testsSummary);
            var html = htmlReportTemplate
                .Replace("${rhino_test_cases}", testCasesHtmlBuilder.ToString())
                .Replace("${rhino_tests_summary}", testsSummaryHtml);
            Logger?.Info("Create-HtmlReportTemplate = OK");

            // write to file
            Directory.CreateDirectory(reportOut);
            var path = Path.Combine(reportOut, "SimpleReport.html");
            File.WriteAllText(path, html);
        }

        private static string GenerateTestsSummaryHtmlTable(IList<IDictionary<string, string>> testsSummary)
        {
            var testsSummayHtmlTableRowsBuilder = new StringBuilder();

            for (int i = 0; i < testsSummary.Count; i++)
            {
                string summaryTableRow = TestsSummaryHtmlTableRowTemplate
                    .Replace("${index}", i + 1 + "")
                    .Replace("${test_id}", testsSummary[i]["testId"])
                    .Replace("${test_name}", testsSummary[i]["testName"])
                    .Replace("${test_status_class}", testsSummary[i]["testStatus"].ToLower())
                    .Replace("${test_status}", testsSummary[i]["testStatus"])
                    .Replace("${run_duration}", testsSummary[i]["runDuration"]);

                testsSummayHtmlTableRowsBuilder
                    .Append(summaryTableRow)
                    .Append('\n');
            }

            int totalTests = testsSummary.Count;
            int passCount = testsSummary.Count(testsSummary => testsSummary["testStatus"] == "Pass");
            int failCount = totalTests - passCount;

            return htmlTestsSummaryTemplate
                .Replace("${total_tests}", totalTests + "")
                .Replace("${pass_count}", passCount + "")
                .Replace("${fail_count}", failCount + "")
                .Replace("${tests_summary_rows}", testsSummayHtmlTableRowsBuilder.ToString());
        }
    }
}
