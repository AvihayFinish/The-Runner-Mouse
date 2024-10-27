# Developers:
> Avihay Finish, 208907113.

> Amit Rovshitz, 207701426.

> Elad Laster, 208968636.

- [About the Project](#about-the-project)
- [Get the Projet](#get-the-projet)
- [Important Note](#important-note)
- [Development Mode](#development-mode)
- [Contributing](#contributing)

# About the Project:
The current game is developed for the "Beit Loewenstein" rehabilitation center. At this center, the Amadeo machine is used to detect the forces applied by the patient's fingers. <br> 
This machine helps patients restore movement abilities, stability, strength, and more. 
To encourage patients to engage with the machine and complete their tasks, we developed this game, which interfaces with the Amadeo machine. <br>

# Get the Projet:
To download and work on the project, first install [Unity 2022.3.16f1](https://unity.com/releases/editor/whats-new/2022.3.16). Then, click the green "Code" button on the repository page and copy the URL provided. <br>
Next, create a new directory on your computer and open it in the command prompt. Run the command: `git clone <copied-URL>`. <br>
After the project is cloned, open **Unity Hub**, click the arrow next to the "Add" button, and select "Add project from disk". <br>
Navigate to the directory where you cloned the project and select it. Finally, open the project from Unity Hub, and Unity will launch the project. <br>

# Important Note:
To fully understand the game and its components, please read this [article](https://github.com/AvihayFinish/The-Runner-Mouse/blob/main/The%20Runner%20Mouse%20Article.pdf). The article contains all the information you need.

# Development Mode:
As mentioned earlier, the game is developed for the Amadeo machine, and the control of the mouse (the in-game character) is adjusted for this device. <br>
To switch the controls to the keyboard's arrow keys for development mode, follow these steps: - <br>
> - Open the "Scene_Rat" scene. In this scene, locate the object named "AmadeoClient" and click on it. In the inspector, you'll find a script called Amadeo Client.
Uncheck the Is Amadeo variable by clicking the square next to it. <br>
> - Open the script "InputMover" and set the notGetForcesFromAmadeo variable to true.
 
## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.
