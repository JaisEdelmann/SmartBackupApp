# SmartBackupApp
Simple and smart backupapp application, that can be scheduled via Windows task scheduler


## Settings
Remember to update the settings in app.config, to match you required scenario. Version's indicates how many version you want the backup solution to contain. This means that you will have '5' versions of your source onces it creates the 6th it will remove the first oldeste version.

```xml
  <appSettings>
    <add key="SleepOnCompletion"  value="false"/> 
    <add key="ZipResult"          value="true"/>
    <add key="Versions"           value="5"/>
    <add key="Source"             value="D:\Data\Backup\Source" />
    <add key="Target"             value="D:\Data\Backup\Target" />
  </appSettings>
```
  
