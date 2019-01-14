using Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Windows.Forms;
using OpenTK.Input;

namespace Chg010_OneDirectionWord {

    public class Engine : IEngine {
        public static float WIDTH = 2400;
        public static float HEIGHT = 800;
        public static float RADIUS = 20;

        public class Circle {
            internal bool canGrow = true;
            internal Vector3 pos;
            internal float radius = 2;

            public Circle(Vector3 pos) {
                this.pos = pos;
            }

            internal void grow() {
                radius += 2f;
            }
        }

        public List<Circle> balls = new List<Circle>();
        public List<Color> colors = new List<Color>();

        public override void SetUp() {
            base.SetUp();
            Canvas((int)WIDTH, (int)HEIGHT);
            BackgroundColor(51);
            Translate(WIDTH / 2, HEIGHT / 2, 0);

            //Create a image of white text on black background
            string text = "Tarina";
            int textSize = 300;
            Vector2 textPosition = new Vector2(100, HEIGHT - 100);
            Vector2 imageSize = new Vector2(WIDTH, HEIGHT);
            Bitmap img = CreateBWTextImage(text, textSize, textPosition);

            List<Vector2> textPoints = new List<Vector2>();
            // Retrieves every point that is on the white part of the image
            for (int x = 0; x < WIDTH; x++) {
                for (int y = 0; y < HEIGHT; y++) {
                    Color pix = img.GetPixel(x, y);
                    if (pix.R == 255 && pix.G == 255 && pix.B == 255) {
                        textPoints.Add(new Vector2(x, (int)HEIGHT - y));
                    }
                }
            }

            List<Circle> circles = calculateCircles(textPoints);
            Console.WriteLine(circles.Count);
            for (int i = 0; i < circles.Count; i++) {
                circles[i].pos.X += -1100;
                circles[i].pos.Y += -300;
                circles[i].pos.Z = 50f * RADIUS / circles[i].radius;
                colors.Add(Rndc());
            }
            balls = circles;

            RotateY(-45f);
        }
        public override void Draw() {
            base.Draw();

            for (int i = 0; i < balls.Count; i++) {
                SetColor(colors[i]);
                DrawSphere(balls[i].pos.X, balls[i].pos.Y, balls[i].pos.Z, RADIUS);
            }

            RotateY(1f);
        }

        public List<Circle> calculateCircles(List<Vector2> textPoints) {
            List<Circle> circles = new List<Circle>();
            int tries = 100;
            int atSameTime = 10;
            while (true) {

                for (int j = 0; j < atSameTime; j++) {
                    for (int i = 0; i < tries + 1; i++) {
                        // Generates a random point in the image
                        Vector2 newPoint = textPoints[Rndi(textPoints.Count)];
                        textPoints.Remove(newPoint);

                        // Point must
                        //      Be on the white part of the image
                        //      It cannot be inside of another circle
                        if (!insideCircle(circles, new Vector3(newPoint.X, newPoint.Y, 0))) {
                            Circle newCircle = new Circle(new Vector3(newPoint.X, newPoint.Y, 0));
                            if (!isTouchingOtherCircle(circles, newCircle)) {
                                // Create a circle on the selected point 
                                circles.Add(newCircle);
                                break;
                            }
                        }
                        if (i == tries) return circles;
                    }
                }
                foreach (Circle circle in circles) {
                    // If can grow, Grows
                    if (circle.canGrow) {
                        // If circle touches other circles, it can grow no more
                        if (isTouchingOtherCircle(circles, circle))
                            circle.canGrow = false;
                        else
                            circle.grow();
                    }
                }
            }
        }

        private bool insideCircle(List<Circle> circles, Vector3 point) {
            foreach (Circle circle in circles) {
                if ((circle.pos - point).Length <= circle.radius)
                    return true;
            }
            return false;
        }

        private bool isTouchingOtherCircle(List<Circle> circles, Circle circle) {
            foreach (Circle item in circles) {
                if (item == circle) continue;
                if ((item.pos - circle.pos).Length <= item.radius + circle.radius + 2)
                    return true;
            }
            return false;
        }

        private Bitmap CreateBWTextImage(string text, int textSize, Vector2 textPosition) {
            Bitmap bmp = new Bitmap((int)WIDTH, (int)HEIGHT);
            using (Graphics g = Graphics.FromImage(bmp)) {
                using (Font f = new Font("Arial", textSize)) {
                    g.FillRectangle(Brushes.Black, new Rectangle(0, 0, (int)WIDTH, (int)HEIGHT));
                    g.DrawString(text, f, Brushes.White, new Point((int)textPosition.X, (int)HEIGHT - (int)textPosition.Y));
                }
            }
            return bmp;
        }
    }
}
