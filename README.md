# unity-2D-Field-of-view-advanced

In this repository I expand the project called "unity field of view" from https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague.
The purpose of this project is to give to the user a starting point to imprement a 2D simulation of what a point of interest would see in an environment.

I use for the environment the "Sci-Fi Styled Modular Pack", a free package available on the Unity store. Here I place three 3DGameobjects capsule, two for the targets and one for the main character that is controlled by the user with mouse and keyboard (for a better explanation about how these controls are implemented look at https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague).
fig2

the controled character setup is cofigured from one script that is attached to him colled "Controller" where the user can set:
the view radius: the max distance from the character center for which we assume the character is able to see.
the view angle: the FOV angle from the character center.
the Traget Mask: The Layer name to which are assigned the two other capsule Gameobject
the Obstable Mast: the Layer assigned to the environment.

In this environment the characted has a field of view that is coloured in green, that simulates what the characted can see, while the rest of the 360 degrees FOV around him is shown as a blue pattern ( what the character would see if it would turn around the 360 degrees). When the character is in front of an obstacles that do not allow him to see thru, the FOV until the obstable is presented as green pattern while the rest of the field of view that is blocked by the obstacle is shown as red.

When the project is running there are also two capsule moving in the environment. These two capsules have their layer mask set as "targets". when one of the capsule enter the view radius of the chatracter it changes its colour with the same FOV colour, so if it will be in the blue part of the view radius will become blue, if it will reach the green part will become green and if it will he hidden behind an obstable will get the red colour. Once it leaves the view radius of the main character it goes back to a white colur. this is just an example to make the player understand the position of the targets relative to the FOV of the main character.

In the script called controller you can access to several lists representing:
all the targets in the scene, accessing the List < GameObject > allTargets
the targets seen in the FOV, accessing the List < GameObject > targetsInFOV
the targets hidden by obstacles, accessing the List < GameObject > targetsHideen
the targets outside the FOV but in the view radius, accessing the List < GameObject > targetsOutsideFOV

<img src="https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/example%201.PNG" width="200">
