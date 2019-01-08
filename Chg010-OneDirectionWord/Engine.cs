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
        public static float WIDTH = 800;
        public static float HEIGHT = 800;
        public override void SetUp() {
            base.SetUp();
            Canvas((int)WIDTH, (int)HEIGHT);
            BackgroundColor(51);
            Translate(WIDTH / 2, HEIGHT / 2, 0);
            
        }
        public override void Draw() {
            base.Draw();
            float radius = 100;
            // First things firts
            // Draw a sphere
            DrawSphere(0, 0, 0, radius);
            RotateY(1f);
        }
    }
}
