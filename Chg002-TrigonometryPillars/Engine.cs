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
        double osc = 0;
        public override void SetUp() {
            base.SetUp();
            Canvas(800, 800);
            BackgroundColor(51);
            RotateY((float)Math.PI);
            RotateX((float)Math.PI);
            Translate(200, 200, -50);

        }
        public override void Draw() {
            base.Draw();

            //int variance = 25 + (int)(25 * Math.Cos(osc));
            //DrawPrism(0, 0, 0, 50, 50 + variance, 50);
            osc += 0.05;
        }
    }
}
