# vam-scripts

These are Unity scripts that can be loaded into VAM as plugins. They belong in this directory alongside the PluginBuilder solution:
VaM_Updater\Custom\Scripts\MeshedVR

## Keymotion

Keymotion plugin can be added to any Atom that has force receivers in VAM. In this example, I am using a Sphere.

![image](https://github.com/user-attachments/assets/f099ab43-ce75-46e6-b7ba-71f4729aa808)

The plugin can be added by Selecting Atom and navigating to the Plugins panel

![image](https://github.com/user-attachments/assets/c79ba866-6246-46ae-b289-9af8f9195fba)

![image](https://github.com/user-attachments/assets/96320d9b-0709-4fee-82ae-aa0e5328a80f)


For the plugin to work properly, set the "Hold Position Max Force" to 0. 
This can be accessed from Physics Control panel of the Atom's properties. Otherwise it will counteract any forces applied by the plugin.

![image](https://github.com/user-attachments/assets/8a66a929-c6b1-456b-8cc5-85e8a7027380)


Set Bounciness and Bounce Combine to "Maximum" to notice bounce effects. This can be accessed from the Physics Object panel

![image](https://github.com/user-attachments/assets/08ccdc43-5189-4461-b226-28ed618b74a2)


### Plugin custom UI

![image](https://github.com/user-attachments/assets/3d901162-c4ce-4ebb-8d13-8f8d0c5300b8)


### Demo

https://i.giphy.com/media/v1.Y2lkPTc5MGI3NjExaDV0aDdnNnQ4bGpzMG5janc0ZGZmZWZobHNjYWp6ZHpma3psbXc5eSZlcD12MV9pbnRlcm5hbF9naWZfYnlfaWQmY3Q9Zw/pTgfFfJiGwQjZXWwA6/giphy.gif
