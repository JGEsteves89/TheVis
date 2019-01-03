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

namespace Chg006_SunflowerNucleus {

    public class Engine : IEngine {
        public static float spacing = -1f;
        public static int layers = 20;
        public static float size = 10f;
        public static float fase = 1f;
        public static float height = 800;
        public static float width = 800;
        public static List<Color> colors = new List<Color>();
        public override void SetUp() {
            base.SetUp();
            Canvas((int)width, (int)height);
            BackgroundColor(51);
            for (int i = 0; i < layers; i++) {
                colors.Add(Rndc());
            }
        }
        public override void Draw() {
            base.Draw();
            for (int i = 0; i < layers; i++) {
                SetFill(colors[i]);
                float radius = spacing * (i + 1) + size * 2f * i;
                int count = (int)(radius * (float)Math.PI / size);
                float deli = Mapf(i, 0, layers, 1f, 1.1f);
                float variance = 0.1f * (float)Math.Cos(fase/ deli);

                for (int j = 0; j < count; j++) {
                    float rad = (float)j * 2f * ((float)Math.PI / (float)count);
                    float dx = width / 2 + radius * (float)Math.Cos(variance + rad);
                    float dy = height / 2 + radius * (float)Math.Sin(variance + rad);
                    DrawCircle(dx, dy, size);
                }
            }
            fase += 0.1f;


            //for (int i = 0; i < nLayers; i++) {
            //    int count = (int)((i+1f) * factor);
            //    for (int j = 0; j < count; j++) {

            //    }
            //}


        }
    }
}
