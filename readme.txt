ECHO OSC

ECHO OSC is a powerful application designed to operate the OBSBOT Tiny Series Camera , Tiny & Tiny 4K,  with smooth pan, tilt, and zoom functions. It leverages the power of Open Sound Control (OSC) to provide seamless control over your OBSBOT devices.

Orginal Fork - https://github.com/johnebgood/the-one-camera-controller

Features
Smooth Pan, Tilt, and Zoom operations for OBSBOT TINY and OBSBOT TINY 4K.
Profile functionality to save and load device settings.
Presets Functionality - Save and recall unlimited preset positions. 
OSC commands for easy and efficient device control.
OSC Feedback functionality for real-time device status updates.

The following OSC commands are supported by ECHO OSC:

/XY int int: This command is used to control the pan and tilt of the camera. It takes two integer arguments representing the X and Y coordinates.
/ZOOM int: This command is used to control the zoom of the camera. It takes one integer argument representing the zoom level.
/CENTER: This command is used to center the device. This command takes no arguments.
/POSITION int int int: This command sets the pan, tilt, and zoom of the device. It takes three integer arguments representing the pan, tilt, and zoom values.
/INFO: This command retrieves the current pan, tilt, and zoom of the device. This command requires 1 argument.
/POS int: This command sets the camera position to a saved position. The command takes one argument, the index of the position in the saved positions list. If the index is -1, it cycles to the next position in the list.
/STORE: This command saves the current camera position to the list of saved positions.
/DELETE: This command deletes the current camera position from the list of saved positions.
/TEST int int : This command lets you cycle through the IAMCAMERCONTROL propeties that can control your camera, mainly for troubleshooting purposes. The first argument is the property and the second argument is the property value. 

UPDATE PLANS : To add a speed variant position recal that is stable. 

OSC Functionality Explantion
/XY int int :Used to control the camera for smooth pan and tilt control. Needs to be connected on a float parameter with your OSC sender. The movemnet is triggered to stop when the command /XY 0 0 is recived. 


OSC Feedback 
Osc Feedback will begin after the first osc message is recived by ECHO OSC

Profile Functionality
ECHO OSC provides a profile functionality that allows you to save and load device settings. This is particularly useful when you have specific settings for different scenarios. You can easily switch between profiles without manually adjusting the settings each time.

Preset Functionality
ECHO OSC also supports presets. Presets allow you to save and quickly switch to specific camera positions. You can create a preset by using the `/STORE` command, which saves the current camera position to the list of saved positions. You can then switch to a saved position by using the `/POS int` command, where `int` is the index of the saved position. If the index is `-1`, it cycles to the next position in the list. You can delete a saved position using the `/DELETE` command.

otification Icon Functionality
ECHO OSC provides a notification icon in the system tray for easy access to some of its features. The notification icon has the following functionalities:
- Minimize the app the the notifiction using the minimize button in the application. 
- Double-click: Double-clicking the notification icon will open the ECHO OSC application if it's minimized or hidden.

- Right-click: Right-clicking the notification icon will open a context menu with the following options:
  - Start Server: This option starts the OSC server.
  - Stop Server: This option stops the OSC server.
  - Port: This option displays the current port number. You can change the port number here.
  - ComboBox: This option allows you to select a profile from a dropdown list.

The notification icon provides a quick and easy way to control the ECHO OSC application without having to open the full application window.
Known Issues - BUTTON MALFCUTNIONS - weird behavior 
UP BUTTON LIMIT
DOWN BUTTON LIMIT
UP 
DOWN 
LEFT 
RIGHT
Mainly a stepping issue. 
UPDATE PLANS - to remove remove buttons and replace with XY CONTROL. 


Libraries Used
ECHO OSC uses the following libraries:

DirectShowLib: A .NET library to access DirectShow functionality. (License: LGPL)
Rug.Osc: A simple and lightweight .NET library for handling OSC (Open Sound Control) packets. (License: MIT)
Newtonsoft.Json: A popular high-performance JSON framework for .NET. (License: MIT)
Credits
ECHO OSC wouldn't be possible without these amazing libraries. We would like to express our gratitude to the authors and contributors of these libraries.


ECHO OSC is a robust and efficient tool for controlling your OBSBOT devices. Its intuitive OSC commands, real-time feedback functionality, and profile feature make it a must-have tool for OBSBOT users. Give it a try and experience the difference!

Please note that ECHO OSC is an open-source project. We welcome contributions from the community. If you encounter any issues or have any suggestions, please feel free to open an issue or submit a pull request.

License
ECHO OSC is released under the MIT License. See LICENSE for more information.
Please note that this project, ECHO OSC, is an independent project and is not affiliated, associated, authorized, endorsed by, or in any way officially connected with OBSBOT or REMO TECH, or any of its subsidiaries or its affiliates. The official OBSBOT website can be found at https://www.obsbot.com. The name “OBSBOT” as well as related names, marks, emblems and images are registered trademarks of their respective owners
