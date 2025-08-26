There are two ways of running this application :
1- using the source code and a compiler 
2- using the executable version of the app
Both links for the compressed formats will be available at the end of the README file
The CLI and the simulation display window can work simultaneously, and you can use the keyword "help" inside the CLI for the full command lists
The sample rate* inside the CLI logging can be set between 1 and 60 
* Sample rate is not the same as data creation rate, which is locked in at 60Hz
You can only log one device at a time using the "log" keyword
There is a bug where if you click the X button in the CLI, it closes the whole application, which I am aware of; I just didn't have enough time to fix
This was my first WPF project and also my first MVVM architecture design, and I tried my best to keep it as clean and in line with the standards as possible
All data created gets saved inside a SQLite DB file, which is inside the file directory called "DevicesLogs.db", and will only exist if you run the app at least once
You can view the DB inside the app by using the database button
PS: The Unit testing project was not uploaded to GitHub 

Exe File: https://drive.google.com/file/d/1zRu1zLlvfLB3VHAkSMg_R8mm48mXX-mC/view?usp=drive_link
Source Code: https://drive.google.com/file/d/1YObzZaMFX_X7E79ZtOwATZKMFc5pcOEq/view?usp=drive_link
