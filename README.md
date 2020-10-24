<p align="center">
  <a href="https://discord.gg/4PpDrCX">
    <img src="https://discordapp.com/api/guilds/767835739177091083/widget.png?style=shield" alt="Discord Server">
  </a>
  ![.NET Core](https://github.com/MattTheDev/DustBunny/workflows/.NET%20Core/badge.svg)
</p>

# DustBunny
Discord Bot / Process Manager

### Configuration File Explanation

#### Ids
```json
  "Ids": {
    "Bot": "DISCORD_BOT_ID",
    "Owner": "YOUR_DISCORD_ID",
    "Server": "SERVER_ID_BOT_RESIDES_IN"
  },
```

* Bot - The Discord ID of the DustBunny Bot
* Owner - Your Discord ID
* Server - The Server the DustBunny Bot resides on

```json
  "Bots": [
    {
      "Id": "DISCORD_BOT_ID",
      "Name": "DISCORD_BOT_NAME",
      "AnnounceChannel": "DISCORD_ANNOUNCE_CHANNEL",
      "OfflineTime": "",
      "Path": "DISCORD_BOT_EXE_PATH",
      "ProcessName": "DISCORD_BOT_PROCESS_PATH"
    }
  ],
```

A list of Bots you want to monitor. You can monitor more than one! 

* Id - Discord ID of the bot you want to monitor the status of.
* Name - Name of the bot you want to monitor the status of.
* AnnounceChannel - Discord ID of the channel you want announcements to go to.
* OfflineTime - Do not touch. This is used by DustBunny.
* Path - File path to the startup exe for the bot, ie: C:\Bots\Bot1\Bot1.exe
* ProcessName - Name of the process the bot runs under

```json
  "Processes": [
    {
      "Name": "PROCESS_DISPLAY_NAME",
      "AnnounceChannel": "DISCORD_ANNOUNCE_CHANNEL",
      "OfflineTime": "",
      "Path": "PROCESS_EXE_PATH",
      "ProcessName": "PROCESS_NAME"
    }
  ],
```

A list of System Processes you want to monitor. You can monitor more than one! 

* Name - Name of the process you want to monitor.
* AnnounceChannel - Discord ID of the channel you want announcements to go to.
* OfflineTime - Do not touch. This is used by DustBunny.
* Path - File path to the startup exe for the bot, ie: C:\Bots\Bot1\Bot1.exe
* ProcessName - Name of the process the bot runs under, ie: Bot1

```json
  "Tokens": {
    "Discord": "DISCORD_MONITOR_BOT_TOKEN"
  }
```

1. Visit the [Discord Developers Portal](http://discord.com/developers/applications)
2. Log in with your Discord Username and Password
3. Create a new Application, Name it your desired bot name.
4. Once created, click Bot in the left navigation menu.
5. Click add bot, and confirm with 'Yes, do it!'
6. Once the bot has been created, click 'Click to Reveal Token'
7. Save this token. >> DO NOT SHARE IT <<
8. Put that token here.
