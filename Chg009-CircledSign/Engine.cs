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
                SetFill(Color.White);
                DrawCircle(pos.X, pos.Y, radius);
            }
        }

        public static float WIDTH = 800;
        public static float HEIGHT = 800;
        public List<Circle> circles = new List<Circle>();
        public List<Vector2> textPoints = new List<Vector2>();
        public override void SetUp() {
            base.SetUp();
            Canvas((int)WIDTH, (int)HEIGHT);
            BackgroundColor(51);

            //Create a image of white text on black background
            string text = "Tarina";
            int textSize = 150;
            Vector2 textPosition = new Vector2(100, 100);
            Vector2 imageSize = new Vector2(WIDTH, HEIGHT);
            Bitmap img = CreateBWTextImage(text, textSize, textPosition, imageSize);


            // Retrieves every point that is on the white part of the image
            for (int x = 0; x < WIDTH; x++) {
                for (int y = 0; y < HEIGHT; y++) {
                    if (img.GetPixel(x, y) == Color.White) {
                        textPoints.Add(new Vector2(x, y));
                    }
                }
            }



        }

        public override void Draw() {
            base.Draw();

            if (textPoints.Count != 0) {
                // Generates a random point in the image
                Vector2 newPoint = textPoints[Rndi(textPoints.Count)];

                // Point must
                //      Be on the white part of the image
                //      It cannot be inside of another circle
                if (!insideCircle(circles, newPoint)) {
                    // Create a circle on the selected point 
                    circles.Add(new Circle(newPoint));
                }

                textPoints.Remove(newPoint);
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
                if ((item.pos - circle.pos).Length <= item.radius + circle.radius)
                    return true;
            }
            return false;
        }

        private Bitmap CreateBWTextImage(string text, int textSize, Vector2 textPosition) {
            Bitmap bmp = new Bitmap((int)WIDTH, (int)HEIGHT);
            GraphicsPath gp = new GraphicsPath();
            using (Graphics g = Graphics.FromImage(bmp)) {
                using (Font f = new Font("Arial", textSize)) {
                    gp.AddString(text, f.FontFamily, 0, textSize, new Point((int)textPosition.X, (int)textPosition.Y), StringFormat.GenericDefault);
                }
            }
            return bmp;
        }
    }


}
