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
         * Tracks a hand by marking it with a light blue ellipse.
         */
        public static void DrawTrackedHands(this Canvas canvas, Joint hand, CoordinateMapper mapper)
        {
            if (hand.TrackingState == JointTrackingState.NotTracked) return;

            Point point = new Point();

            // Use CoordinateMapper to map the point of the hand's position to the appropriate point in the color stream
            // Need to do this because resolution varies between skeleton stream and color stream
            ColorImagePoint colorPoint = mapper.MapSkeletonPointToColorPoint(hand.Position, ColorImageFormat.RgbResolution1280x960Fps12);
            point.X = colorPoint.X;
            point.Y = colorPoint.Y;

            Ellipse ellipse = new Ellipse
            {
                Width = 50,
                Height = 50,
                Stroke = new SolidColorBrush(Colors.LightBlue),
                StrokeThickness = 4
            };

            Canvas.SetLeft(ellipse, point.X - ellipse.Width / 2);
            Canvas.SetTop(ellipse, point.Y - ellipse.Height / 2);

            canvas.Children.Add(ellipse);
        }

        /*
         * Draws ants from right hand to the right elbow and from the right elbow to the right shoulder
         */
        public static void DrawAntsAlongArms(this Canvas canvas, Skeleton skeleton, CoordinateMapper mapper)
        {
            if (skeleton == null) return;

            canvas.DrawAnts(skeleton.Joints[JointType.HandRight], skeleton.Joints[JointType.ElbowRight], mapper);
            canvas.DrawAnts(skeleton.Joints[JointType.ElbowRight], skeleton.Joints[JointType.ShoulderRight], mapper);
            
        }

        /*
         * Calculates point between two given points that is a given distance away from the starting point 
         */
        public static Point GetPointBetweenPoints(Point startPoint, Point endPoint, int distance)
        {
            double x0 = startPoint.X;
            double y0 = startPoint.Y;
            double x1 = endPoint.X;
            double y1 = endPoint.Y;

            double dx = x1 - x0;
            double dy = y1 - y0;

            // Get length/distance between the two points
            double length = Math.Sqrt(Math.Pow(dx, 2.0) + Math.Pow(dy, 2.0));

            // Calculate X and Y coordinates of new point
            double newX = x0 + dx / length * distance;
            double newY = y0 + dy / length * distance;

            Point newPoint = new Point();
            newPoint.X = newX;
            newPoint.Y = newY;

            return newPoint;

        }

        /*
         * Calculate the midpoint between two points using the midpoint formula
         */
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

        /*
         * Draw the legs of an ant using a given point as the starting point for the legs 
         */
        public static void DrawAntLegs(this Canvas canvas, Point startPoint)
        {
            double x0 = startPoint.X;
            double y0 = startPoint.Y;

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

        /*
         * Draw the arms of an ant using a given point as the starting point for the arms
         */
        public static void DrawAntArms(this Canvas canvas, Point startPoint)
        {
            double x0 = startPoint.X;
            double y0 = startPoint.Y;

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

        /*
         * Draw ants between two given points (path of an arm). Draws 6 ants between the two point points
         * Calculates an array of points between the two joint points that are a set distance apart (20)
         * Use those points as the starting points and ending points for bodies of ants
         */
        public static void DrawAnts(this Canvas canvas, Joint jointFrom, Joint jointTo, CoordinateMapper mapper)
        {
            if (jointFrom.TrackingState == JointTrackingState.NotTracked || jointTo.TrackingState == JointTrackingState.NotTracked) return;
            
            Point fromPoint = new Point();
            Point toPoint = new Point();

            // Use CoordinateMapper to map the point of the joint's position to the appropriate point in the color stream
            // Need to do this because resolution varies between skeleton stream and color stream
            ColorImagePoint colorPointFrom = mapper.MapSkeletonPointToColorPoint(jointFrom.Position, ColorImageFormat.RgbResolution1280x960Fps12);
            fromPoint.X = colorPointFrom.X;
            fromPoint.Y = colorPointFrom.Y;

            // Use CoordinateMapper to map the point of the joint's position to the appropriate point in the color stream
            // Need to do this because resolution varies between skeleton stream and color stream
            ColorImagePoint colorPointTo = mapper.MapSkeletonPointToColorPoint(jointTo.Position, ColorImageFormat.RgbResolution1280x960Fps12);
            toPoint.X = colorPointTo.X;
            toPoint.Y = colorPointTo.Y;

            Point endBody0 = GetPointBetweenPoints(fromPoint, toPoint, 10);

            // Drawing code is wrapped in try catch blocks to prevent a crash due to occasional null values when
            // when tracking joint position points

            try
            {
                Line body0 = new Line()
                {
                    X1 = fromPoint.X,
                    Y1 = fromPoint.Y,
                    X2 = endBody0.X,
                    Y2 = endBody0.Y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };

                // Use start of body as starting point for legs
                canvas.DrawAntLegs(fromPoint);
                // Use midpoint of body for starting point for arms
                canvas.DrawAntArms(GetMidpointBetweenPoints(fromPoint, endBody0));

                Ellipse head0 = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Black)
                };

                Canvas.SetLeft(head0, endBody0.X - head0.Width / 2);
                Canvas.SetTop(head0, endBody0.Y - head0.Height / 2);

                canvas.Children.Add(body0);
                canvas.Children.Add(head0);
            }
            catch(System.ArgumentException) { }

            Point startBody1 = GetPointBetweenPoints(endBody0, toPoint, 20);
            Point endBody1 = GetPointBetweenPoints(startBody1, toPoint, 10);

            try
            {
                Line body1 = new Line()
                {
                    X1 = startBody1.X,
                    Y1 = startBody1.Y,
                    X2 = endBody1.X,
                    Y2 = endBody1.Y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };

                // Use start of body as starting point for legs
                canvas.DrawAntLegs(startBody1);
                // Use midpoint of body for starting point for arms
                canvas.DrawAntArms(GetMidpointBetweenPoints(startBody1, endBody1));

                Ellipse head1 = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Black)
                };

                Canvas.SetLeft(head1, endBody1.X - head1.Width / 2);
                Canvas.SetTop(head1, endBody1.Y - head1.Height / 2);

                canvas.Children.Add(body1);
                canvas.Children.Add(head1);
            }
            catch (System.ArgumentException) { }

            Point startBody2 = GetPointBetweenPoints(endBody1, toPoint, 20);
            Point endBody2 = GetPointBetweenPoints(startBody2, toPoint, 10);

            try
            {
                Line body2 = new Line()
                {
                    X1 = startBody2.X,
                    Y1 = startBody2.Y,
                    X2 = endBody2.X,
                    Y2 = endBody2.Y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };

                // Use start of body as starting point for legs
                canvas.DrawAntLegs(startBody2);
                // Use midpoint of body for starting point for arms
                canvas.DrawAntArms(GetMidpointBetweenPoints(startBody2, endBody2));

                Ellipse head2 = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Black)
                };

                Canvas.SetLeft(head2, endBody2.X - head2.Width / 2);
                Canvas.SetTop(head2, endBody2.Y - head2.Height / 2);

                canvas.Children.Add(body2);
                canvas.Children.Add(head2);
            }
            catch (System.ArgumentException) { }

            Point startBody3 = GetPointBetweenPoints(endBody2, toPoint, 20);
            Point endBody3 = GetPointBetweenPoints(startBody3, toPoint, 10);

            try
            {
                Line body3 = new Line()
                {
                    X1 = startBody3.X,
                    Y1 = startBody3.Y,
                    X2 = endBody3.X,
                    Y2 = endBody3.Y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };

                // Use start of body as starting point for legs
                canvas.DrawAntLegs(startBody3);
                // Use midpoint of body for starting point for arms
                canvas.DrawAntArms(GetMidpointBetweenPoints(startBody3, endBody3));

                Ellipse head3 = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Black)
                };

                Canvas.SetLeft(head3, endBody3.X - head3.Width / 2);
                Canvas.SetTop(head3, endBody3.Y - head3.Height / 2);

                canvas.Children.Add(body3);
                canvas.Children.Add(head3);
            }
            catch (System.ArgumentException) { }


            Point startBody4 = GetPointBetweenPoints(endBody3, toPoint, 20);
            Point endBody4 = GetPointBetweenPoints(startBody4, toPoint, 10);

            try
            {
                Line body4 = new Line()
                {
                    X1 = startBody4.X,
                    Y1 = startBody4.Y,
                    X2 = endBody4.X,
                    Y2 = endBody4.Y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };

                // Use start of body as starting point for legs
                canvas.DrawAntLegs(startBody4);
                // Use midpoint of body for starting point for arms
                canvas.DrawAntArms(GetMidpointBetweenPoints(startBody4, endBody4));

                Ellipse head4 = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Black)
                };

                Canvas.SetLeft(head4, endBody4.X - head4.Width / 2);
                Canvas.SetTop(head4, endBody4.Y - head4.Height / 2);

                canvas.Children.Add(body4);
                canvas.Children.Add(head4);
            }
            catch (System.ArgumentException) { }
            

            Point startBody5 = GetPointBetweenPoints(endBody4, toPoint, 20);
            Point endBody5 = GetPointBetweenPoints(startBody5, toPoint, 10);

            try
            {
                Line body5 = new Line()
                {
                    X1 = startBody5.X,
                    Y1 = startBody5.Y,
                    X2 = endBody5.X,
                    Y2 = endBody5.Y,
                    Stroke = new SolidColorBrush(Colors.Black),
                    StrokeThickness = 2
                };

                // Use start of body as starting point for legs
                canvas.DrawAntLegs(startBody5);
                // Use midpoint of body for starting point for arms
                canvas.DrawAntArms(GetMidpointBetweenPoints(startBody5, endBody5));

                Ellipse head5 = new Ellipse()
                {
                    Width = 10,
                    Height = 10,
                    Fill = new SolidColorBrush(Colors.Black)
                };

                Canvas.SetLeft(head5, endBody5.X - head5.Width / 2);
                Canvas.SetTop(head5, endBody5.Y - head5.Height / 2);

                canvas.Children.Add(body5);
                canvas.Children.Add(head5);
            }
            catch (System.ArgumentException) { }            
            
        }


        
    }
}
