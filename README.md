# UnityVRGrabberExample

Simple grabbing system with animated hands for Unity.

Based on [VRSandBoxGame from ololralph](https://github.com/ololralph/vrsandboxgame). Thank you ololralph.
Created with Unity 2019.1.13f1.
It was tested with an Oculus Rift and an Oculus Quest.

Use the package "XR Legacy Input Helpers" because at the time this example was created, [Unity suggest to continue to use the XR legacy system](https://forum.unity.com/threads/any-example-of-the-new-2019-1-xr-input-system.629824/#post-4513171) and I wanted to use the simple TrackedPoseDriver Component.

# Why This ?

I wanted to have VR animated hands to grab objects in Unity **without using the Oculus SDK or any other SDK**
I wanted something to indicate when you can grab an object and when you are grabbing it.

# Download
You can clone the repo or [download the unitypackage](https://github.com/Thomas-6freedom/UnityVRGrabberExample/releases/latest)

# How to Use it ?
You simply have to use the Left Hand and the Right Hand prefab. Then you have a Grabbable prefab. There is a test scene named "VRHandExampleScene".

![](https://thumbs.gfycat.com/PolishedNiceBluebottle-size_restricted.gif)


#Troubleshoot
If you have only one hand usable at one time. Add this line to your manifest: 
`<uses-feature android:name="android.hardware.vr.headtracking" android:required="true" android:version="1"/>`

If you have a Physics2D compiler error; remove Unity UI in your package manager
