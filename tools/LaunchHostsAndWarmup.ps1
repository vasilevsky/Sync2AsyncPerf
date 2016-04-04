Start-Process Output/Wcf.ConsoleHost.exe
Start-Process Output/Api.ConsoleHost.exe

Invoke-WebRequest http://localhost:8090/v1/customers -Method Post -ContentType application/json -Body "{`"name`":`"warming`", `"email`": `"warming@example.com`"}"
Invoke-WebRequest http://localhost:8090/v1/customersasync -Method Post -ContentType application/json -Body "{`"name`":`"warming`", `"email`": `"warming@example.com`"}"