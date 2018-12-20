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

namespace Template {

    public class Engine : IEngine {
        
        public override void SetUp() {
            base.SetUp();
            Canvas(800, 800);
            BackgroundColor(51);
        }
        public override void Draw() {
            base.Draw();
        }
    }
}
