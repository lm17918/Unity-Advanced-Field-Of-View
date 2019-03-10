# unity-Field-of-view-advanced

In this project I expand the project called "unity field of view"  adding several targets that much be seen from the main character.
As you can see, in this image I use the same field ov view from the other project but in the map the are several capsule object that are marked as "targets".
<img src="https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/example%201.PNG" width="200"> <img src="https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/example%202.PNG" width="185"> 

In this version some importations are printed on the screen:
<img src="https://github.com/lm17918/unity-Field-of-view-advanced/blob/master/example%203.PNG" width="800">
These info represent:
-The number of targets seen by the main character in the 360°
-The number of targets seen by the main character only in its field of view
-The number of targets hidden by obstacles in the field of viewof the main character  

Looking at the code in the "controller" script you will be able to see three functions called :
-FOV_seen (where the number of target seen in the filed of view is calculated)
-FOV_hidden (where the number of target hidden in the filed of view is calculated)
-FOV_360 (where the number of target seen at 360° in the filed of view is calculated)

In these functions there are some lines that are not executed by the program because are comment.
In "FOV_seen" you can use those lines to have a list of the names of the targets seen in the field of view(targets_seen). 
In "FOV_hidden" you can use those lines to have a list of the names of the targets hidden in the field of view(hidden_targets). 
In "FOV_360" you can use those lines to have a list of the names of the targets see at 360°(degreespoints).

I hope that this code will help you with your project!


For any question send me an email at lorenzo.mirante@outlook.it








