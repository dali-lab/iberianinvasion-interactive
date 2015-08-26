using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KinectHandTracking
{
    public static class Extensions
    {
        /*
        public static Point Scale(this Joint joint, CoordinateMapper mapper)
        {
            Point point = new Point();

            ColorImagePoint colorPoint = mapper.MapSkeletonPointToColorPoint(joint.Position, ColorImageFormat.RgbResolution640x480Fps30);
            point.X *= float.IsInfinity(colorPoint.X) ? 0.0 : colorPoint.X;
            point.Y *= float.IsInfinity(colorPoint.Y) ? 0.0 : colorPoint.Y;
            
            return point;
        }
        */
        
        public static void DrawTrackedHands(this Canvas canvas, Joint hand, CoordinateMapper mapper)
        {
            if (hand.TrackingState == JointTrackingState.NotTracked) return;

            Point point = new Point();

            ColorImagePoint colorPoint = mapper.MapSkeletonPointToColorPoint(hand.Position, ColorImageFormat.RgbResolution640x480Fps30);
            point.X = colorPoint.X;
            point.Y = colorPoint.Y;

            Ellipse ellipse = new Ellipse
            {
                Width = 80,
                Height = 80,
                Stroke = new SolidColorBrush(Colors.LightBlue),
                StrokeThickness = 4
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        public static void DrawAntsAlongArms(this Canvas canvas, Skeleton skeleton, CoordinateMapper mapper)
        {
            if (skeleton == null) return;

            canvas.DrawAnts(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.ElbowRight], mapper);
            canvas.DrawAnts(skeleton.Joints[JointType.ElbowRight], skeleton.Joints[JointType.ShoulderRight], mapper);
            
        }

        public static Point GetPointBetweenPoints(Point startPoint, Point endPoint, int distance)
        {
            double x0 = startPoint.X;
            double y0 = startPoint.Y;
            double x1 = endPoint.X;
            double y1 = endPoint.Y;

            double dx = x1 - x0;
            double dy = y1 - y0;

            double length = Math.Sqrt(Math.Pow(dx, 2.0) + Math.Pow(dy, 2.0));

            double newX = x0 + dx / length * distance;
            double newY = y0 + dy / length * distance;

            Point newPoint = new Point();
            newPoint.X = newX;
            newPoint.Y = newY;

            return newPoint;

        }

        public static Point GetMidpointBetweenPoints(Point startPoint, Point endPoint)
        {
            double x0 = startPoint.X;
            double y0 = startPoint.Y;
            double x1 = endPoint.X;
            double y1 = endPoint.Y;

            double newX = (x0 + x1) / 2;
            double newY = (y0 + y1) / 2;

            Point newPoint = new Point();
            newPoint.X = newX;
            newPoint.Y = newY;

            return newPoint;
        }

        public static void DrawAntLegs(this Canvas canvas, Point startPoint)
        {
            double x0 = startPoint.X;
            double y0 = startPoint.Y;

            /*
            double x1 = x0 + length * Math.Cos(armAngle);
            double y1 = y0 + length * Math.Sin(armAngle);
            */

            Line leg1 = new Line()
            {
                X1 = x0,
                Y1 = y0,
                X2 = x0 - 10,
                Y2 = y0 + 10,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line leg1bot = new Line()
            {
                X1 = x0 - 10,
                Y1 = y0 + 10,
                X2 = x0 - 5,
                Y2 = y0 + 20,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line leg2 = new Line()
            {
                X1 = x0,
                Y1 = y0,
                X2 = x0 + 10,
                Y2 = y0 + 10,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line leg2bot = new Line()
            {
                X1 = x0 + 10,
                Y1 = y0 + 10,
                X2 = x0 + 5,
                Y2 = y0 + 20,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            canvas.Children.Add(leg1);
            canvas.Children.Add(leg1bot);
            canvas.Children.Add(leg2);
            canvas.Children.Add(leg2bot);
        }

        public static void DrawAntArms(this Canvas canvas, Point startPoint)
        {
            double x0 = startPoint.X;
            double y0 = startPoint.Y;

            /*
            double x1 = x0 + length * Math.Cos(armAngle);
            double y1 = y0 + length * Math.Sin(armAngle);
            */

            Line arm1 = new Line()
            {
                X1 = x0,
                Y1 = y0,
                X2 = x0 - 10,
                Y2 = y0 - 10,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line arm1top = new Line()
            {
                X1 = x0 - 10,
                Y1 = y0 - 10,
                X2 = x0 - 5,
                Y2 = y0 - 20,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line arm2 = new Line()
            {
                X1 = x0,
                Y1 = y0,
                X2 = x0 + 10,
                Y2 = y0 - 10,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line arm2top = new Line()
            {
                X1 = x0 + 10,
                Y1 = y0 - 10,
                X2 = x0 + 5,
                Y2 = y0 - 20,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            canvas.Children.Add(arm1);
            canvas.Children.Add(arm1top);
            canvas.Children.Add(arm2);
            canvas.Children.Add(arm2top);
        }

        public static void DrawAnts(this Canvas canvas, Joint jointFrom, Joint jointTo, CoordinateMapper mapper)
        {
            if (jointFrom.TrackingState == JointTrackingState.NotTracked || jointTo.TrackingState == JointTrackingState.NotTracked) return;
            
            Point fromPoint = new Point();
            Point toPoint = new Point();

            ColorImagePoint colorPointFrom = mapper.MapSkeletonPointToColorPoint(jointFrom.Position, ColorImageFormat.RgbResolution640x480Fps30);
            fromPoint.X = colorPointFrom.X;
            fromPoint.Y = colorPointFrom.Y;

            ColorImagePoint colorPointTo = mapper.MapSkeletonPointToColorPoint(jointTo.Position, ColorImageFormat.RgbResolution640x480Fps30);
            toPoint.X = colorPointTo.X;
            toPoint.Y = colorPointTo.Y;

            Point endBody0 = GetPointBetweenPoints(fromPoint, toPoint, 10);

            Line body0 = new Line()
            {
                X1 = fromPoint.X,
                Y1 = fromPoint.Y,
                X2 = endBody0.X,
                Y2 = endBody0.Y,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            canvas.DrawAntLegs(fromPoint);
            canvas.DrawAntArms(GetMidpointBetweenPoints(fromPoint, endBody0));

            Ellipse head0 = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(head0, endBody0.X - head0.Width / 2);
            Canvas.SetTop(head0, endBody0.Y - head0.Height / 2);

            Point startBody1 = GetPointBetweenPoints(endBody0, toPoint, 20);
            Point endBody1 = GetPointBetweenPoints(startBody1, toPoint, 10);

            Line body1 = new Line()
            {
                X1 = startBody1.X,
                Y1 = startBody1.Y,
                X2 = endBody1.X,
                Y2 = endBody1.Y,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            canvas.DrawAntLegs(startBody1);
            canvas.DrawAntArms(GetMidpointBetweenPoints(startBody1, endBody1));

            Ellipse head1 = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(head1, endBody1.X - head1.Width / 2);
            Canvas.SetTop(head1, endBody1.Y - head1.Height / 2);

            Point startBody2 = GetPointBetweenPoints(endBody1, toPoint, 20);
            Point endBody2 = GetPointBetweenPoints(startBody2, toPoint, 10);

            Line body2 = new Line()
            {
                X1 = startBody2.X,
                Y1 = startBody2.Y,
                X2 = endBody2.X,
                Y2 = endBody2.Y,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            canvas.DrawAntLegs(startBody2);
            canvas.DrawAntArms(GetMidpointBetweenPoints(startBody2, endBody2));

            Ellipse head2 = new Ellipse()
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(head2, endBody2.X - head2.Width / 2);
            Canvas.SetTop(head2, endBody2.Y - head2.Height / 2);

            canvas.Children.Add(body0);
            canvas.Children.Add(head0);
            canvas.Children.Add(body1);
            canvas.Children.Add(head1);
            canvas.Children.Add(body2);
            canvas.Children.Add(head2);
            
        }

        /*
        public static void DrawAnt(this Canvas canvas, Joint joint, CoordinateMapper mapper)
        {
            if (joint.TrackingState == JointTrackingState.NotTracked) return;

            Point point = new Point();

            ColorImagePoint colorPoint = mapper.MapSkeletonPointToColorPoint(joint.Position, ColorImageFormat.RgbResolution640x480Fps30);
            point.X = colorPoint.X;
            point.Y = colorPoint.Y;

            Ellipse ellipse = new Ellipse
            {
                Width = 10,
                Height = 10,
                Fill = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            Line body = new Line()
            {
                X1 = point.X,
                Y1 = point.Y,
                X2 = point.X,
                Y2 = point.Y + 20,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };
            
            Line leftArmBottom = new Line()
            {
                X1 = point.X,
                Y1 = point.Y + 10,
                X2 = point.X - 10,
                Y2 = point.Y - 5,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line leftArmTop = new Line()
            {
                X1 = point.X - 10,
                Y1 = point.Y - 5,
                X2 = point.X + 10,
                Y2 = point.Y - 10,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line rightArmBottom = new Line()
            {
                X1 = point.X,
                Y1 = point.Y + 10,
                X2 = point.X + 10,
                Y2 = point.Y - 5,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line rightArmTop = new Line()
            {
                X1 = point.X + 10,
                Y1 = point.Y - 5,
                X2 = point.X - 10,
                Y2 = point.Y - 10,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line leftLegTop = new Line()
            {
                X1 = point.X,
                Y1 = point.Y + 20,
                X2 = point.X - 10,
                Y2 = point.Y + 25,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line leftLegBottom = new Line()
            {
                X1 = point.X - 10,
                Y1 = point.Y + 25,
                X2 = point.X + 10,
                Y2 = point.Y + 30,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line rightLegTop = new Line()
            {
                X1 = point.X,
                Y1 = point.Y + 20,
                X2 = point.X + 10,
                Y2 = point.Y + 25,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            Line rightLegBottom = new Line()
            {
                X1 = point.X + 10,
                Y1 = point.Y + 25,
                X2 = point.X - 10,
                Y2 = point.Y + 30,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 2
            };

            canvas.Children.Add(ellipse);
            canvas.Children.Add(body);
            canvas.Children.Add(leftArmBottom);
            canvas.Children.Add(leftArmTop);
            canvas.Children.Add(rightArmBottom);
            canvas.Children.Add(rightArmTop);
            canvas.Children.Add(leftLegTop);
            canvas.Children.Add(leftLegBottom);
            canvas.Children.Add(rightLegTop);
            canvas.Children.Add(rightLegBottom);
             

            canvas.Children.Add(ellipse);
            canvas.Children.Add(body);
        }
        */

        #region Camera

        /*
        public static ImageSource ToBitmap(this ColorImageFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            byte[] pixels = new byte[width * height * ((format.BitsPerPixel + 7) / 8)];

            if (frame.RawColorImageFormat == ColorImageFormat.Bgra)
            {
                frame.CopyRawFrameDataToArray(pixels);
            }
            else
            {
                frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra);
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        public static ImageSource ToBitmap(this DepthImageFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort minDepth = frame.DepthMinReliableDistance;
            ushort maxDepth = frame.DepthMaxReliableDistance;

            ushort[] pixelData = new ushort[width * height];
            byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(pixelData);

            int colorIndex = 0;
            for (int depthIndex = 0; depthIndex < pixelData.Length; ++depthIndex)
            {
                ushort depth = pixelData[depthIndex];

                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                pixels[colorIndex++] = intensity; // Blue
                pixels[colorIndex++] = intensity; // Green
                pixels[colorIndex++] = intensity; // Red

                ++colorIndex;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }

        public static ImageSource ToBitmap(this InfraredImageFrame frame)
        {
            int width = frame.FrameDescription.Width;
            int height = frame.FrameDescription.Height;
            PixelFormat format = PixelFormats.Bgr32;

            ushort[] frameData = new ushort[width * height];
            byte[] pixels = new byte[width * height * (format.BitsPerPixel + 7) / 8];

            frame.CopyFrameDataToArray(frameData);

            int colorIndex = 0;
            for (int infraredIndex = 0; infraredIndex < frameData.Length; infraredIndex++)
            {
                ushort ir = frameData[infraredIndex];

                byte intensity = (byte)(ir >> 7);

                pixels[colorIndex++] = (byte)(intensity / 1); // Blue
                pixels[colorIndex++] = (byte)(intensity / 1); // Green   
                pixels[colorIndex++] = (byte)(intensity / 0.4); // Red

                colorIndex++;
            }

            int stride = width * format.BitsPerPixel / 8;

            return BitmapSource.Create(width, height, 96, 96, format, null, pixels, stride);
        }
        */
        #endregion

        /*

        #region Body

        public static Point Scale(this Joint joint, CoordinateMapper mapper)
        {
            Point point = new Point();

            ColorSpacePoint colorPoint = mapper.MapCameraPointToColorSpace(joint.Position);
            point.X *= float.IsInfinity(colorPoint.X) ? 0.0 : colorPoint.X;
            point.Y *= float.IsInfinity(colorPoint.Y) ? 0.0 : colorPoint.Y;

            return point;
        }

        #endregion

        #region Drawing

        public static void DrawSkeleton(this Canvas canvas, Body body, CoordinateMapper mapper)
        {
            if (body == null) return;

            foreach (Joint joint in body.Joints.Values)
            {
                canvas.DrawPoint(joint, mapper);
            }

            canvas.DrawLine(body.Joints[JointType.Head], body.Joints[JointType.Neck], mapper);
            canvas.DrawLine(body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder], mapper);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight], mapper);
            canvas.DrawLine(body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid], mapper);
            canvas.DrawLine(body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight], mapper);
            canvas.DrawLine(body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight], mapper);
            canvas.DrawLine(body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.WristRight], body.Joints[JointType.HandRight], mapper);
            canvas.DrawLine(body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight], mapper);
            canvas.DrawLine(body.Joints[JointType.HandTipLeft], body.Joints[JointType.ThumbLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.HandTipRight], body.Joints[JointType.ThumbRight], mapper);
            canvas.DrawLine(body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase], mapper);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight], mapper);
            canvas.DrawLine(body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight], mapper);
            canvas.DrawLine(body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight], mapper);
            canvas.DrawLine(body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft], mapper);
            canvas.DrawLine(body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight], mapper);
        }

        public static void DrawPoint(this Canvas canvas, Joint joint, CoordinateMapper mapper)
        {
            if (joint.TrackingState == TrackingState.NotTracked) return;

            Point point = joint.Scale(mapper);

            Ellipse ellipse = new Ellipse
            {
                Width = 20,
                Height = 20,
                Fill = new SolidColorBrush(Colors.LightBlue)
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        public static void DrawHand(this Canvas canvas, Joint hand, CoordinateMapper mapper)
        {
            if (hand.TrackingState == TrackingState.NotTracked) return;

            Point point = hand.Scale(mapper);

            Ellipse ellipse = new Ellipse
            {
                Width = 100,
                Height = 100,
                Stroke = new SolidColorBrush(Colors.LightBlue),
                StrokeThickness = 4
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        public static void DrawThumb(this Canvas canvas, Joint thumb, CoordinateMapper mapper)
        {
            if (thumb.TrackingState == TrackingState.NotTracked) return;

            Point point = thumb.Scale(mapper);

            Ellipse ellipse = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = new SolidColorBrush(Colors.LightBlue),
                Opacity = 0.7
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        public static void DrawLine(this Canvas canvas, Joint first, Joint second, CoordinateMapper mapper)
        {
            if (first.TrackingState == TrackingState.NotTracked || second.TrackingState == TrackingState.NotTracked) return;

            Point firstPoint = first.Scale(mapper);
            Point secondPoint = second.Scale(mapper);

            Line line = new Line
            {
                X1 = firstPoint.X,
                Y1 = firstPoint.Y,
                X2 = secondPoint.X,
                Y2 = secondPoint.Y,
                StrokeThickness = 8,
                Stroke = new SolidColorBrush(Colors.LightBlue)
            };

            canvas.Children.Add(line);
        }
        */

       // #endregion

        
    }
}
