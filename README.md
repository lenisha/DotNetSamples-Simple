# WcfService on PCF

## WcfService-Simple
Hosting simple WCF REST service using HWC and Binary buildpacks

`/api/hello` - hello world

`/api/env` - print all environment variables

## WcfService-Tcp 
Self hosted WCF service over tcp, project includes service and client
_Note:_ it's example to test how to run in PCF, for production better management cycle should be written. 
Example is based on sample:
https://docs.microsoft.com/en-us/dotnet/framework/wcf/samples/default-nettcpbinding

### Running TCP example on PCF
* Enable TCP Routing on ERT
* Create shared domain and quota as per instructions:
https://docs.pivotal.io/pivotalcf/1-11/adminguide/enabling-tcp-routing.html
```
cf create-shared-domain <space> tcp.<apps-domain>  --router-group default-tcp
cf update-quota default —reserved-route-ports 100
```
* Push application using supplied manifest (binary buildpack, port healthcheck and tcp route)
```
cf push -f manifest_bin.yml
```
* To create route manually (although if supplied in manifest it will be created during push)
```
cf create-route pcfdev-space tcp.local.pcfdev.io --port 9473
cf map-route gs-spring tcp.local.pcfdev.io --port 9473
```
### Run tcp client to test
Point client to the service route in app.config
```
<endpoint name="" address="net.tcp://tcp.cfapps.haas-51.pez.pivotal.io:9743/servicemodelsamples/service" />

```

# WF applications on PCF
Windows Workflow Foundation has 3 models of hosting the flows
https://docs.microsoft.com/en-us/dotnet/framework/windows-workflow-foundation/using-workflowinvoker-and-workflowapplication,


## WFConsole-Simple
Demontrates  **WorkflowInvoker** - console application, stateless, sync invocation - suitable for one off tasks.
No special PCF updates required, it runs well, example hosting in binary buildpack.
When deployed starts as daemon and workflow invoked as a task:
```
cf run-task console-app-bin "bin/Debug/ConsoleApp-Task.exe dump" --name printenv
```

## WFService-Calc
Demonstrates **WorkflowServiceHost** - expose workflow as invocable WCF service.
No special PCF updates required it runs well, example hosting in binary and hwc buildpack.
To run use posteman collection in this repo or WcfTestClient.exe.

## WF-LongApp
Demonstrates **WorkflowApplication** - stateful (persistent) host, running async - suitable for long running tasks.
No special PCF updates required it runs well, example hosting in binary and hwc buildpack.
This app shows WorkflowApplication with flow that could be idle and uses resume bookmarks.
To use 
```
 GET /start - will start workflow, print "what is your name?" and will suspend and wait for user activity
 GET /resume - will resume suspended workflow and pring "Hello Pivotal".
 ```

## WorkflowASPNet
Demonstrates **WorkflowApplication and WorkflowInvoker**  sync and async usage from ASP.NET
