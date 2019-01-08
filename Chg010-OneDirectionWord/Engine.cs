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
        }
        public override void Draw() {
            base.Draw();
        }
    }
}
