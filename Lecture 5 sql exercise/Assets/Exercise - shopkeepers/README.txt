In this exercise you have to:
-design a database for holding the item/inventory information
-integrate it with the unity game (e.g. retrieve the information)
-display the retrieved information with the UI
-extra: add an inventory to the player and complete the buying mechanic

Things the game already does:
-CameraController just moves the camera
-PlayerController moves the player and activates the shop interface when pressing E close to a shopkeeper (calls some functions in the shopkeeper script)
-Item contains a helper class that holds some key info for items. You can keep it as is, or modify it as you want.

The Shopkeeper script is where you would want to do most of your work. It has two main methods:
-ActivateShop() this is going to be pretty complex
-DeactivateShop() this is quite easy :) 


The shop interface is built with the Shop Canvas game object. It can display a max of 8 items in the current implementation.