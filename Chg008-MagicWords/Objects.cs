using System;
using System.Collections.Generic;
using OpenTK;
using Engine;
using static Engine.IEngine;
using System.Drawing;


namespace Chg008_MagicWords {
    public class Particle {
        public float limitSpeed = 5;
        public Vector2 pos;
        public Vector2 vec;
        public Vector2 acc;
        public float radius = 4;
        public float dumpEffect = 0.2f;
        public Color color = Color.White;
        public long timeSpan = 0;
        public Vector2 target;
        public bool targetActive = false;
        
        public Particle(Vector2 pos, Vector2 vec, Vector2 acc) {
            this.pos = pos;
            this.vec = vec;
            this.acc = acc;
        }
        public Particle(Vector2 pos) : this(pos, new Vector2(0, 0), new Vector2(0, 0)) {
        }
        public Particle(Vector2 pos, Vector2 vec) : this(pos, vec, new Vector2(0, 0)) {
        }
        public virtual void Update() {
            if (targetActive) {
                Vector2 dir = (target - pos).Normalized();
                acc = dumpEffect * dir;
            }

            pos += vec;
            vec += acc;

            if (vec.Length> limitSpeed) 
                vec = vec.Normalized() * limitSpeed;
        }
        public virtual void Draw() {
            SetColor(color);
            timeSpan++;
            DrawCircle(pos.X, pos.Y, radius);
        }
    }
    public class Comet : Particle {
        List<Particle> tail = new List<Particle>();
        public int delay = 200;
        public int count = 0;
        public Comet(Vector2 pos) : base(pos) {

        }
        public override void Draw() {
            base.Draw();
            int maxTime = 200;
            foreach (Particle item in tail) {
                int color = Mapi((int)item.timeSpan, 0, maxTime, 0, 255);
                item.color = Color.FromArgb(255 - color, 255 - color, 255);
                item.Draw();
            }
        }
        public override void Update() {
            base.Update();

            if (Engine.TARGETS.Count != 0) {
                int number = (int)Math.Min(Engine.TARGETS.Count, Engine.OUTPUT_SIZE);
                for (int i = 0; i < number; i++) {
                    Vector2 newVel = -vec;
                    newVel.X += Rndf(-2f, 2f);
                    newVel.Y += Rndf(-2f, 2f);

                    Particle newPart = new Particle(pos, newVel, Engine.GRAVITY);
                    newPart.target = Engine.TARGETS[0];
                    newPart.targetActive = true;
                    Engine.TARGETS.RemoveAt(0);
                    tail.Add(newPart);
                }
            } else {
                target = new Vector2(2 * Engine.WIDTH, 2 * Engine.HEIGHT);
                if (count != delay) {
                    count++;
                } 
                //else {
                //    for (int i = tail.Count - 1; i >= 0; i--) {
                //        tail[i].targetActive = false;
                //        tail[i].acc = new Vector2(0, 0);

                //        Vector2 dir = (tail[i].target - tail[i].pos);
                //        float mag = (float)Math.Min(dir.Length, 5f);
                //        if (mag > 0.1f)
                //            tail[i].vec = mag * dir.Normalized();
                //        else {
                //            tail[i].vec = new Vector2(0, 0);
                //            tail[i].pos = tail[i].target;
                //        }

                //    }
                //}
            }
            for (int i = tail.Count - 1; i >= 0; i--) {
                float m = 1.73f;
                float r = 0.00158f;
                float x = tail[i].timeSpan*2;
                float exp = m * ((float)Math.Exp(r*x)-1f);
                tail[i].dumpEffect = Truncf(exp, 0, 50);
                if (tail[i].pos.X < -Engine.WIDTH) tail.RemoveAt(i);
                else if (tail[i].pos.X > 2 * Engine.WIDTH) tail.RemoveAt(i);
                else if (tail[i].pos.Y < -Engine.HEIGHT) tail.RemoveAt(i);
                else if (tail[i].pos.Y > 2 * Engine.HEIGHT) tail.RemoveAt(i);
                else tail[i].Update();
            }
        }
    }
}
