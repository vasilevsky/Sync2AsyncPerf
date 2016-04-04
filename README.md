# Sync2AsyncPerf
Project aims to compare performance of async/sync approaches over close to real use cases, identify pitfalls and proper setup.

## About application ##

Application implements following case in terms of Customer-Order domain:

* Store Customer data into xml file

* Create Customer passing via uniqueness rules

* Add Order to Customer


Application exposes REST API for further benchmark.

* API implemented with OWIN hosted ASP.NET Web API.

* Database persistence/reads implemented through Entity Framework 6.

* External service call simulated through call to separate WCF service.


There are 2 host applications:

* Api.ConsoleHost hosting API on **localhost:8090**

* WCF.ConsoleHost hosting WCF service on **localhost:8089**

##Configuration##

Specify proper database connection strings in **app.config** files of **Api.ConsoleHost** and **Wcf.ConsoleHost**. Database would be created automatically when the first call would be made.

##How to run##

Use **LaunchHostsAndWarmup.ps1** script in order to start host applications and make warming up calls. You may need to bypass execution policy in order to run the script. **WCF.ConsoleHost** requires elevated rights to start.

You may use **ab.exe** (ApacheBench) to run your measurements directly or through **ab_sample.cmd**.

Endpoints for tests are following: 

Sync  - **POST http://localhost:8090/v1/customers** + customer data payload

Async - **POST http://localhost:8090/v1/customersasync** + customer data payload

##Benchmarks##
||Number of concurent requests/All requests|Requests per second|Setup|Remark|
|------|-------------------------|-------------------|------|-----|
|async|500/20000|119.27|single, i5-2500 @ 3.30 GHz 4 Cores, 16GB, Win10 64, LocalDb|Fails when exceed number of `Max Pool Size`|
|sync|500/20000|154.97|||
|async|500/10000|130.18|caller - network, Xeon E5-2609 2 Cores, 4GB, local MSSQL2014|~~4001 failed requests~~ __verify__|
|sync|500/10000|144.59||CPU ~100%|
|async|500/20000|Empty DB - 190.78, 60000 - 143.41, 100000 - 84.51|api - Xeon E5-2609 2 Cores, 4GB; sql - network, MSSQL2014, Xeon E5-2609 2 Cores, 4GB| SQL 100%|
|sync|500/20000|Empty BD - 220.31, 60000 - 110.52, 100000 - 88.94||SQL 100%|
