Originally developed by rony.byalsky  

# Simple HTML Reporter
Rhino plugin for creating a flat, single file HTML report using HTML and CSS only.

## Prerequisits
1. [.NET 5.0 SDK installed](https://dotnet.microsoft.com/download/dotnet/5.0).
2. [Rhino Agent deployed](https://github.com/savanna-projects/rhino-agent/blob/master/docs/pages/Home.md).

## How to Deploy?
1. Clone the code.
2. Build the solution.
3. From the build output folder copy the file `Rhino.Api.Reporter.SimpleHtml.dll` into the folder where you deployed Rhino Agent.
4. Run Rhino Agent

## How to Verify?
While Rhino Agent is running, send the following GET command using any client (you can also use the browser):
```
<rhino-base-address>:<rhino-port>/api/v3/widget/reporters
```  

The response will be a list of all available reporters on the server:

```js
[
  {
    "description": "The default Rhino HTML Reporter. A rich HTML Report with all test results and quality matrix.",
    "name": "reporter_basic",
    "typeId": "Rhino.Api.Contracts.Attributes.ReporterAttribute, Rhino.Api.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
  },
  {
    "description": "Rhino plugin for creating a flat, single file HTML report using HTML and CSS only.",
    "name": "reporter_simple_html",
    "typeId": "Rhino.Api.Contracts.Attributes.ReporterAttribute, Rhino.Api.Contracts, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
  }
]

```

## How to Use?
Under the [`reportConfiguration` token in your configuration](https://github.com/savanna-projects/rhino-agent/blob/master/docs/pages/ApiReference/Configurations.md#report-configuration), add `reporter_simple_html` (the reporter name) to the reporters list:
```js
...
"reportingConfiguration": {
    ...
    "reporters": [
        "reporter_basic",
        "reporter_simple_html"
    ]
    ...
}
...
```
