Client by UntappdWindowsService
To change the URL of connection to the service
Add to Startup project in App.config:

  <appSettings>
    <add key="UntappdWCFServiceUrlBase" value="" />
  </appSettings>
 
 Or this root project add file UntappdWindowsService.Extension.dll.config (Copy Always), example link content include in package UntappdWindowsService.Extension.dll.Example.config:

 <?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="UntappdWCFServiceUrlBase" value="" />
  </appSettings>
</configuration>

