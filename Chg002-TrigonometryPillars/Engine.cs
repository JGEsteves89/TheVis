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

namespace Chg002_TrigonometryPillars {

    public class Engine : IEngine {
        float osc = 0;
        int cols = 15;
        int rows = 15;
        int size = 30;
        public override void SetUp() {
            base.SetUp();
            Canvas(800, 800);
            BackgroundColor(51);
            Translate(400, 200, 0);

            RotateX(-25f);
            RotateY(-45f);
            //
            //RotateX(45f);


        }
        public override void Draw() {
            base.Draw();
            for (int i = 0; i < cols; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    double dist = Math.Abs(rows / 2f - i) + Math.Abs(cols / 2f - j);
                    int variance = size / 2 + (int)(size/2 * Math.Cos(osc + dist*0.4));
                    DrawCube(i * size, 0, j * size, size, size + variance, size);
                    DrawCube(i * size, -(size + variance), j * size, size, size + variance, size);
                }

            }
            osc += 0.1f;
            //Console.WriteLine(osc);
        }
    }
}
