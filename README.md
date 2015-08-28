IberianInvasion: Ant Visualization with the Microsoft Kinect - An Interactive Surrealist Piece

Make sure Visual Studio 2013 (or later) is installed.

This project's uses the Microsoft Kinect v1 Cam so make sure to use SDK v1.8 (SDK v2.0 is only compatible with the Microsoft Kinect v2 Cam).
Certain parts of the code in the SDK vary from v1.0 to v2.0 so it is important to use SDK v1.8 and not SDK v2.0.

Open Visual Studio and navigate to the KinectHandTracking directory and open KinectHandTracking.sln

Extensions.cs contains functions extending the Canvas class that are used for drawing. Helper functions for calculating points for drawing are included here as well.

MainWindow.xaml.cs contains functions that set up the Kinect color stream and prepares the Kinect for skeleton detection.

MainWindow.xaml contains the XML mark up for the window displaying the Kinect color stream video playback.

**See comments in the code that provide brief descriptions of how functions work

Useful Resources\n

Skeleton tracking: https://msdn.microsoft.com/en-us/library/JJ131025.aspx \n
Hand tracking (this is for the Kinect v2 Cam, but the explanations of the code are still useful for understanding how hand tracking works in general: http://pterneas.com/2014/03/21/kinect-for-windows-version-2-hand-tracking/
Coordinate mapping (this is for the Kinect v2 Cam, but the explanation is useful for understanding how coordinate mapping works and why it is important when drawing onto the Kinect color stream playback): http://pterneas.com/2014/05/06/understanding-kinect-coordinate-mapping/




