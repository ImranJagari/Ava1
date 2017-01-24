# Ava1

Ava1 is a non-profit C# server side emulator for the game StreetGears.

Note: I was still learning at this time

- [**1. Setup**](#required-setup)
  - [1.1 Required Files](#required-setup)
  - [1.2 MySQL Database installation](#database-setup)
  - [1.3 Ava1 Setup](#ava1-setup)
  - [1.4 Comptability](#comptability-setup)
- [**2. Informations**](#database-informations)
  - [2.1 Account Database](#database-informations)
  - [2.2 Server Database](#database-informations)
  - [2.3 Clan Database](#database-informations)
  - [2.4 Packet Protocol](#packet-informations)
- [**3. Community**](https://github.com/greatmaes/Ava1/wiki)
  - [3.1 Isssues tracker](https://github.com/greatmaes/Ava1/issues)
  - [3.2 Wiki](https://github.com/greatmaes/Ava1/wiki)
  
### Required Files (*Setup*)

To successfully setup and play with Ava1 you must have downloaded:
- .NET Framework 4.5.2

### Database (*Setup*)

First you will need to create a MySQL Database. We will use wamp in this tutorial to create a local database and Navicat Premium to setup our database.

Once Wamp is downloaded and started, start Navicat or any database manager and create a new connection to your wamp local server port.

![Image1](https://raw.githubusercontent.com/greatmaes/Ava1/master/img/db1.jpg)

Then create a new database, we will name it 'db_streetgears'

![Image2](https://raw.githubusercontent.com/greatmaes/Ava1/master/img/db2.jpg)

Open it and add it the SQL file from Ava1/SQL.

![Image2](https://raw.githubusercontent.com/greatmaes/Ava1/master/img/db3.jpg)

Refresh, tables should pop up, we're done.

![Image2](https://raw.githubusercontent.com/greatmaes/Ava1/master/img/db4.jpg)

Once it's done configure the database part of the config.ini file with your informations like this.

```
[Database]
Host=YOUR_DB_HOST
User=YOUR_DB_USERNAME
Password=YOUR_DB_PASSWORD
Database=YOUR_DB_NAME
```

### Ava1 (*Setup*)

Place every files from the Ava1 folder you downloaded on the StreetGears folder except the sql folder

![Image5](https://raw.githubusercontent.com/greatmaes/Ava1/master/img/files1.jpg)

Finally, start "*Ava1.exe*" to start the emulator and StreetGear.

### Comptability (*Setup*)

If you have any problem running both game and emulator on windows 7/vista/xp then try to change comptability settings to "*Windows 95*" of both game and emulator like this, this may help.

![Image6](https://raw.githubusercontent.com/greatmaes/Ava1/master/img/comptability.jpg)

### Account Database (*Informations*)
Key | Description
--- | -----------
id   | (int) Player's id
user   | (string) Player's username
password    | (string) Player's password
char_name   | (string) Player's nickname
char_rank   | (string) Admin,Developper,Player,Bot,Banned.
char_type   | (int) Player's character. 0=Luna, 1=Tippy, 2=Rush, 3=Rookie, 4=Kara, 5=Klaus.
char_sex    | (int) Player's sex.
char_level  | (int) Player's level. 0-45
char_exp    | (int) Player's experience. 0-102400 (0%-100%)
char_licence   | (int) Player's licence. 0-4
char_cash   | (int) Player's gpotatos. 0-999,999,999
char_gamecash   | (int) Player's rupees. 0-999,999,999
char_coins   | (int) Player's coins. 0-999
char_questpoints   | (int) Player's questpoints. 0-999,999,999
char_tricks   | (string) Player's trick level list. "trick_id,trick_level_trick_apply" Actual order: Grind,Backflip,FrontFlip,Airtwist,Powerswing,Gripturn,Dash,Backskating,Jumpingsteer,Butting,Powerslide,Powerjump,Wallride
char_inventory   | (string) Player's inventory. "item_id,item_tradestatus,item_duration,item_equiped")
char_clanname   | (string) Player's clan name.
char_age    | (int) Player's age.
char_zone   | (string) Player's country info.
char_bio    | (string) Player's bio.
char_country   | (int) Player's country.
first_login   | (int) Boolean 0=True, 1=False. 1 will push you on the character creation screen. Used to check if the player logged before. 

### Server Database (*Informations*)
Key | Description
--- | -----------
id   | (int) Server's id
name   | (string) Server name
channel_status   | (int) Channel status. 0=Open, 1=Closed
channel_maxnum   | (int) Channel maximum player count
channel_currentnum   | (int) Channel current player count

### Clan Database (*Informations*)
Key | Description
--- | -----------
id   | (int) Clan id
clan_name   | (string) Clan name
clan_level   | (int) Clan level 0-200
clan_exp   | (int) Clan experience 
clan_motd   | (string) Clan message of the day
clan_members   | (string) Clan members. "player_name,permission|player_name2,permission" Permission: 0=None, 1=Sub Leader, 2=Leader
clan_points   | (int) Clan points

### Packets (*Informations*)
![Image7](https://raw.githubusercontent.com/greatmaes/Ava1/master/img/struct.jpg)

# Binaries
- [**Lastest update (v1.0.0.0)**](https://github.com/greatmaes/StreetEngine-Emulator/releases/tag/1.0.0.1)
  - [Binary](https://github.com/greatmaes/StreetEngine-Emulator/releases/download/1.0.0.1/StreetEngine-Emulator-Binary.rar)
  - [Full Source Code](https://github.com/greatmaes/StreetEngine-Emulator/releases/download/1.0.0.1/StreetEngine-Emulator-Full-Source.rar)

# Credits (In alphabetical order)
- Geekgame
- itsexe
- K1ramox
- skeezr