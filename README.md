## shadermotion-to-bvh

This is found in `Assets/shadermotion-dev/Example/Example2.scene`

Converts shadermotion videos to Bvh (with mixamo rig)

This is a tool to convert videos recorded using [shadermotion](https://www.youtube.com/watch?v=r8YpXP0RlZc) (a shader made by lox that allows you to record mocap data by projecting the mocap data into the screen), into BVH, the standard mocap format (that can be open in Blender, Maya, etc).

To use it, download the latest release, and run the executable `shadermotion2.exe` with argument `--video [video_filename]` where `[video_filename]` should be the path to the video file you want to conver to bvh. For example if the video is `my_video.mp4`, then you can run `shadermotion2.exe --video my_video.mp4` from the command line after navigating to the folder containing the shadermotion2.exe executable.

There's also a bat file provided which can convert all the videos inside a folder. You just need to edit it, and change the variable `args1` to be the path of the folder containing the videos you want to convert to bvh (which should be in mp4 format). The results will be saved inside `Assets/bvh`.

This tool is using lox9973's [shadermotion](https://gitlab.com/lox9973/ShaderMotion), and emilianavt's [BVHTools](https://github.com/emilianavt/BVHTools) for unity. 

## BVH to Neos

This is found in `Assets/shadermotion-dev/Example/Example2_motion_stream.scene`
