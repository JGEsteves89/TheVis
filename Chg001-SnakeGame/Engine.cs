﻿using Engine;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chg001_SnakeGame {

    public class Engine : IEngine {
        static Random rnd = new Random();
        static int SCALE = 20;
        class Snake {
            public Point pos;
            public Point dir;
            public List<Point> tail = new List<Point>();
            private int count = 0;

            public Snake() {
                pos = new Point(0, 0);
                dir = new Point(1, 0);
                count = 1;
                tail.Add(pos);
            }
            internal void move(Size size) {
                pos.X += dir.X * SCALE;
                pos.Y += dir.Y * SCALE;
                edge(size);
                List<Point> newTail = new List<Point>();
                newTail.Add(pos);
                for (int i = 0; i < count - 1; i++) {
                    newTail.Add(tail[i]);
                }
                tail = newTail;
            }
            internal void edge(Size size) {
                if (pos.X > size.Width - SCALE)
                    pos.X = 0;
                if (pos.X < 0)
                    pos.X = size.Width - SCALE;
                if (pos.Y > size.Height - SCALE)
                    pos.Y = 0;
                if (pos.Y < 0)
                    pos.Y = size.Height - SCALE;
            }
            internal bool contains(Point point) {
                foreach (Point item in tail) {
                    if (item.X == point.X && item.Y == point.Y)
                        return true;
                }
                return false;
            }
            internal void draw() {
                bool first = false;
                SetColor(Color.Red);
                foreach (Point p in tail) {
                    DrawRectangle(p.X, p.Y, SCALE, SCALE);
                    if (!first) {
                        first = true;
                        SetColor(Color.White);
                    }
                }
            }
            internal bool eat(Food f) {
                if (pos.X == f.pos.X && pos.Y == f.pos.Y) {
                    count++;
                    return true;
                }
                return false;
            }
        }
        class Food {
            public Point pos;
            public Food(Size size) {
                int rVal = rnd.Next(0, size.Width - SCALE);
                pos.X = rVal - rVal % SCALE;
                rVal = rnd.Next(0, size.Height - SCALE);
                pos.Y = rVal - rVal % SCALE;
            }
            internal void draw() {
                SetColor(Color.Yellow);
                DrawRectangle(pos.X, pos.Y, SCALE, SCALE);
            }
        }
        Snake s;
        Food f;
        void think() {
            List<Point> possible = new List<Point>();
            Point target = new Point(f.pos.X - s.pos.X, f.pos.Y - s.pos.Y);
            if (Math.Abs(target.X) + Math.Abs(target.Y) == 0) return;

            if (Math.Abs(target.X) > Math.Abs(target.Y)) {
                possible.Add(new Point(target.X / Math.Abs(target.X), 0));
                if (target.Y != 0)
                    possible.Add(new Point(0, target.Y / Math.Abs(target.Y)));
                else
                    possible.Add(new Point(0, 1));

            } else {
                possible.Add(new Point(0, target.Y / Math.Abs(target.Y)));
                if (target.X != 0)
                    possible.Add(new Point(target.X / Math.Abs(target.X), 0));
                else
                    possible.Add(new Point(1, 0));
            }

            possible.Add(new Point(-possible[0].X, -possible[0].Y));
            possible.Add(new Point(-possible[1].X, -possible[1].Y));

            foreach (Point item in possible) {
                if (!s.contains(new Point(item.X * SCALE + s.pos.X, item.Y * SCALE + s.pos.Y))) {
                    s.dir = item;
                    return;
                }
            }

        }
        public override void SetUp() {
            base.SetUp();
            Canvas(800, 800);
            BackgroundColor(51);
            SetFrameRate(100);
            s = new Snake();
            f = new Food(ClientSize);
        }
        public override void Draw() {
            base.Draw();
            s.move(ClientSize);
            if (s.eat(f))
                f = new Food(ClientSize);
            s.draw();
            f.draw();
            think();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e) {
            base.OnKeyDown(e);
            switch (e.Key) {
                case Key.Up:
                    s.dir = new Point(0, 1);
                    break;
                case Key.Down:
                    s.dir = new Point(0, -1);
                    break;
                case Key.Left:
                    s.dir = new Point(-1, 0);
                    break;
                case Key.Right:
                    s.dir = new Point(1, 0);
                    break;
                default:
                    break;
            }
        }
    }
}
