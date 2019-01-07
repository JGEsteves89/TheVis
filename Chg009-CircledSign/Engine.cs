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
using System.Drawing.Drawing2D;

namespace Chg009_CircledSign {

    public class Engine : IEngine {
        public class Circle {
            internal bool canGrow = true;
            internal Vector2 pos;
            internal float radius = 2;

            public Circle(Vector2 pos) {
                this.pos = pos;
            }

            internal void grow() {
                radius += 1;
            }

            internal void draw() {
                int green = Mapi((int)radius, 0, 15, 0, 200);
                SetFill(Color.FromArgb(100, green,100 ));
                DrawCircle(pos.X, pos.Y, radius);
            }
        }

        public static float WIDTH = 2400;
        public static float HEIGHT = 800;
        public List<Circle> circles = new List<Circle>();
        public List<Vector2> textPoints = new List<Vector2>();
        public override void SetUp() {
            base.SetUp();
            Canvas((int)WIDTH, (int)HEIGHT);
            BackgroundColor(51);

            //Create a image of white text on black background
            string text = "Tarina";
            int textSize = 300;
            Vector2 textPosition = new Vector2(100, HEIGHT-100);
            Vector2 imageSize = new Vector2(WIDTH, HEIGHT);
            Bitmap img = CreateBWTextImage(text, textSize, textPosition);


            // Retrieves every point that is on the white part of the image
            for (int x = 0; x < WIDTH; x++) {
                for (int y = 0; y < HEIGHT; y++) {
                    Color pix = img.GetPixel(x, y);
                    if (pix.R == 255 && pix.G == 255 && pix.B == 255) {
                        textPoints.Add(new Vector2(x, (int)HEIGHT - y));
                    }
                }
            }



        }

        public override void Draw() {
            base.Draw();
            int number = 30;
            int count = 0;

            while(count != number) {
                if ((textPoints.Count == 0)) break;
                // Generates a random point in the image
                Vector2 newPoint = textPoints[Rndi(textPoints.Count)];

                textPoints.Remove(newPoint);

                // Point must
                //      Be on the white part of the image
                //      It cannot be inside of another circle
                if (!insideCircle(circles, newPoint)) {
                    Circle newCircle = new Circle(newPoint);
                    if (!isTouchingOtherCircle(circles, newCircle)) {
                        // Create a circle on the selected point 
                        circles.Add(newCircle);
                        count++;
                    }
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
                circle.draw();
            }
        }

        private bool insideCircle(List<Circle> circles, Vector2 point) {
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
                    g.DrawString(text, f, Brushes.White, new Point((int)textPosition.X, (int)HEIGHT-(int)textPosition.Y));
                }
            }
            return bmp;
        }
    }


}
