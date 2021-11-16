# IKScenario

## Description

This project is about inverse kinematics. In this project, I implemented inverse kinematics in 2D and 3D. In each version, there are one or more characters in the scene with two arms. For each character, there is a green ball between two arms and the character tries to reach the ball using its arm. In 3D version, I used two strategies to solve the problem. One was Cyclic Coordinate Descent(CCD), which I combined quaternion to adjust the rotation from end of the arm to root of the arm between each joint. The other was Fabirk, which gradually adjusted the position of joint from end of arm to root of arm and from root to end of arm back and force. 

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

## Challenge Write Up & Diffifulties

In this project, I implemented both basic IK and IK Fabrik. 

IK Fabrik was very simple to implement because it just modified the transform of each joint back and force between the target, if reachable, and root for each frame. If the ball was too far to reach, the joints only needed to point at the direction of target. The algorithm itself does not involve any rotation, which looks simple. With the joints only, the program seemed perform correctly. However, when I attached arm(cubes) to one end of joint, the arm looked disconnected to the other end of joint (image attached below). I then solved this problem by rotating the forward vector of the arm(cube) always pointed to the target joint. Also because of this reason, it is more difficult to apply limitation on Fabirk.

CCD is more difficult to implement comparing with Fabrik because it involves a large amount of rotation, which is not quite easy to compute. For 2D, rotation can be computed using sin and cos. However in 3D, we have to use either rotation matrix or quaternion. I chose to do rotation with quaternion, by finding the 4th plane between two vectors and rotating certain angles between two vectors based on the plane. Furthermore, if one of the parent joints is incorrect, the rotation will be totally wrong. One advantage for CCD is that it is easily to set the limitation for joints in CCD because we use angle during computation, which is quite straight forward. In 2D, the joint limit is very easy find for each arm because it only has one degree of freedom and the limit can be found by trying different values for several times. Nonetheless, it is not easy to find the proper limitation, especially in 3D. The reason is in 3D, we have more degree of freedom, sometimes the arm might rotate to a degree which we do not expected. However, In think CCD performs better because the rotation transitions natually between joints. 


## References

Textures: 

https://assetstore.unity.com/packages/2d/textures-materials/sky/skybox-series-free-103633

https://assetstore.unity.com/packages/2d/textures-materials/lowpoly-textures-pack-140717
