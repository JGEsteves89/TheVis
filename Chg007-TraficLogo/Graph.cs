using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chg007_TraficLogo {
    public class Node {
        public int i = 0;
        public object data = null;
        public Node(int i, object data = null) {
            this.i = i;
            this.data = data;
        }
    }
    public class Edge {     
        public int i;
        public int j;
        public float cost = 0;
        public Edge(int i, int j, float cost = 1) {
            this.i = i;
            this.j = j;
            this.cost = cost;
        }
    }
    public class Graph {
        public List<Node> nodes = new List<Node>();
        public List<Edge> edges = new List<Edge>();

        public Edge getEdge(int i, int j) {
            foreach (Edge item in edges) {
                if (item.i == i && item.j == j)
                    return item;
                else if (item.i == j && item.j == i)
                    return item;
            }
            return null;
        }
    }
    public class Dijkstra {
        public static List<int> runDijkstra(Graph graph, int source, int target) {
            List<int> prev = new List<int>();
            List<int> sol = new List<int>();
            List<float> D = new List<float>();
            List<bool> S = new List<bool>();
            List<int> Q = new List<int>();
            for (int i = 0; i < graph.nodes.Count; i++) {
                D.Add(int.MaxValue);
                S.Add(false);
                prev.Add(-1);
            }
            D[source] = 0;

           
            for (int i = 0; i < graph.nodes.Count; i++) {
                int u = SelectIndexMinDist(D,S);
                S[u] = true;
                for (int v = 0; v < graph.nodes.Count; v++) {
                    if(!S[v]){
                        Edge edge = graph.getEdge(u, v);
                        if (edge != null) {
                            float alt = D[u] + edge.cost;
                            if (alt < D[v]) {
                                D[v] = alt;
                                prev[v] = u;
                            }
                        }
                    }
                }
            }
            sol.Add(target);
            int cur = prev[target];
            while (cur != source) {
                if (cur == -1) throw new Exception();
                sol.Add(cur);
                cur = prev[cur];
            }
            sol.Add(source);
            sol.Reverse();
            return sol;
        }

        private static int SelectIndexMinDist(List<float> d, List<bool> s) {
            float min = int.MaxValue;
            int minIndex = 0;
            for (int i = 0; i < d.Count; i++) {
                if (!s[i] && d[i]<= min) {
                    min = d[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }
    }
}
