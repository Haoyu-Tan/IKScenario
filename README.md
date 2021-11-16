# IKScenario

## Description

This project is about inverse kinematics. In this project, I implemented inverse kinematics in 2D (in processing) and 3D (in unity). In each version, there are one or more characters in the scene with two arms. For each character, there is a green ball between two arms and the character tries to reach the ball using its arm. In 3D version, I used two strategies to solve the problem. One was Cyclic Coordinate Descent(CCD), which I combined quaternion to adjust the rotation from end of the arm to root of the arm between each joint. The other was Fabirk, which gradually adjusted the position of joint from end of arm to root of arm and from root to end of arm back and force. 

## Features Implemented

### (1) PART 2

* Multi-Arm IK (included single-arm IK)
* Joint limits
* Obstacles
* 3D Simulation and Rendering
* User Interaction

### (2) Challenge (grad*)
* Alternative IK Solver (Fabrik)

## User Control

### (1) 2D Version

* pressing 'w' 'a' 's' 'd'  to move the target (a green sphere)
* mouse drag to move obstacles in scene (red spheres)
* pressing 'z' on keyboard to switch between arms with and without limited. The default mode is limited arm

### (2) 3D Version

* pressing 'w' 's' 'a' 'd' to move the camera up and down or left and right. 
* pressing 'q' 'e' to move the camera forward and backward
* pressing 'up' 'down' 'left' 'right' to rotate the camera
* mouse drag to move each target(green ball)
* For each character, there is a tag above its head to clarify the method I implemented

## Images



## Videos & time stamp

Link: https://youtu.be/ys9APLU1Kdw

Time stamp:

00:02-00:21 2D multiple arms without limit

00:22-00:40 2D multiple arms with limit

00:44-01:31 2D collision with arm limited

1:32-1:47 2D collision without arm limited

1:49-2:50 3D CCD multiple arms with limit

2:51-3:30 3D CCD multiple arms without limit

3:31-4:09 3D Fabrik with multiple arms


## Challenge Write Up & Diffifulties

In this project, I implemented both basic IK and IK Fabrik. 

IK Fabrik is very simple to implement because it just modifies the transform of each joint back and force between the target, if reachable, and root for each frame. If the ball is too far to reach, the joints only need to point at the direction of target. The algorithm itself does not involve any rotation, which looks simple. With the joints only, the program seems perform correctly. However, when I attached arm(cubes) to one end of joint, the arm looks disconnected to the other end of joint (image attached below). I then solves this problem by rotating the forward vector of the arm(cube) always pointed to the target joint. Also because of this reason, it is more difficult to apply limitation on Fabirk.

CCD is more difficult to implement comparing with Fabrik because it involves a large amount of rotation, which is not quite easy to compute. For 2D, rotation can be computed using sin and cos. However in 3D, we have to use either rotation matrix or quaternion. I chose to do rotation with quaternion, by finding the 4th plane between two vectors and rotating certain angles between two vectors based on the plane. Furthermore, if one of the parent joints is incorrect, the rotation will be totally wrong. One advantage for CCD is that it is easily to set the limitation for joints in CCD because we use angle during computation, which is quite straight forward. In 2D, the joint limit is very easy find for each arm because it only has one degree of freedom and the limit can be found by trying different values for several times. Nonetheless, it is not easy to find the proper limitation, especially in 3D. The reason is in 3D, we have more degrees of freedom, sometimes the arm might rotate to a degree which we do not expected. However, In think CCD performs better because the transformation looks more natual between joints. 

Difficulties that I encounted was finding the proper limits for joints in 3D. My character looks natual when perform some actions, such as hugging the ball. However, it is easy to stuck at some position due to the limitation and it requires us to wisely move the target to bring it back. Furthermore, there is no gaurenteen that the conversion from quaterion back to euler provide the correct value because there might be more than one results for a rotation.

![8e97506a5e370a34072906485d69c43](https://user-images.githubusercontent.com/35856355/141891980-88e59ca3-e354-4f6e-90e7-5aa2f704a48e.png)


## References

Textures: 

https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633

https://assetstore.unity.com/packages/2d/textures-materials/lowpoly-textures-pack-140717
