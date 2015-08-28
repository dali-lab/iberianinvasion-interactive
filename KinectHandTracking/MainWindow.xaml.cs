using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KinectHandTracking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members

        KinectSensor _sensor; // Kinect sensor object
        Skeleton[] _skeletonData; // Array to hold all skeletons detected by Kinect
        WriteableBitmap _colorBitmap;
        byte[] _colorPixels;

        #endregion

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Event handlers

        /*
         * Sets up the window, Kinect color stream (video playback) and Kinect skeleton detection
        */
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _sensor = KinectSensor.KinectSensors.FirstOrDefault(s => s.Status == KinectStatus.Connected);

            // Turn on the color stream to receive color frames
            _sensor.ColorStream.Enable(ColorImageFormat.RgbResolution1280x960Fps12);

            // Allocate space to put the pixels we'll receive
            _colorPixels = new byte[_sensor.ColorStream.FramePixelDataLength];

            // This is the bitmap we'll display on-screen
            _colorBitmap = new WriteableBitmap(_sensor.ColorStream.FrameWidth, _sensor.ColorStream.FrameHeight, 96.0, 96.0, PixelFormats.Bgr32, null);

            // Set the image we display to point to the bitmap where we'll put the image data
            this.Camera.Source = _colorBitmap;

            // Add an event handler to be called whenever there is new color frame data
            _sensor.ColorFrameReady += this.SensorColorFrameReady;

            // Enable skeleton detection
            _sensor.SkeletonStream.Enable();

            _skeletonData = new Skeleton[_sensor.SkeletonStream.FrameSkeletonArrayLength];

            // Add event handler to be called whenever a new skeleton frame is ready
            _sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);

            _sensor.Start();

        }

        /*
         * Callback function called whenever there is new color frame data
         */
        private void SensorColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (colorFrame != null)
                {
                    // Copy the pixel data from the image to a temporary array
                    colorFrame.CopyPixelDataTo(_colorPixels);

                    // Write the pixel data into our bitmap
                    _colorBitmap.WritePixels(
                        new Int32Rect(0, 0, _colorBitmap.PixelWidth, _colorBitmap.PixelHeight),
                        _colorPixels,
                        _colorBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

        /*
         * Callback function called whenever there is a new skeleton frame ready 
         */
        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame()) // Open the Skeleton frame
            {
                if (skeletonFrame != null && _skeletonData != null) // Check that a frame is available
                {
                    skeletonFrame.CopySkeletonDataTo(_skeletonData); // Get the skeletal information in this frame
                }
            }

            TrackNearestSkeleton();
        }

        /*
         * Track the skeleton nearest to the Kinect sensor
         */
        public void TrackNearestSkeleton()
        {
            canvas.Children.Clear();

            if (_sensor != null && _sensor.SkeletonStream != null)
            {
                if (!_sensor.SkeletonStream.AppChoosesSkeletons)
                {
                    _sensor.SkeletonStream.AppChoosesSkeletons = true; // Make sure to let the app choose the skeleton to track by setting this to true
                }

                float closestDistance = 10000f; // Large starting distance
                int closestID = 0;

                foreach (Skeleton skeleton in _skeletonData.Where(s => s.TrackingState != SkeletonTrackingState.NotTracked))
                {
                    // Find the skeleton that is closest to the Kinect sensor using the Z coordinate
                    if (skeleton.Position.Z < closestDistance) 
                    {
                        // Save the skeleton's tracking ID and distance from the Kinect sensor
                        closestID = skeleton.TrackingId;
                        closestDistance = skeleton.Position.Z;
                    }
                }

                if (closestID > 0)
                {
                    _sensor.SkeletonStream.ChooseSkeletons(closestID);
                    // Get the tracked skeleton using its tracking ID
                    Skeleton[] result = _skeletonData.Where(s => s.TrackingId == closestID).ToArray();

                    if (result.Length > 0)
                    {
                        Skeleton trackedSkeleton = result[0];
                        Joint rightHand = trackedSkeleton.Joints[JointType.HandRight];
                        Joint rightShoulder = trackedSkeleton.Joints[JointType.ShoulderRight];

                        // Uncomment this to use a marker to track the right hand
                        // canvas.DrawTrackedHands(rightHand, _sensor.CoordinateMapper);

                        canvas.DrawAntsAlongArms(trackedSkeleton, _sensor.CoordinateMapper);
                    }
                }
            }

        }


        private void Window_Closed(object sender, EventArgs e)
        {
            if (_sensor != null)
            {
                _sensor.Stop();
            }
        }

        #endregion
    }
}
