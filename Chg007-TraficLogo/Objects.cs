using System;
using System.Collections.Generic;
using OpenTK;
using Engine;
using static Engine.IEngine;
using System.Drawing;

namespace Chg007_TraficLogo {
    public class Cross : Node{
        public float size = 10f;
        public Vector2 pos;
        public List<Road> roads = new List<Road>();
        public Cross(int index, Vector2 pos):base(index) {
            this.pos = pos;
        }

        public void Draw() {
            IEngine.SetColor(Color.FromArgb(125, Color.DarkGray));
            DrawCircle(pos.X, pos.Y, size);
        }
    }
    public class Road : Edge {
        public Cross start;
        public Cross end;
        public float size = 10f;
        public Road(Cross start, Cross end) : base(start.i, end.i,(end.pos-start.pos).LengthFast) {
            this.start = start;
            this.end = end;
            this.start.roads.Add(this);
            this.end.roads.Add(this);
        }
        public void Draw() {
            SetColor(Color.FromArgb(125, Color.DarkGray));
            Vector2 dir = end.pos - start.pos;
            float a = (float)Math.Atan2(dir.Y, dir.X);
            Vector2 su = new Vector2(
                start.pos.X + size / 2f * (float)Math.Cos(a + Math.PI / 2),
                start.pos.Y + size / 2f * (float)Math.Sin(a + Math.PI / 2)
                );
            Vector2 sd = new Vector2(
                start.pos.X - size / 2f * (float)Math.Cos(a + Math.PI / 2),
                start.pos.Y - size / 2f * (float)Math.Sin(a + Math.PI / 2)
                );
            Vector2 eu = new Vector2(
                end.pos.X + size / 2f * (float)Math.Cos(a + Math.PI / 2),
                end.pos.Y + size / 2f * (float)Math.Sin(a + Math.PI / 2)
                );
            Vector2 ed = new Vector2(
                end.pos.X - size / 2f * (float)Math.Cos(a + Math.PI / 2),
                end.pos.Y - size / 2f * (float)Math.Sin(a + Math.PI / 2)
                );
            DrawLine(su.X, su.Y, eu.X, eu.Y);
            DrawLine(sd.X, sd.Y, ed.X, ed.Y);
        }

        internal Vector2 getDir() {
            return (end.pos - start.pos).Normalized();
        }
    }
    public class Solution {
        public List<Cross> route = new List<Cross>();
        public float dist;
    }
    public class Car {
        public float radius = 3;
        public float pace = 20;
        public Cross target;
        public Cross source;
        public Vector2 pos;
        public List<Cross> route = new List<Cross>();
        public bool arrived = false;
        public int location = 0;

        public Car(Cross source, Cross target, Vector2 targetPos, List<Cross> route) {
            this.source = source;
            this.target = target;
            this.pos = source.pos;
            this.route.AddRange(route);
            this.route.Add(new Cross(0,targetPos));
        }
        public void Update() {
            if (arrived) return;
            pos = nextPos();
        }

        internal Vector2 nextPos() {
            if (arrived) return new Vector2(0,0);

            Cross nextCross = route[location];

            if ((pos - nextCross.pos).Length < pace) {
                location++;
                if (location == route.Count)
                    arrived = true;
                return nextCross.pos;
            }

            return pos + pace * (nextCross.pos - pos).Normalized();
        }

        public virtual void Draw() {
            float hue = Mapf(pos.X, 0, Engine.width, 60, 180);
            SetColor(new HSLColor(hue, 255 / 2,  255/2));
            DrawCircle(pos.X, pos.Y, radius);
        }
    }
}
