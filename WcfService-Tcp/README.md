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
