# SmartBackupApp
Simple and smart backupapp application, that can be scheduled via Windows task scheduler.

I created this little app, because I really coulden't find any simple and free solutions for my Virtual machines to get automaticly pushed into to my NAS when i felt it was nessary. So i decided to stop wasting time looking for something simple, and just decided build something simple :) 

I personally run the app every night and configure the task in windows task manager so it is able to turn on the pc from sleep mode. 

Feel free to branch out and do addtional changes, if you feel like it. I'm open for new idea's and suggestions.


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
