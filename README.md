# College Organiser Application

- **Student Name:** Will Hogan
- **Student Number:** G00318460
- **College Name:** GMIT
- **Course:** Software Development
- **Module:** Mobile Application Development 3
- **Lecturer:** Martin Kenirons
- **Current College Year:** 4th Year 
- **Project Title:** College Organiser

# Application Overview
This is a Universal Windows Application(UWP), that has been designed for College Students to help them keep track of various events, tasks and priorities within an educational / college type environment. The application allows users to enter new events and details and save them to the cloud. When offline, the data is stored locally and will then sync with cloud when the user is in a sync enabled environment. 
The application also allows users to create events with a certain priority level. These priority levels and a count for each level are displayed for the user that allows them to visualise the amount of task by priority level. 

The app is straightforward and easy to use. Below is a breakdown of the controls on the visual display and how they operate:

| *Control Type* | *Function*      |
| ------------- |:-------------:|
| **Event / Module Name**    | Here you enter the Module or subject associated with the event  |
| **Deadline**         | This is broken up into various drop down lists [Day, Date, Month, Year] |
| **Percentage of Module**      | This is how much of your subject is impacted by this percentage. Leave at 0% if not relevant |
| **Priority Level**  | There are 4 different levels [URGENT, NORMAL, LOW, None] |
| **Add** | Adds an event to your list and immediately displays that event in the list event list area |
| **Sync** | Performs a sync with the Cloud server (Azure). Any existing events will be retrieved and displayed in the list area |

### Compatibility
This app was created as a universal windows application and will work on any of the below devices:
* Windows Phone
* Windows Tablet / Fablet
* Windows Desktop / Laptop
* Windows Larger Computer Monitors

---

# Technical Details

### Structure of Project
I used Visual Studio 2015 for my development and testing of this application. In order to identify key parts of the project that stood out from others, I opted to separate relevant code into specific folders (see 'Design Patterns' below). 

### Design Pattern
Although this application doesn't have the modularity associated with a full MVVM (Model, View, View, Model) design pattern structure, it does contain flavours of it. For example, all of my Views are in a separate folder and my Data has also been separated where possible. However, the local and cloud based information in the application are highly coupled with my C# code. 

### Programming languages / Technologies used

| *Technology/Language* | *How it was used in the Application* |
| ------------- |:-------------:|
| **C#** | Used throughout the application to communicate with the View, Cloud and local storage |
| **XAML** | The language used for the View, the visual side of the application |
| **SQLite** | Used to store offline data locally, using a minimalistic version of SQL |
| **Azure Mobile Services** | Used as part of a PaaS (Platform as a Service) to store my events in the cloud |
| **Newtonsoft (JSON)** | Used to map my data in the form of data classes in C#, to my azure model / SQLite model |
| **GitHub Extension** | I used this plug in to help with Version Control and i was able to manage everything locally and remotely |
| **Zen Hub** | This is a GitHub project management extension that allowed me to manage all aspects of my project |


### GitHub
I used GitHub extensively throughout my application development, with particular emphasis on the following areas;
* **Commits** - I kept regular track of what was being committed and pushed
* **Branches** - I made a number of branches that used in conjunction with my master branch and allowed for prototype development
* **Project Management** - I used GitHub for my project management. This allowed me to create milestones and track issues associated with them. As mentioned above, i used the GitHub integration tool Zen Hub which allowed me to visualise my outstanding Milestones and Issues on a Kanban board. This is all held remotely on the GitHub site. 

### Localisation
All hard coded text values have been removed from the application display and will be injected at runtime. This allows for a scalable application that can be changed to fit another countries language criteria, with this another language can be added to the .resw file

### Comments
Each code section has been fully commented to enable other developers to understand my code and who may want to use parts of my project in their own projects, to understand what's going on, but also to help myself in the future should i revisit the application. 

---

# Deployment Instructions
To download and run this application, here's what you'll need.

A fully installed version of Visual Studio 2015. 
After successfully installing, navigate to the nuget package manager within visual studio and add the following packages dependencies to your references folder;

* Microsoft.NETCore.UniversalWindowsPlatform
* Microsoft.Azure.Mobile.Client.SQLiteStore
* Newtonsoft.Json

### SQLite local installation instructions
To be able to run this application through Visual Studio 2015, you will need to install SQLite. 
Please follow these steps:

- Go to this site [https://www.sqlite.org/download.html](https://www.sqlite.org/download.html)
- Download the latest version of SQLite for ```Universal Windows Platform```
- Open up Visual Studio
- There should be a Reference to SQLite in the References Folder of your Solution Explorer, if so skip the next few steps and continue from 'Go to Tools'
- Right click on the ```References``` folder of your ```Solution Explorer``` and click ```Add Reference```
- On the pop up window, go to the Universal Windows section on the left and click ```Extensions``` 
- Tick the ```SQLite for Universal App platform``` box, then OK
- Go to tools => NuGet package manager => Package Manager Console
- In the package manager console, type the following ```Install-Package SQLite.Net-PCL```
- At this point you should be informed in screen that the package was successfully installed

If you are planning on building something similar using SQLite, you may need something to visualise the backend. 
My choice was a SQLite IDE called ```sqlitebrowser```, which will help you monitor and test values as they are being inserted and deleted. 
[Here](http://sqlitebrowser.org/) is the link to their site. 

### Azure Database instance information
For your cloud data sync, you'll need to have an Azure Portal account. [Here](https://portal.azure.com/) is the link to help you get started. You'll also need to change the url endpoint address of your database instance on azure, in your ***App.xaml.cs*** file

---

# Screenshots

### Main Page

![title]("https://github.com/willhogan11/CollegeOrganiser/blob/master/CollegeOrganiser/CollegeOrganiser/Images/MainPage.jpg")

### Menu Page

### Events Page

