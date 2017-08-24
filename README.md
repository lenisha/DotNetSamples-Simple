# WcfService on PCF

## WcfService-Simple
Hosting simple WCF REST service using HWC and Binary buildpacks

`/api/hello` - hello world

`/api/env` - print all environment variables

# WF applications on PCF
Windows Workflow Foundation has 3 models of hosting the flows
https://docs.microsoft.com/en-us/dotnet/framework/windows-workflow-foundation/using-workflowinvoker-and-workflowapplication,


## WFConsole-Simple
 WorkflowInvoker - console application, stateless, sync invocation - suitable for one off tasks
No special PCF updates required, it runs well, example hosting in binary buildpack

## WFService-Calc
WorkflowServiceHost - expose workflow as invocable WCF service
No special PCF updates required it runs well, example hosting in binary and hwc buildpack

## WF-LongApp
WorkflowApplication - stateful (persistent) host, running async - suitable for long running tasks
No special PCF updates required it runs well, example hosting in binary and hwc buildpack
Demo WorkflowApplication with idle, resume bookmarks


## WorkflowASPNet
Demo WorkflowApplication and WorkflowInvoker usage from ASP.NET
