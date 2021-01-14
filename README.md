# unity-Field-of-view

In this repository I expand the project called "unity field of view" from https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague.
The purpose of this project is to give to the user a more complex simulation of what a point of interest, moving in an environemnt, "sees" inside a specific view radius and field of view.

The environment used in this project is the "Sci-Fi Styled Modular Pack", a free package available on the Unity store. Here three 3DGameobjects capsule are placed, two with the role of being the targets and one controlled by the user with mouse and keyboard.
![alt text](https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/images/figure1.PNG)

The capsule GameObject controlled by the user has attached a script called "Controller" where the user can set:

```
- The view radius: the max distance from which we assume the capsule moved from the user is able to see the targets.
- The view angle: the FOV angle simulated from the capsule center.
- The Traget Mask: The Layer name assigned the two targets in the scene.
- The Obstable Mast: the Layer assigned to the environment in the scene.
```

In this environment the capsule GameObject controlled by the user has a field of view, the green pattern, that simulates what the characted can see. The rest of the 360 degrees FOV is shown as a blue pattern (what the capsule would see if it turns around its 360 degrees). When the GameObject controlled by the user is in front of an obstacle that doesn't allow him to see through, the FOV until the obstable is presented as green pattern while the rest of the field of view that is blocked by the obstacle is shown as red. some examples are shown in the pitures below.

![alt text](https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/images/figure2.PNG)
![alt text](https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/images/figure3.PNG)
![alt text](https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/images/figure4.PNG)
![alt text](https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/images/figure5.PNG)

In these picture it is also shown how the two moving targets in the scene change their color entering the field of view of the GameObject controlled by the user.
This is just an example to show that it is also possible to have a list of the targets inside and outside the field of view and interact with them.
When one of the targets enters the view radius it changes its color accordingly, and once it leaves the view radius of the GameObject it goes back to a white color.

The user has access to this data searching in the script called "Controller" for:

```
- List < GameObject > allTargets: Representing all the targets in the scene.
- List < GameObject > targetsInFOV: Representing the targets seen in the FOV(green).
- List < GameObject > targetsHideen: Representing the targets hidden by obstacles(red).
- List < GameObject > targetsOutsideFOV: Representing the targets outside the FOV but in the view radius(blue).
```
