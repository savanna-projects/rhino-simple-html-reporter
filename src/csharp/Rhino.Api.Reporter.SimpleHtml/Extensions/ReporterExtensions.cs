using Rhino.Api.Contracts.AutomationProvider;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rhino.Api.Reporter.SimpleHtml.Extensions
{
    internal static class ReporterExtensions
    {
        #region *** To HTML ***
        private const string TestCaseHtmlTableRowTemplate =
            "<tr>" +
            "   <td class=\"index_cell\">${step_number}</td>" +
            "   <td class=\"step_status ${status_class}\">${step_status}</td>" +
            "   <td class=\"step_action\">${step_action}</td>" +
            "   <td class=\"step_expected_result\">${step_expected_result}</td>" +
            "   <td class=\"duration_cell\">${step_duration}</td>" +
            "</tr>\n";

        public static string ToHtml(this RhinoTestCase testCase, string htmlTemplate, IList<IDictionary<string, string>> testsSummary)
        {
            var testName = testCase.Scenario;
            var testId = testCase.Key;
            var startTime = testCase.Start.ToString();
            var endTime = testCase.End.ToString();
            var runDuration = testCase.RunTime.ToString(@"mm\:ss\.fff");
            var testStatus = testCase.Steps.Any(i => !i.Actual) ? "Fail" : "Pass";

            var testSummary = new Dictionary<string, string>();
            testSummary["testId"] = testId;
            testSummary["testName"] = testName;
            testSummary["testStatus"] = testStatus;
            testSummary["runDuration"] = runDuration;
            testsSummary.Add(testSummary);

            htmlTemplate = htmlTemplate
                .Replace("${test_id}", testId)
                .Replace("${test_name}", testName)
                .Replace("${status_highlight_class}", testStatus.ToLower() + "_highlight")
                .Replace("${test_status}", testStatus)
                .Replace("${test_duration}", runDuration)
                .Replace("${test_start_time}", startTime)
                .Replace("${test_end_time}", endTime);

            var testSteps = testCase.Steps.ToList();
            var htmlTableRowsBuilder = new StringBuilder();

            for (int i = 0; i < testSteps.Count; i++)
            {
                var tr = ToHtmlTableRow(testSteps[i], i + 1);
                htmlTableRowsBuilder.Append(tr);
            }

            return htmlTemplate.Replace("${rhino_test_case_step_rows}", htmlTableRowsBuilder.ToString());
        }

        private static string ToHtmlTableRow(RhinoTestStep step, int stepNumber)
        {
            var status = step.Actual ? "V" : "X";
            var statusClass = step.Actual ? "pass" : "fail";

            return TestCaseHtmlTableRowTemplate
                .Replace("${step_number}", stepNumber + "")
                .Replace("${status_class}", statusClass)
                .Replace("${step_status}", status)
                .Replace("${step_action}", step.Action)
                .Replace("${step_expected_result}", step.Expected)
                .Replace("${step_duration}", step.RunTime.ToString(@"mm\:ss\.fff"));
        }
        #endregion
    }
}
