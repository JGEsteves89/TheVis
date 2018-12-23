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

namespace Chg003_FireWorks {

    public class Engine : IEngine {
        public static int count;
        public static int width;
        public static int height;
        public class ParticleBase {
            public static float gravity = -0.1f;
            public Vector2 pos;
            public Vector2 vel;
            public Vector2 acc;
            public float size = 10;
            public Color clr;
            public List<Vector2> trail = new List<Vector2>();
            public int trailLen = 20;
            public ParticleBase(int size, Vector2 pos, Vector2 vel, Color clr) {
                this.pos = pos;
                this.vel = vel;
                this.clr = clr;
                this.size = size;
                this.acc = new Vector2(0f, gravity);
            }
            public virtual void Update() {
                trail.Add(pos);
                pos.X += vel.X;
                pos.Y += vel.Y;
                vel.X += acc.X;
                vel.Y += acc.Y;
            }
            public virtual void Render() {
                SetColor(clr);
                DrawRectangle((int)pos.X, (int)pos.Y, size, size);
                for (int i = trail.Count()-1; i >=0 ; i--) {
                    float newSize = Mapf(i, 0f, trailLen, 0f, size);
                    DrawRectangle(trail[i].X, trail[i].Y, newSize, newSize);
                }
                if (trail.Count() > trailLen) trail.RemoveAt(0);
            }
            public virtual bool Gone() {
                if (pos.X + size > width)
                    return true;
                if (pos.X + size < 0)
                    return true;
                if (pos.Y + size > height)
                    return true;
                if (pos.Y + size < 0)
                    return true;
                return false;
            }
        }
        public class Cracker : ParticleBase {
            public int life = 100;
            public float oSize;
            public Cracker(Vector2 pos, Vector2 vel, Color clr) : base(5, pos, vel, clr) {
                oSize = size;
            }
            public override void Update() {
                base.Update();
            }
            public override void Render() {
                life--;
                size = Mapf(life, 0f, 100f, 0f, oSize);
                if (size > 0) {
                    base.Render();
                }
                
            }
            public override bool Gone() {
                return base.Gone() || life == 0;
            }
        }
        public class FireCracker : ParticleBase {
            public static float expForce = 4;
            public bool exp = false;
            public List<Cracker> crackers = new List<Cracker>();
            public FireCracker() : base(
                                        10,
                                        new Vector2(Rndf(width), 0),
                                        new Vector2(Rndf(-0.5f, 0.5f), Rndf(10, 12)),
                                        Rndc()) {

            }
            public override void Update() {
                base.Update();
                if (!exp && vel.Y < 0.1) {
                    Explode();
                }
                foreach (Cracker item in crackers) {
                    item.Update();
                }
            }
            public override void Render() {
                base.Render();
                foreach (Cracker item in crackers) {
                    item.Render();
                }
            }
            public override bool Gone() {
                if (!base.Gone())
                    return false;

                foreach (Cracker item in crackers) {
                    if (!item.Gone())
                        return false;
                }
                return true;
            }
            public void Explode() {
                exp = true;
                int num = Rndi(50, 100);
                for (int i = 0; i < num; i++) {
                    crackers.Add(new Cracker(pos, Rndv2(Rndf(expForce)), clr));
                }
            }
        }
        
        public List<FireCracker> fireCrackers = new List<FireCracker>();
        public override void SetUp() {
            base.SetUp();
            width = 1200;
            height = 800;
            Canvas(width, height);
            BackgroundColor(51);
        }

        public override void Draw() {
            base.Draw();
            count++;
            int prob = 5;
            if (count > 500) {
                prob = 10;
                FireCracker.expForce = 6;
            }
            if (count > 1500) {
                prob = 20;
                FireCracker.expForce = 8;
            }
            if (Rndi(100) < prob) {
                fireCrackers.Add(new FireCracker());
            }
            for (int i = fireCrackers.Count() - 1; i >= 0; i--) {
                fireCrackers[i].Update();
                fireCrackers[i].Render();
                if (fireCrackers[i].Gone()) {
                    fireCrackers.RemoveAt(i);
                }
            }
        }
    }
}
