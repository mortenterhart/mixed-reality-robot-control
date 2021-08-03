# Mixed Reality Robot Control

This project implements a robot control for a [Fischertechnik TXT robot](https://www.fischertechnik.de/en/products/playing/robotics/522429-txt-controller) by using
a Mixed Reality simulation on the [Microsoft HoloLens 2](https://www.microsoft.com/en-us/hololens/buy) and the [HP Reverb G2](https://www.hp.com/us-en/vr/reverb-g2-vr-headset.html).
The AR/VR simulation for these headsets is implemented with the [Mixed Reality Toolkit (MRTK) for Unity](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/unity/mrtk-getting-started) by Microsoft.

## Installation

This guide explains how to setup the devices to get started with the Mixed Reality robot control.

### Setting up the Raspberry Pi

The following instructions explain how to setup the Raspberry Pi for the robot control. If the Raspberry Pi is used that was already setup
for this project, only steps 7 and 8 are necessary.

1. Turn on the Raspberry Pi and login with user `pi` and password `Mrtk2021`.
2. Install the MQTT broker [Mosquitto](https://mosquitto.org):

   ```
   $ sudo apt-get install -y mosquitto
   ```

3. Install Python3 and `pip3`:

   ```
   $ sudo apt-get install -y python3 pip3
   ```

4. Install the required Python packages for the control:

   ```
   $ pip3 install ftrobopy paho-mqtt
   ```

5. Restart the Raspberry Pi with `sudo reboot` to start Mosquitto in order to accept commands.
6. Clone the repository and change into the `HighBayStorageRack` directory:

   ```
   $ git clone https://github.com/mortenterhart/mixed-reality-robot-control
   $ cd mixed-reality-robot-control/HighBayStorageRack
   ```

This directory contains the Python script `hbsr.py` that receives the commands and redirects them
to the robot.

7. Connect the Raspberry Pi to the robot's Wifi `ft-txt_5528`. Instructions for this can be found
   [here](https://www.raspberrypi.org/documentation/configuration/wireless/wireless-cli.md). The
   password is displayed at the robot.
   
8. Run `hbsr.py` to start the robot control:

   ```
   $ python3 hbsr.py
   ```

Now the Raspberry Pi is ready to accept commands from the headsets and redirect them to the robot.

### Setting up the Headsets

The following instructions explain how to transfer the AR and VR simulation of the control from the PC
to the Microsoft HoloLens 2 and the HP Reverb G2. To execute the control, Visual Studio 2019 is required
which can be downloaded [here](https://visualstudio.microsoft.com/de/downloads).

1. Download the Unity builds for AR and VR from <https://github.com/mortenterhart/mixed-reality-robot-control/releases/latest>
and extract the ZIP files to your disk.

**For Microsoft HoloLens 2:**

2. Turn on the headset and connect it to the PC with USB.
3. Enable the developer mode on the HoloLens. Instructions for this can be found
   [here](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/platform-capabilities-and-apis/using-visual-studio?tabs=hl2#enabling-developer-mode).
4. Connect the HoloLens to the robot's Wifi `ft-txt_5528`.
5. Open the solution file `mr-robot-control.sln` from the AR build in Visual Studio.
6. Select the run configuration **Master**, the platform **ARM64** and the deployment target **Device**:

   ![Run Configuration for Microsoft HoloLens 2](https://i.imgur.com/UAKbrqr.png)

7. Start the simulation on the HoloLens using the menu **Debug** 🡒 **Start without Debugging** or press <kbd>Ctrl+F5</kbd>.

**For HP Reverb G2:**

2. Connect the headset to the PC over USB and plug in the power cable.
3. Connect the PC to the robot's Wifi `ft-txt_5528`.
4. Open the solution file `mr-robot-control.sln` from the VR build in Visual Studio.
5. Select the run configuration **Master**, the platform **x86** and the deployment target **Local Computer**:

   ![Run Configuration for HP Reverb G2](https://i.imgur.com/ePReOPV.png)

6. Start the simulation on the HP Reverb G2 using the menu **Debug** 🡒 **Start without Debugging** or press <kbd>Ctrl+F5</kbd>.

The robot control is now executed on the headsets and can submit commands to the robot over the Raspberry Pi.
An important requirement for this is that all devices are properly connected to the robot's Wifi.

## Helpful Resources

### Installation

* [Fischertechnik TXT Community Firmware](https://cfw.ftcommunity.de/ftcommunity-TXT/de/)
* [Installing the tools for MRTK](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/install-the-tools?tabs=unity)

### Getting Started

* [Choosing Unity Version and XR Plugin](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/unity/choosing-unity-version)
* [Creating an MRTK project in Unity for HoloLens 2](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/unity/tutorials/mr-learning-base-02?tabs=openxr)
* [Using the Mixed Reality OpenXR Plugin](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/unity/openxr-getting-started)

### Development

* [Unity Development for HoloLens](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/unity/unity-development-overview?tabs=arr%2Chl2)
* [Unity Development for VR and Windows Mixed Reality](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/unity/unity-development-wmr-overview)

### Deployment

* [Using Visual Studio to deploy and debug](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/platform-capabilities-and-apis/using-visual-studio?tabs=hl2)
* [Using the HoloLens Emulator](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/platform-capabilities-and-apis/using-the-hololens-emulator)
* [Using the Windows Mixed Reality Simulator for immersive headsets](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/platform-capabilities-and-apis/using-the-windows-mixed-reality-simulator)
* [Recommended Unity Settings](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/unity/recommended-settings-for-unity)

### Examples

* [Examples for HoloLens 2](https://docs.microsoft.com/de-de/windows/mixed-reality/develop/features-and-samples?tabs=unity)
* [Sample Unity projects for OpenXR](https://github.com/microsoft/OpenXR-Unity-MixedReality-Samples)
* [Periodic Table of the Elements](https://github.com/microsoft/MRDL_Unity_PeriodicTable)
