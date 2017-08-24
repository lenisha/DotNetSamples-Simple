# WcfService on PCF

## WcfService-Simple
Hosting simple WCF REST service using HWC and Binary buildpacks

`/api/hello` - hello world

`/api/env` - print all environment variables

# WF applications on PCF
Windows Workflow Foundation has 3 models of hosting the flows
https://docs.microsoft.com/en-us/dotnet/framework/windows-workflow-foundation/using-workflowinvoker-and-workflowapplication,


## WFConsole-Simple
Demontrates  ** WorkflowInvoker ** - console application, stateless, sync invocation - suitable for one off tasks.
No special PCF updates required, it runs well, example hosting in binary buildpack.
When deployed starts as daemon and workflow invoked as a task:
```
cf run-task console-app-bin "bin/Debug/ConsoleApp-Task.exe dump" --name printenv
```

## WFService-Calc
Demonstrates ** WorkflowServiceHost ** - expose workflow as invocable WCF service.
No special PCF updates required it runs well, example hosting in binary and hwc buildpack.
To run use posteman collection in this repo or WcfTestClient.exe.

## WF-LongApp
Demonstrates ** WorkflowApplication **- stateful (persistent) host, running async - suitable for long running tasks.
No special PCF updates required it runs well, example hosting in binary and hwc buildpack.
This app shows WorkflowApplication with flow that could be idle and uses resume bookmarks.
To use 
```
 GET /start - will start workflow, print "what is your name?" and will suspend and wait for user activity
 GET /resume - will resume suspended workflow and pring "Hello Pivotal".
 ```

## WorkflowASPNet
Demonstrates **WorkflowApplication and WorkflowInvoker**  sync and async usage from ASP.NET
