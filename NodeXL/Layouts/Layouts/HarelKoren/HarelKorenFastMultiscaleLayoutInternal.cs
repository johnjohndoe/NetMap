
// Define COMPILE_FOR_NODEXL to comment out code not used by NodeXL.

#define COMPILE_FOR_NODEXL

// Copyright (c) Microsoft Corporation.  All rights reserved.

// The code in this file was written by Janez Brank at Microsoft Research
// Cambridge in May 2009.  It was incorporated into NodeXL by making calls into
// it from the HarelKorenFastMultiScaleLayout class.  The only changes made to
// Janez's code were to comment out code not used by NodeXL, such as that used
// for performance measurement and drawing.


using System;
using System.Collections.Generic;
#if !COMPILE_FOR_NODEXL
using System.ComponentModel;
using System.Data;
#endif  // COMPILE_FOR_NODEXL
using System.Drawing;
#if !COMPILE_FOR_NODEXL
using System.Text;
#endif  // COMPILE_FOR_NODEXL
using System.Diagnostics;
#if !COMPILE_FOR_NODEXL
using System.Windows.Forms;
using System.IO;
#endif  // COMPILE_FOR_NODEXL

namespace MultiScaleLayout
{
    #if !COMPILE_FOR_NODEXL
    // This class contains static methods that create various kinds
    // of synthetic graphs for testing the layout methods.
    // The graphs are the same as used in Harel & Koren's 2002 paper.
    class GraphFactory
    {
        // u0 = top vertex, u1 = bottom left vertex, u2 = bottom right vertex.
        protected static void SierpinskiRecursion(Graph g, int depth, int u0, int u1, int u2)
        {
            if (depth == 0)
            {
                g.AddEdge(u0, u1);
                g.AddEdge(u1, u2);
                g.AddEdge(u2, u0);
            }
            else
            {
                int u01 = g.AddVertex();
                int u12 = g.AddVertex();
                int u02 = g.AddVertex();
                SierpinskiRecursion(g, depth - 1, u0, u01, u02);
                SierpinskiRecursion(g, depth - 1, u01, u1, u12);
                SierpinskiRecursion(g, depth - 1, u02, u12, u2);
            }

        }

        public static Graph CreateSierpinskiTriangle(int depth)
        {
            Graph g = new Graph(3, string.Format("Sierpinski Triangle, depth {0}", depth));
            SierpinskiRecursion(g, depth, 0, 1, 2);
            return g;
        }

        public static Graph CreateGrid(int w, int h)
        {
            return CreateGrid(w, h, false);
        }

        // folded = true causes the corner vertices (at diagonally opposed corners)
        // to be directly connected by an edge.
        public static Graph CreateGrid(int w, int h, bool folded)
        {
            return CreateGrid(w, h, folded, 1.0);
        }

        // Use edgeProbability < 1 if you want to randomly omit some edges from the grid.
        public static Graph CreateGrid(int w, int h, double edgeProbability)
        {
            return CreateGrid(w, h, false, edgeProbability);
        }

        public static Graph CreateGrid(int w, int h, bool folded, double edgeProbability)
        {
            Random r = new Random(123);
            string name = string.Format("Grid {0} * {1}", w, h);
            if (folded) name += ", corners folded";
            if (edgeProbability < 1) name += string.Format(", edge probability = {0}", edgeProbability);
            Graph g = new Graph(w * h, name);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    int u = y * w + x;
                    if (x + 1 < w) if (r.NextDouble() <= edgeProbability) g.AddEdge(u, u + 1);
                    if (y + 1 < h) if (r.NextDouble() <= edgeProbability) g.AddEdge(u, u + w);
                }
            if (folded)
            {
                // Connect the diagonally opposed vertices.
                g.AddEdge(0, w * h - 1);
                g.AddEdge(w - 1, w * (h - 1));
            }
            return g;
        }

        // The outermost 'border' rows and columns of the grid have
        // normal connections, but the inner part of the grid has just
        // vertical connections.
        public static Graph CreatePartialGrid(int w, int h, int leftBorder, int topBorder, int rightBorder, int bottomBorder)
        {
            string name = string.Format("Partial grid {0} * {1}, some horizontal edges omitted", w, h);
            Graph g = new Graph(w * h, name);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    int u = y * w + x;
                    if (x + 1 < w && ((x < leftBorder - 1 || x >= w - rightBorder) || (y < topBorder || y >= h - bottomBorder)))
                        g.AddEdge(u, u + 1);
                    if (y + 1 < h)
                        g.AddEdge(u, u + w);
                }
            return g;
        }

        public static Graph CreateTorus(int w, int h)
        {
            return CreateTorus(w, h, 1.0);
        }

        public static Graph CreateTorus(int w, int h, double edgeProbability)
        {
            Random r = new Random(123);
            string name = string.Format("Torus {0} * {1}", w, h);
            if (edgeProbability < 1) name += string.Format(", edge probability = {0}", edgeProbability);
            Graph g = new Graph(w * h, name);
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    int u = y * w + x;
                    if (r.NextDouble() <= edgeProbability)
                        if (x + 1 < w) g.AddEdge(u, u + 1);
                        else g.AddEdge(u, u - (w - 1));
                    if (r.NextDouble() <= edgeProbability)
                        if (y + 1 < h) g.AddEdge(u, u + w);
                        else g.AddEdge(u, u - (h - 1) * w);
                }
            return g;
        }

        public static Graph CreateFullBinaryTree(int depth)
        {
            int nVerts = (1 << (depth + 1)) - 1;
            Graph g = new Graph(nVerts, string.Format("Full binary tree, depth {0}", depth));
            for (int u = 1; u < nVerts; u++)
                g.AddEdge(u, (u - 1) / 2);
            return g;
        }

        public static Graph CreateFromList(int minNodeNo, int[] edges)
        {
            int nEdges = edges.Length; Debug.Assert(nEdges % 2 == 0); nEdges /= 2;
            int maxNodeNo = minNodeNo;
            for (int i = 0; i < nEdges * 2; i++)
                if (edges[i] > maxNodeNo) maxNodeNo = edges[i];
            Graph g = new Graph(maxNodeNo - minNodeNo + 1, "Graph specified by edge list");
            for (int i = 0; i < nEdges; i++)
                g.AddEdge(edges[2 * i] - minNodeNo, edges[2 * i + 1] - minNodeNo);
            return g;
        }

        public static Graph CreateEduarda1()
        {
            return CreateFromList(1, new int[] {
                9, 1, 
                16, 1,
                5, 1,
                11, 3,
                7, 3,
                17, 3,
                17, 4,
                7, 4,
                9, 5,
                7, 5,
                11, 5,
                6, 5,
                1, 5,
                11, 7,
                15, 8,
                16, 8,
                1, 9,
                8, 9,
                17, 11,
                16, 11,
                8, 15,
                4, 15,
                1, 16,
                16, 17,
                15, 17 });
        }
    }
    #endif  // COMPILE_FOR_NODEXL

    class Util
    {
        public const double eps = 1e-8;

        #if !COMPILE_FOR_NODEXL
        public static long GetProcessTicks()
        {
            return Process.GetCurrentProcess().TotalProcessorTime.Ticks;
        }

        public static double TicksToSec(long ticks)
        {
            return new TimeSpan(ticks).TotalSeconds;
        }
        #endif  // COMPILE_FOR_NODEXL

        public static bool ApproxEqual(double a, double b)
        {
            double d2 = (a - b) * (a - b);
            double thresh = Math.Max(a, b);
            thresh *= thresh;
            if (eps > thresh) thresh = eps;
            return d2 < eps * thresh;
        }

        public enum TLin2x2Solution
        {
            None, Point, Line, Plane
        }

        public static double LinInterp(double x1, double y1, double x2, double y2, double x)
        {
            if (Math.Abs(x2 - x1) < eps) return 0.5 * (y1 + y2);
            else return y1 + (y2 - y1) * (x - x1) / (x2 - x1);
        }

        // Solves the pair of linear equations Ax + By + C = 0, Dx + Ey + F = 0.
        // Returns one of the TLin2x2Solution constants describing the solution space.
        // If there are no solutions, x and y receive the value NaN, otherwise they
        // receive the solution closest to (0, 0).
        public static TLin2x2Solution SolveLin2by2(double A, double B, double C, double D, double E, double F, out double x, out double y)
        {
            if (Math.Abs(A) < Math.Abs(D)) { double t = A; A = D; D = t; t = B; B = E; E = t; t = C; C = F; F = t; }
            if (Math.Abs(A) < eps)
            {
                // By + C = 0, Ey + F = 0
                if (Math.Abs(B) < Math.Abs(E)) { double t = B; B = E; E = t; t = C; C = F; F = t; }
                if (Math.Abs(B) < eps)
                {
                    // C = 0, E = 0.
                    if (Math.Abs(C) < eps && Math.Abs(C) < eps) { x = 0; y = 0; return TLin2x2Solution.Plane; }
                    else { x = double.NaN; y = double.NaN; return TLin2x2Solution.None; }
                }
                y = -C / B;
                if (Math.Abs(E * y + F) < eps) { x = 0; return TLin2x2Solution.Line; }
                else { x = double.NaN; y = double.NaN; return TLin2x2Solution.None; }
            }
            // Ax + By + C = 0 means that x = (-B/A)y + (-C/A); plugging this into Dx + Ey + F = 0
            // results in (E - BD/A)y + (F - CD/A) = 0, or Uy + V = 0.
            double U = E - B * D / A, V = F - C * D / A;
            if (Math.Abs(U) < eps)
            {
                // V = 0.
                if (!(Math.Abs(V) < eps)) { x = double.NaN; y = double.NaN; return TLin2x2Solution.None; }
                // Any y is OK, and the corresponding x is x = -(By + C)/A.
                // From these solutions, let's return the one with the smallest norm.
                // f(y) = x^2 + y^2 = (B^2 y^2 + 2BC y + C^2 + A^2 y^2)/A^2,
                // f'(y) = [2(A^2 + B^2)y + 2BC] / A^2 equals 0 at
                // (A^2 + B^2)y + BC = 0.
                double den = (A * A + B * B);
                if (Math.Abs(den) < eps) y = 0;
                else y = -B * C / den;
                x = -(B * y + C) / A;
                return TLin2x2Solution.Line;
            }
            y = -V / U;
            x = -(B * y + C) / A;
            return TLin2x2Solution.Point;
        }

        // The system Ax + By + C = 0, Dx + Ey + F = 0 might not have any solutions.
        // But in that case we can still find an approximate solution, one that
        // minimizes f(x, y) = (Ax + By + C)^2 + (Dx + Ey + F)^2.
        // This function returns such a point in the x and y arguments,
        // and the corresponding value of f as the function return value.
        public static double SolveLin2by2Approx(double A, double B, double C, double D, double E, double F, out double x, out double y)
        {
            // f_x(x, y) = 2A(Ax + By + C) + 2D(Dx + Ey + F)
            // f_y(x, y) = 2B(Ax + By + C) + 2E(Dx + Ey + F)
            TLin2x2Solution retVal = SolveLin2by2(
                A * A + D * D, A * B + D * E, A * C + D * F,
                A * B + D * E, B * B + E * E, B * C + E * F,
                out x, out y);
            // Note that SolveLin2by2 can only return None if its A and D arguments
            // are both close to 0; in our case this means that A^2 + D^2 and B^2 + E^2 are
            // both close to 0, and thus A, B, D, E are all close to 0; i.e. we have two
            // equations C = 0 and F = 0.  In that case, the C and F parameters for SolveLin2by2
            // were AC + DF and BC + EF, i.e. both equal to 0, so SolveLin2By2 should have
            // returned Plane rather than None.  If, due to some numerical inaccuracy,
            // this fails, we nevertheless know that any (x, y) is an equally good approximate
            // solution of our two equations C = 0 and F = 0.
            if (retVal == TLin2x2Solution.None) { x = 0; y = 0; }
            double U = A * x + B * y + C, V = D * x + E * y + F;
            return U * U + V * V;
        }
    }

    class GraphLayoutSettings
    {
        public int localNeighRadius; // default: 7
        public int nIterations; // default: Harel & Koren 2002 use 4, but in my experience a larger number works better for some graphs, so I used 10
        public int kGrowthRatio; // default: 3
        public int minSize; // default: 10

        public GraphLayoutSettings() : this(7) { }
        public GraphLayoutSettings(int localNeighRadius_) : this(localNeighRadius_, 10) { }
        public GraphLayoutSettings(int localNeighRadius_, int nIterations_) : this(localNeighRadius_, nIterations_, 3, 10) { }
        public GraphLayoutSettings(int localNeighRadius_, int nIterations_,
            int kGrowthRatio_, int minSize_)
        {
            localNeighRadius = localNeighRadius_;
            nIterations = nIterations_;
            kGrowthRatio = kGrowthRatio_;
            minSize = minSize_;
        }
    }

    class GraphRenderSettings
    {
        #if !COMPILE_FOR_NODEXL
        public Graphics graphics;
        public Pen edgePen;
        public Brush vertexBrush;
        public int vertexRad; // each vertex is drawn as a circle with this radius
        // The graph is drawn on the rectangle with the top left corner at 'origin' and the size 'size'.  
        public Point origin;
        public Size size;
        public double margin; // amount of space left blank at the margins (relative to size.width or size.height)

        public GraphRenderSettings(Graphics graphics_, int width, int height)
        {
            edgePen = SystemPens.WindowText;
            vertexBrush = SystemBrushes.Highlight;
            vertexRad = 2;
            margin = 0.02; // the outer 2% of the destination rectangle will be left blank by default
            graphics = graphics_;
            origin = new Point(0, 0);
            size = new Size(width, height);
        }
        #endif  // COMPILE_FOR_NODEXL
    }

    struct PointD
    {
        public double x, y;
        public PointD(double x_, double y_) { x = x_; y = y_; }
    };

    class Graph
    {
        int nVerts;
        int[,] shortPath; // shortest path length
        struct Edge : IComparable<Edge>
        {
            internal int u, v;
            public Edge(int u_, int v_) { u = u_; v = v_; }
            public int CompareTo(Edge other) { int i = u - other.u; return i == 0 ? v - other.v : i; }
        };
        List<Edge> edges; // each edge is listed twice, as (u, v) and (v, u)
        public string name; // for diagnostic purposes only
        // Neighbourhood lists: deg[u] is the degree of vertex u, and the neighbours of
        // u are listed in neigh[firstNeigh[u]..firstNeigh[u]+deg[u]-1].
        int[] firstNeigh, neigh, deg;

        #if !COMPILE_FOR_NODEXL
        public int NumVerts { get { return nVerts; } }
        public int NumEdges { get { return edges == null ? 0 : edges.Count; } }

        public Graph(int nVerts_) : this(nVerts_, "") { }
        #endif  // COMPILE_FOR_NODEXL

        public Graph(int nVerts_, string Name)
        {
            nVerts = nVerts_;
            shortPath = null;
            edges = new List<Edge>();
            name = Name;
            firstNeigh = null; neigh = null; deg = null;
        }

        #if !COMPILE_FOR_NODEXL
        public void SaveEdges(string fileName)
        {
            StreamWriter f = File.CreateText(fileName);
            for (int i = 0; i < edges.Count; i++)
                if (edges[i].u < edges[i].v)
                    f.WriteLine(string.Format("{0} {1}", edges[i].u, edges[i].v));
            f.Close();
        }

        public int AddVertex()
        {
            Debug.Assert(firstNeigh == null && neigh == null && deg == null && shortPath == null);
            int u = nVerts;
            nVerts++;
            return u;
        }
        #endif  // COMPILE_FOR_NODEXL

        public void AddEdge(int u, int v)
        {
            Debug.Assert(0 <= u && u < nVerts);
            Debug.Assert(0 <= v && v < nVerts);
            #if !COMPILE_FOR_NODEXL
            Debug.Assert(u != v);
            #endif  // COMPILE_FOR_NODEXL
            edges.Add(new Edge(u, v));
            edges.Add(new Edge(v, u));
        }

        // PrepareForUse should be called after you're done adding vertices and edges,
        // and before trying to lay out the graph.  PrepareForUse removes duplicate
        // edges, prepares the neighbourhood lists and computes the shortest paths
        // between all pairs of nodes.
        public void PrepareForUse()
        {
            edges.Sort();
            // Remove duplicate edges.
            int j = 0;
            for (int i = 0; i < edges.Count; i++)
                if (j == 0 || edges[i].CompareTo(edges[j - 1]) != 0)
                    edges[j++] = edges[i];
            edges.RemoveRange(j, edges.Count - j);
            Debug.Assert(edges.Count == j);
            // Compute the degrees of all the vertices.
            deg = new int[nVerts];
            for (int u = 0; u < nVerts; u++) deg[u] = 0;
            for (int i = 0; i < edges.Count; i++) deg[edges[i].u]++;
            // Prepare the firstNeigh table.
            firstNeigh = new int[nVerts];
            firstNeigh[0] = 0;
            for (int u = 1; u < nVerts; u++) firstNeigh[u] = firstNeigh[u - 1] + deg[u - 1];
            // Prepare the neighbor lists.
            neigh = new int[edges.Count];
            int[] curNeigh = new int[nVerts];
            for (int u = 0; u < nVerts; u++) curNeigh[u] = firstNeigh[u];
            for (int i = 0; i < edges.Count; i++) neigh[curNeigh[edges[i].u]++] = edges[i].v;
            for (int u = 0; u < nVerts; u++) Debug.Assert(curNeigh[u] == firstNeigh[u] + deg[u]);
            // Prepare the matrix of all-pairs shortest paths.
            //FloydWarshall();
            BfsFromAllSources();
        }

        void BfsFromAllSources()
        {
            shortPath = new int[nVerts, nVerts];
            int[] queue = new int[nVerts];
            for (int s = 0; s < nVerts; s++)
            {
                for (int v = 0; v < nVerts; v++) shortPath[s, v] = -1;
                int head = 0, tail = 0; queue[tail++] = s; shortPath[s, s] = 0;
                while (head < tail)
                {
                    int u = queue[head++];
                    int uPath = shortPath[s, u];
                    for (int i = 0; i < deg[u]; i++)
                    {
                        int v = neigh[firstNeigh[u] + i];
                        if (shortPath[s, v] >= 0) continue;
                        shortPath[s, v] = uPath + 1;
                        queue[tail++] = v;
                    }
                }
            }
        }

        #if !COMPILE_FOR_NODEXL
        // This could be used as an alternative to BfsFromAllSources,
        // but it's slower -- it would only make sense if not all edges were
        // treated as equally long.  The asymptotic running time of Floyd-Warshall
        // is O(V^3), while for BFS from all sources it's O(V(V + E)).
        void FloydWarshall()
        {
            System.Diagnostics.StackTrace s = new StackTrace();
            Debug.Write(s.ToString());
            int[,] oldPaths = new int[nVerts, nVerts];
            int[,] newPaths = new int[nVerts, nVerts];
            for (int u = 0; u < nVerts; u++) for (int v = 0; v < nVerts; v++)
                    newPaths[u, v] = (u == v ? 0 : -1);
            for (int i = 0; i < edges.Count; i++)
            {
                newPaths[edges[i].u, edges[i].v] = 1;
                newPaths[edges[i].v, edges[i].u] = 1;
            }
            for (int t = 0; t < nVerts; t++)
            {
                if (nVerts > 100 && t % 10 == 0) Debug.WriteLine(string.Format("Floyd-Warshall, {0}/{1}", t, nVerts));
                int[,] temp = oldPaths; oldPaths = newPaths; newPaths = temp;
                for (int u = 0; u < nVerts; u++) for (int v = 0; v < nVerts; v++) newPaths[u, v] = oldPaths[u, v];
                // At this point, oldPaths[u, v] is the shortest path from u to v
                // that doesn't make use of vertices t, t+1, ..., nVerts-1.
                for (int u = 0; u < nVerts; u++) for (int v = 0; v < nVerts; v++)
                    {
                        int cand = newPaths[u, v], cand2a = oldPaths[u, t], cand2b = oldPaths[t, v];
                        if (cand2a >= 0 && cand2b >= 0)
                            if (cand < 0 || cand2a + cand2b < cand) cand = cand2a + cand2b;
                        newPaths[u, v] = cand;
                    }
            }
            shortPath = newPaths;
        }
        #endif  // COMPILE_FOR_NODEXL

        // Ideally, the k 'centers' are a set of k vertices such that
        //    max_v min_{u in centers} d_vu
        // is as small as possible.  Since this problem is NP-complete,
        // we use a greedy approximation algorithm (Harel and Koren, sec. 4.1).
        // (Here d_vu stands for the distance between v and u, which we
        // have stored in shortPath[v, u].)
        int[] KCenters(Random random, int k)
        {
            int[] centers = new int[k];
            // Select the first center randomly.
            centers[0] = random.Next(nVerts);
            // distFromCenter[u] is the distance from u to the nearest center.  
            // The value of -1 is used to represent infinity (if no centers are reachable form u), same as in shortPath.
            int[] distFromCenter = new int[nVerts];
            bool[] isCenter = new bool[nVerts];
            for (int u = 0; u < nVerts; u++) { distFromCenter[u] = shortPath[u, centers[0]]; isCenter[u] = false; }
            isCenter[centers[0]] = true;
            // Select the remaining k-1 centers.
            int[] bestCands = new int[nVerts];
            for (int i = 1; i < k; i++)
            {
                // Find the vertex u which maximizes distFromCenter[u].
                // There may be several vertices with the same (and maximal) value of distFromCenter[u].
                int bestDist = 0, nCands = 0;
                for (int u = 0; u < nVerts; u++)
                {
                    if (isCenter[u]) continue;
                    int dist = distFromCenter[u];
                    if (nCands == 0 || (dist < 0 || (bestDist >= 0 && dist >= bestDist)))
                    {
                        if (nCands > 0 && dist == bestDist) bestCands[nCands++] = u;
                        else { nCands = 1; bestDist = dist; bestCands[0] = u; }
                    }
                }
                // From these candidate vertices (which are all equally good candidates),
                // select the new center randomly.
                Debug.Assert(nCands > 0);
                int iCand = random.Next(nCands);
                int newCenter = bestCands[iCand];
                centers[i] = newCenter; isCenter[newCenter] = true;
                // Update distFromCenter to take the new center into account.
                for (int u = 0; u < nVerts; u++)
                {
                    int dist = distFromCenter[u];
                    int cand = shortPath[u, newCenter];
                    if (cand >= 0 && (dist < 0 || cand < dist))
                        distFromCenter[u] = cand;
                }
            }
            return centers;
        }

        // Returns max_v min_u d_vu, where v and u range over 'verts'.
        int GetMaxMinDist(int[] verts)
        {
            int n = verts.Length;
            int result = 0;
            for (int i = 0; i < n; i++)
            {
                int closest = -1;
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    Debug.Assert(verts[i] != verts[j]);
                    int d = shortPath[verts[i], verts[j]];
                    if (d < 0) continue;
                    if (closest < 0 || d < closest) closest = d;
                }
                // Note: what if some vertices from verts[] are unreachable from verts[i]?
                // In this case we have closest < 0; should we treat that as d_vu = \infty
                // and therefore return \infty (or nVerts, which is longer than any real path
                // in an undirected graph could be)?  Unfortunately GetMinMaxDist
                // is used chiefly to set the radius ('k') parameter for LocalLayout,
                // so returning \infty here would mean that each center's local 
                // neighbourhood would include the entire component containing that center.  
                // Since there quite likely does exist one large component and most
                // centers are located in it, this leads to many large neighbourhoods
                // and the call to LocalLayout is very time-consuming.  For example, the grid
                // created by CreateGrid(80, 80, 1.0) takes 38 sec to lay out (total time of MultiScaleLayout),
                // while the grid of CreateGrid(80, 80, 0.9) takes 140 s and CreateGrid(80, 80, 0.75) takes 232 s;
                // [CreateGrid(80, 80, 0.5) takes just 46 s, CreateGrid(80, 80, 0.25) just 9.6 s].
                //if (closest < 0 || (result >= 0 && closest > result)) result = closest;
                if (closest >= 0 && result >= 0 && closest > result) result = closest;
            }
            return result < 0 ? nVerts : result;
        }

        // For each vertex that is not one of the 'centers', 
        // this method finds the nearest center (in terms of shortest path length)
        // and moves that vertex to a random location near that center.
        void SpreadAroundCenters(int[] centers, PointD[] L, Random random, double maxDelta)
        {
            for (int v = 0, nCenters = centers.Length; v < nVerts; v++)
            {
                int dBest = -1, iBest = -1;
                for (int i = 0; i < nCenters; i++)
                {
                    int u = centers[i]; if (u == v) { iBest = i; dBest = 0; break; }
                    int d_uv = shortPath[v, u];
                    if (iBest < 0 || (dBest < 0 && d_uv >= 0) || (dBest >= 0 && d_uv >= 0 && d_uv < dBest)) { dBest = d_uv; iBest = i; }
                }
                if (dBest == 0) continue;
                int uu = centers[iBest];
                L[v].x = L[uu].x + (random.NextDouble() * 2 - 1) * maxDelta;
                L[v].y = L[uu].y + (random.NextDouble() * 2 - 1) * maxDelta;
            }
        }

        // Normalizes all coordinates to the range [0, 1].
        // This should only be done after the Harel-Koren layout algorithm terminates;
        // to do it previously would interfere with the algorithm, whose cost function
        // encourages the Euclidean distance between the vertices to be similar to their
        // graph-theoretic distance, and so it makes no sense to require that all coordinates
        // should be in the range [0, 1].
        void RenormalizeCoordinates(PointD[] L)
        {
            double xMin = double.NaN, xMax = double.NaN, yMin = double.NaN, yMax = double.NaN;
            for (int v = 0; v < nVerts; v++)
            {
                if (v == 0 || L[v].x < xMin) xMin = L[v].x;
                if (v == 0 || L[v].x > xMax) xMax = L[v].x;
                if (v == 0 || L[v].y < yMin) yMin = L[v].y;
                if (v == 0 || L[v].y > yMax) yMax = L[v].y;
            }
            for (int v = 0; v < nVerts; v++)
            {
                L[v].x = Util.LinInterp(xMin, 0, xMax, 1, L[v].x);
                L[v].y = Util.LinInterp(yMin, 0, yMax, 1, L[v].y);
            }
        }

        // This counter measures the time spent by LocalLayout to find the
        // next vertex to move.  Harel & Koren recommend using a heap
        // for this, while our current implementation uses a less efficient
        // linear search instead.  In the experiments so far, the algorithm
        // spent only a small percentage of time on this operation, so it
        // did not seem worth while to optimize it.
        public long nTicksInFindMin = 0;

        // This is the LocalLayout algorithm from sec. 4.2 of Harel & Koren (2002).
        // It attempts to minimize the cost function
        //    E = sum_{v in verts} sum_{u in verts, u != v, d_vu < k} (1/d_uv) (||L_u - L_v|| - d_uv)^2,
        // where L_u and L_v are the points in the 2-d plane to which vertices u and v
        // have been mapped, and d_uv = shortPath[u, v].  
        // Note that it is possible for this process to end up in a
        // local minimum which corresponds to a worse layout than the global minimum.
        double LocalLayout(int[] verts, PointD[] L, int k, int nIterations)
        {
            const double eps = 1e-8;
            int nLocVerts = verts.Length;
            int[] nLocNeigh, firstLocNeigh, locNeigh; int nAllLocNeigh = 0;
            nLocNeigh = new int[nLocVerts]; firstLocNeigh = new int[nLocVerts];
            /*
            for (int i = 0; i < nLocVerts; i++)
            {
                firstLocNeigh[i] = nAllLocNeigh;
                int m = 0, v = verts[i];
                for (int u = 0; u < nVerts; u++)
                    if (shortPath[v, u] >= 0 && shortPath[v, u] < k) m++;
                nLocNeigh[i] = m; nAllLocNeigh += m;
            }
            locNeigh = new int[nAllLocNeigh];
            for (int i = 0; i < nLocVerts; i++)
                for (int m = firstLocNeigh[i], v = verts[i], u = 0; u < nVerts; u++)
                    if (shortPath[v, u] >= 0 && shortPath[v, u] < k) locNeigh[m++] = u;
            */
            // For each node v from 'verts', we want to find other nodes from 'verts'
            // which are less than k steps away from it in the original graph.
            // We will store the number of these nodes in nLocNeigh[v], and
            // their indices (indices into 'verts', not node IDs in the range 0..nVerts-1)
            // into locNeigh[firstLocNeigh[v]..firstLocNeigh[v]+nLocNeigh[v]-1].
            for (int i = 0; i < nLocVerts; i++)
            {
                firstLocNeigh[i] = nAllLocNeigh;
                int m = 0, v = verts[i];
                for (int j = 0; j < nLocVerts; j++)
                    if (i != j && shortPath[v, verts[j]] >= 0 && shortPath[v, verts[j]] < k) m++;
                nLocNeigh[i] = m; nAllLocNeigh += m;
            }
            locNeigh = new int[nAllLocNeigh];
            for (int i = 0; i < nLocVerts; i++)
                for (int j = 0, m = firstLocNeigh[i], v = verts[i]; j < nLocVerts; j++)
                    if (i != j && shortPath[v, verts[j]] >= 0 && shortPath[v, verts[j]] < k) locNeigh[m++] = j;
            // dEdX[i] and dEdY[i] are partial derivatives of the cost function E
            // wrt the x- and y- coordinates of the node verts[i]; Egradient[i] is
            // dEdX[i]^2 + dEdY[i]^2.
            double[] dEdX = new double[nLocVerts], dEdY = new double[nLocVerts], Egradient = new double[nLocVerts];
            for (int i = 0; i < nLocVerts; i++) { dEdX[i] = 0; dEdY[i] = 0; }
            Func<int, double> kFunc = ((int d_uv) => 1.0 / ((double)d_uv)); // could also be  1 / d_uv^2
            //kFunc = ((int d_uv) => (double)1);
            #if !COMPILE_FOR_NODEXL
            long tmStart = Util.GetProcessTicks();
            #endif  // COMPILE_FOR_NODEXL
            const double baseEdgeLen = 1;
            // Compute the partial derivatives given the initial layout of the nodes.
            for (int i = 0; i < nLocVerts; i++)
            {
                int v = verts[i];
                double xv = L[v].x, yv = L[v].y;
                for (int ni = 0; ni < nLocNeigh[i]; ni++)
                {
                    int j = locNeigh[firstLocNeigh[i] + ni];
                    int u = verts[j];
                    Debug.Assert(u != v); if (u == v) continue;
                    // E contains a term k_uv ( || L_u - L_v || - l * d_uv )^2.
                    // Where l = baseEdgeLen, k_uv = 1/d_uv or 1/d_uv^2.
                    // Note that || L_u - L_v || = ((x_u - x_v)^2 + (y_u - y_v)^2)^{1/2}.
                    // This term contributes the following to the partial derivative of E wrt x_u:
                    //    k_uv 2 ( || L_u - L_v || - l d_uv ) (1/2) || L_u - L_v ||^{-1} 2 (x_u - x_v)
                    double xu = L[u].x, yu = L[u].y;
                    int d_uv = shortPath[v, u];
                    if (d_uv < 0) continue; // u is unreachable from v; this shouldn't happen since we selected u
                    // such that d_uv should be < k; anyway, u being unreachable from v means
                    // that d_uv = infty and therefore k_uv = 0, so this pair (u, v) doesn't affect E and its derivatives.
                    double k_uv = kFunc(d_uv);
                    double Ldist = (xu - xv) * (xu - xv) + (yu - yv) * (yu - yv);
                    Ldist = (Ldist < eps) ? 0 : Math.Sqrt(Ldist);
                    double t = k_uv * 2 * (Ldist - baseEdgeLen * d_uv) / Math.Max(eps, Ldist);
                    dEdX[j] += t * (xu - xv); dEdX[i] += t * (xv - xu);
                    dEdY[j] += t * (yu - yv); dEdY[i] += t * (yv - yu);
                }
            }
            for (int i = 0; i < nLocVerts; i++) Egradient[i] = dEdX[i] * dEdX[i] + dEdY[i] * dEdY[i];
            // Now perform a number of local optimization steps to try to
            // minimize the value of the cost function E.
            for (int iterNo = 0; iterNo < 1 * nIterations * nLocVerts; iterNo++)
            {
                #if !COMPILE_FOR_NODEXL
                if (false && iterNo % nIterations == 0)
                {
                    // Perhaps it might occasionally be desirable to recalculate the gradients,
                    // in case of numerical errors?
                    for (int i = 0; i < nLocVerts; i++) { dEdX[i] = 0; dEdY[i] = 0; }
                    for (int i = 0; i < nLocVerts; i++)
                    {
                        int vv = verts[i];
                        double xvv = L[vv].x, yvv = L[vv].y;
                        for (int ni = 0; ni < nLocNeigh[i]; ni++)
                        {
                            int j = locNeigh[firstLocNeigh[i] + ni];
                            int u = verts[j];
                            Debug.Assert(u != vv); if (u == vv) continue;
                            // E contains a term k_uv ( || L_u - L_v || - l * d_uv )^2.
                            // Where l = baseEdgeLen, k_uv = 1/d_uv or 1/d_uv^2.
                            // Note that || L_u - L_v || = ((x_u - x_v)^2 + (y_u - y_v)^2)^{1/2}.
                            // This term contributes the following to the partial derivative of E wrt x_u:
                            //    k_uv 2 ( || L_u - L_v || - l d_uv ) (1/2) || L_u - L_v ||^{-1} 2 (x_u - x_v)
                            double xu = L[u].x, yu = L[u].y;
                            int d_uv = shortPath[vv, u];
                            if (d_uv < 0) continue; // u is unreachable from v; this shouldn't happen since we selected u
                            // such that d_uv should be < k; anyway, u being unreachable from v means
                            // that d_uv = infty and therefore k_uv = 0, so this pair (u, v) doesn't affect E and its derivatives.
                            double k_uv = kFunc(d_uv);
                            double Ldist = (xu - xvv) * (xu - xvv) + (yu - yvv) * (yu - yvv);
                            Ldist = (Ldist < eps) ? 0 : Math.Sqrt(Ldist);
                            double tt = k_uv * 2 * (Ldist - baseEdgeLen * d_uv) / Math.Max(eps, Ldist);
                            dEdX[j] += tt * (xu - xvv); dEdX[i] += tt * (xvv - xu);
                            dEdY[j] += tt * (yu - yvv); dEdY[i] += tt * (yvv - yu);
                        }
                    }
                    for (int i = 0; i < nLocVerts; i++) Egradient[i] = dEdX[i] * dEdX[i] + dEdY[i] * dEdY[i];
                }
                #endif  // COMPILE_FOR_NODEXL
                // Choose the vertex v which maximizes Egradient(v).  
                // ToDo: eventually we'll want to use a heap for this.  However, in all the experiments
                // so far, the percentage of time spent in this loop was so small that it doesn't seem
                // worth the trouble of replacing it with a heap.
                long tmFindMinStart = Process.GetCurrentProcess().UserProcessorTime.Ticks;
                int v, vi = -1;
                for (int i = 0; i < nLocVerts; i++) if (vi < 0 || Egradient[i] > Egradient[vi]) vi = i;
                nTicksInFindMin += Process.GetCurrentProcess().UserProcessorTime.Ticks - tmFindMinStart;
                v = verts[vi];
                double xv = L[v].x, yv = L[v].y;
                // We want to find a local minimum of E, i.e. move to a point where
                // the partial derivatives are approximately 0.  For the moment,
                // consider E as a function of only xv and yv.  If we move the vertex v
                // from (xv, yv) to (xv + dx, yv + dy), we will have the partial derivative
                // E_x(xv + dx, yv + dy) \approx E_x(xv, yv) + dx E_xx(xv, yv) + dy E_xy(xv, yv),
                // and we want this to equal zero.  A similar equation can be written for E_y.
                // Thus we have a system of two linear equations with two unknowns, dx and dy.
                // We already have the values of E_x and E_y, but now we have to also
                // compute the second derivatives.
                double dXX = 0, dYY = 0, dXY = 0, dYX = 0, dX = 0, dY = 0;
                for (int ni = 0; ni < nLocNeigh[vi]; ni++)
                {
                    int ui = locNeigh[firstLocNeigh[vi] + ni];
                    int u = verts[ui];
                    Debug.Assert(u != v); //if (u == v) continue;
                    // E contains a term k_uv ( || L_u - L_v || - l d_uv )^2,
                    // where ||L_u - L_v|| = ((x_u - x_v)^2 + (y_u - y_v)^2)^{1/2}.
                    // This contributes k_uv 2 ( || L_u - L_v || - l d_uv) (1/2) || L_u - L_v ||^{-1} 2(x_v - x_u)
                    // to the partial derivative of E wrt x_v.
                    // Thus it contributes
                    //   k_uv 2 ( (1/2) || L_u - L_v ||^{-1} 2 (x_v - x_u) ) || L_u - L_v ||^{-1} (x_v - x_u)
                    // + k_uv 2 ( || L_u - L_v || - l d_uv) (-1/2) || L_u - L_v ||^{-3} 2 (x_v - x_u) (x_v - x_u)
                    // + k_uv 2 ( || L_u - L_v || - l d_uv) || L_u - L_v ||^{-1}
                    // to the partial derivative dE^2 / dx_v^2; and it contributes
                    //   k_uv 2 ( (1/2) || L_u - L_v ||^{-1} 2 (y_v - y_u) ) || L_u - L_v ||^{-1} (x_v - x_u)
                    // + k_uv 2 ( || L_u - L_v || - l d_uv) (-1/2) || L_u - L_v ||^{-3} 2 (y_v - y_u) (x_v - x_u)
                    // to the partial derivative dE^2 / dx_v dy_v.
                    double xu = L[u].x, yu = L[u].y;
                    int d_uv = shortPath[v, u];
                    double k_uv = kFunc(d_uv);
                    double Ldist2 = (xu - xv) * (xu - xv) + (yu - yv) * (yu - yv); // || L_u - L_v ||^2
                    double Ldist = (Ldist2 < eps) ? eps : Math.Sqrt(Ldist2); // || L_u - L_v ||
                    double Ldist3 = Ldist2 * Ldist;
                    dX += k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist * 2 * (xv - xu);
                    dXX += k_uv * ((xv - xu) / Ldist2) * (xv - xu) * 2;
                    dXX -= k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist3 * 2 * (xv - xu) * (xv - xu);
                    dXX += k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist * 2;
                    dXY += k_uv * (yv - yu) / Ldist2 * (xv - xu) * 2;
                    dXY -= k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist3 * 2 * (yv - yu) * (xv - xu);
                    dY += k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist * 2 * (yv - yu);
                    dYY += k_uv * ((yv - yu) / Ldist2) * (yv - yu) * 2;
                    dYY -= k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist3 * 2 * (yv - yu) * (yv - yu);
                    dYY += k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist * 2;
                    dYX += k_uv * (xv - xu) / Ldist2 * (yv - yu) * 2;
                    dYX -= k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist3 * 2 * (xv - xu) * (yv - yu);
                    /*
                    // Old, buggy formulas.
                    dYY += k_uv * ((yv - yu) / Ldist2) * 2 * (yv - yu);
                    dYY -= k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist2 * 4 * (yv - yu) * (yv - yu);
                    dYY += k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist * 2;
                    dYX += k_uv * (xv - xu) / Ldist2 * 2 * (yv - yu);
                    dYX -= k_uv * (Ldist - baseEdgeLen * d_uv) / Ldist2 * 4 * (xv - xu) * (yv - yu); 
                    */
                }
                // Note: originally we computed dEdX and dEdY so as to take each
                // link into account twice, as (u, v) and (v, u).  Hence we will now
                // multiply our derivatives by 2.
                dX *= 2; dY *= 2; dXX *= 2; dXY *= 2; dYX *= 2; dYY *= 2;
                Debug.Assert(Util.ApproxEqual(dX, dEdX[vi]));
                Debug.Assert(Util.ApproxEqual(dY, dEdY[vi]));
                // Now we want to solve the equations
                //      dX + s dXX + t dXY = 0   and   dY + s dYX + t dYY = 0.
                double s, t;
                Util.SolveLin2by2Approx(dXX, dXY, dX, dYX, dYY, dY, out s, out t);
                double moveDist = s * s + t * t;
                #if !COMPILE_FOR_NODEXL
                const double maxMove = 0.03;
                #endif  // COMPILE_FOR_NODEXL
                double moveCoef = 1.0;
                #if !COMPILE_FOR_NODEXL
                if (false && moveDist > maxMove * maxMove) moveCoef = maxMove / Math.Sqrt(moveDist);
                #endif  // COMPILE_FOR_NODEXL
                s *= moveCoef; t *= moveCoef;
                double xNew = xv + s, yNew = yv + t;
                // The change of v's coordinates affects the derivatives of E wrt 
                // x_u and y_u for all u that are neighbours of v; and of course for u = v.  
                // We will now update the corresponding entries in the dEdX, dEdY and Egradient arrays.
                double dEdXv_old = 0, dEdXv_new = 0, dEdYv_old = 0, dEdYv_new = 0;
                for (int ni = 0; ni < nLocNeigh[vi]; ni++)
                {
                    int j = locNeigh[firstLocNeigh[vi] + ni];
                    int u = verts[j];
                    Debug.Assert(u != v); if (u == v) continue;
                    // E contains a term k_uv ( || L_u - L_v || - l * d_uv )^2.
                    // Where l = baseEdgeLen, k_uv = 1/d_uv or 1/d_uv^2.
                    // Note that || L_u - L_v || = ((x_u - x_v)^2 + (y_u - y_v)^2)^{1/2}.
                    // This term contributes the following to the partial derivative of E wrt x_u:
                    //    k_uv 2 ( || L_u - L_v || - l d_uv ) (1/2) || L_u - L_v ||^{-1} 2 (x_u - x_v)
                    double xu = L[u].x, yu = L[u].y;
                    int d_uv = shortPath[v, u];
                    if (d_uv < 0) continue; // u is unreachable from v; this shouldn't happen since we selected u
                    // such that d_uv should be < k; anyway, u being unreachable from v means
                    // that d_uv = \infty and therefore k_uv = 0, so this pair (u, v) doesn't affect E and its derivatives.
                    double k_uv = kFunc(d_uv);
                    double oldLdist = (xu - xv) * (xu - xv) + (yu - yv) * (yu - yv);
                    double newLdist = (xu - xNew) * (xu - xNew) + (yu - yNew) * (yu - yNew);
                    oldLdist = (oldLdist < eps) ? 0 : Math.Sqrt(oldLdist);
                    newLdist = (newLdist < eps) ? 0 : Math.Sqrt(newLdist);
                    double ttOld = k_uv * 2 * (oldLdist - baseEdgeLen * d_uv) / Math.Max(eps, oldLdist);
                    double ttNew = k_uv * 2 * (newLdist - baseEdgeLen * d_uv) / Math.Max(eps, newLdist);
                    dEdX[j] -= 2 * ttOld * (xu - xv); // '2 *' because our loop here looks just at (v, u) pairs, but not at (u, v) pairs.
                    dEdX[j] += 2 * ttNew * (xu - xNew);
                    dEdY[j] -= 2 * ttOld * (yu - yv);
                    dEdY[j] += 2 * ttNew * (yu - yNew);
                    Egradient[j] = dEdX[j] * dEdX[j] + dEdY[j] * dEdY[j];
                    dEdXv_old += 2 * ttOld * (xv - xu);
                    dEdXv_new += 2 * ttNew * (xNew - xu);
                    dEdYv_old += 2 * ttOld * (yv - yu);
                    dEdYv_new += 2 * ttNew * (yNew - yu);
                }
                Debug.Assert(Math.Abs(dEdX[vi] - dEdXv_old) < eps);
                Debug.Assert(Math.Abs(dEdY[vi] - dEdYv_old) < eps);
                dEdX[vi] = dEdXv_new; dEdY[vi] = dEdYv_new;
                Egradient[vi] = dEdX[vi] * dEdX[vi] + dEdY[vi] * dEdY[vi];
                L[v].x = xNew; L[v].y = yNew;
                //
                #if !COMPILE_FOR_NODEXL
                if (nVerts < 5 && false) // Debug output.
                {
                    Debug.Write("Coords: ");
                    for (int u = 0; u < nVerts; u++) Debug.Write(string.Format(" ({0:0.000}, {1:0.000})", L[u].x, L[u].y));
                    Debug.Write(" - Grads: ");
                    for (int i = 0; i < nLocVerts; i++) Debug.Write(string.Format(" {0}:({1:0.000}, {2:0.000})/{3:0.00}", verts[i], dEdX[i], dEdY[i], Egradient[i])); Debug.WriteLine("");
                }
                #endif  // COMPILE_FOR_NODEXL
            }
            // All the time so far we were working just with the derivatives
            // of the cost function E.  However, we would like to also report the
            // final value of E itself to the caller.
            double E = 0;
            for (int i = 0; i < nLocVerts; i++)
            {
                int vv = verts[i];
                double xvv = L[vv].x, yvv = L[vv].y;
                for (int ni = 0; ni < nLocNeigh[i]; ni++)
                {
                    int j = locNeigh[firstLocNeigh[i] + ni];
                    int u = verts[j];
                    Debug.Assert(u != vv); if (u == vv) continue;
                    // E contains a term k_uv ( || L_u - L_v || - l * d_uv )^2.
                    double xu = L[u].x, yu = L[u].y;
                    int d_uv = shortPath[vv, u];
                    if (d_uv < 0) continue;
                    double k_uv = kFunc(d_uv);
                    double Ldist = (xu - xvv) * (xu - xvv) + (yu - yvv) * (yu - yvv);
                    Ldist = (Ldist < eps) ? 0 : Math.Sqrt(Ldist);
                    double error = (Ldist - baseEdgeLen * d_uv);
                    E += k_uv * error * error;
                }
            }
            #if !COMPILE_FOR_NODEXL
            Console.WriteLine(string.Format(" - LocalLayout: {0} centers, neigh radius {1}, total neighbor count: {2} -> {3:0.000} s", nLocVerts, k, nAllLocNeigh, Util.TicksToSec(Util.GetProcessTicks() - tmStart)));
            #endif  // COMPILE_FOR_NODEXL
            return E;
        }

        public PointD[] vertexCoords = null;

        // Returns a new array containing some of the elements of 'allCoords',
        // namely those whose indices are listed in 'indices'.
        PointD[] BackupPoints(PointD[] allCoords, int[] indices)
        {
            int n = indices.Length;
            PointD[] bkp = new PointD[n];
            for (int i = 0; i < n; i++) bkp[i] = allCoords[indices[i]];
            return bkp;
        }

        // For each i, stores 'backup[i]' into 'allCoords[indices[i]]'.
        void RestorePoints(PointD[] allCoords, int[] indices, PointD[] backup)
        {
            int n = indices.Length;
            for (int i = 0; i < n; i++) allCoords[indices[i]] = backup[i];
        }

        public void MultiScaleLayout(int randSeed) { MultiScaleLayout(randSeed, null, null); }
        public void MultiScaleLayout(int randSeed, GraphLayoutSettings gls) { MultiScaleLayout(randSeed, gls, null); }
        public void MultiScaleLayout(int randSeed, GraphRenderSettings grs) { MultiScaleLayout(randSeed, null, grs); }

        // This is the main subroutine of the Harel & Koren multi-scale
        // layout algorithm (sec. 4.3 in their 2002 paper).
        // - If 'gls' is null, default settings will be used.
        // - If 'grs' is non-null, it will be used to draw the graph after
        //   each iteration of the main loop.
        // When the function returns, the new layout of the graph
        // will be available in 'this.vertexCoords'.
        public void MultiScaleLayout(int randSeed, GraphLayoutSettings gls, GraphRenderSettings grs)
        {
            Random random = new Random(randSeed);
            #if !COMPILE_FOR_NODEXL
            if (false) // return a random layout, for debugging purposes
            {
                this.vertexCoords = new PointD[nVerts];
                for (int u = 0; u < nVerts; u++) vertexCoords[u] = new PointD(random.NextDouble(), random.NextDouble());
                return;
            }
            #endif  // COMPILE_FOR_NODEXL
            //
            if (gls == null) gls = new GraphLayoutSettings();
            int localNeighRadius = gls.localNeighRadius, nIterations = gls.nIterations, kGrowthRatio = gls.kGrowthRatio, minSize = gls.minSize;
            //
            nTicksInFindMin = 0;
            PointD[] L = new PointD[nVerts];
            vertexCoords = L;
            //Random random = new Random(randSeed);
            for (int v = 0; v < nVerts; v++) { L[v].x = random.NextDouble(); L[v].y = random.NextDouble(); }
            /*L[0].x = 0; L[0].y = 0;
            L[1].x = 1; L[1].y = 0;
            L[2].x = -1.5; L[2].y = 0;*/
            int k = minSize; if (k > nVerts) k = nVerts;
            while (k <= nVerts)
            {
                int[] centers = KCenters(random, k);
                int radius = GetMaxMinDist(centers);
                radius *= localNeighRadius;
                // In my experience, the cost function used by LocalLayout sometimes has local
                // minima that make for a very undesirable layout, e.g. having all vertices on
                // a single straight line.  In an effort to avoid this at least at the first
                // stages of the algorithm, when 'k' is small and thus LocalLayout is fast,
                // we will run it multiple times and keep the best of the layouts obtained this way.
                int nAttempts = Math.Max(1, 100 / centers.Length); 
                // Note: if we want to be more faithful to the original Harel & Koren algorithm, 
                // we could just set nAttempts to 1 at this point.
                double bestError = -1; PointD[] bestNewCoords = null, origCoords = BackupPoints(L, centers);
                for (int attemptNo = 0; attemptNo < nAttempts; attemptNo++)
                {
                    if (attemptNo > 0) RestorePoints(L, centers, origCoords);
                    double error = LocalLayout(centers, L, radius, nIterations);
                    if (attemptNo == 0 || error < bestError) { bestError = error; bestNewCoords = BackupPoints(L, centers); /*Render(grs);*/ }
                }
                RestorePoints(L, centers, bestNewCoords);
                #if !COMPILE_FOR_NODEXL
                // For debugging purposes, we can display the current layout of the graph.
                if (grs != null)
                {
                    SpreadAroundCenters(centers, L, random, 0);
                    if (nVerts <= 150) { Render(grs); Application.DoEvents(); }
                    Debug.WriteLine(string.Format("# - k = {0}, error = {1:0.00}", k, bestError));
                    //System.Threading.Thread.Sleep(1500);
                }
                #endif  // COMPILE_FOR_NODEXL

                // Now that the 'k' centers have been laid out nicely, arrange other
                // vertices around their corresponding nearest centers (nearest in terms
                // of the shortest path in the graph, that is).
                SpreadAroundCenters(centers, L, random, 0.1);
                //
                if (k == nVerts) break;
                k *= kGrowthRatio;
                if (k > nVerts) k = nVerts;
            }
            RenormalizeCoordinates(L);
        }

        #if !COMPILE_FOR_NODEXL
        public void Render(GraphRenderSettings settings)
        {
            Graphics g = settings.graphics;
            //
            g.FillRectangle(SystemBrushes.Window, new Rectangle(settings.origin, settings.size));
            int[] xs = new int[nVerts], ys = new int[nVerts];
            double xMin = vertexCoords[0].x, yMin = vertexCoords[0].y;
            double xMax = xMin, yMax = yMin;
            for (int v = 0; v < nVerts; v++)
            {
                PointD pt = vertexCoords[v];
                if (pt.x > xMax) xMax = pt.x; if (pt.x < xMin) xMin = pt.x;
                if (pt.y > yMax) yMax = pt.y; if (pt.y < yMin) yMin = pt.y;
            }
            for (int v = 0; v < nVerts; v++)
            {
                PointD pt = vertexCoords[v];
                double x = Util.LinInterp(xMin, settings.margin, xMax, 1 - settings.margin, pt.x);
                double y = Util.LinInterp(yMin, settings.margin, yMax, 1 - settings.margin, pt.y);
                xs[v] = (int)Math.Round(settings.origin.X + x * settings.size.Width);
                ys[v] = (int)Math.Round(settings.origin.Y + y * settings.size.Height);
            }
            for (int v = 0; v < nVerts; v++)
            {
                for (int i = 0; i < deg[v]; i++)
                {
                    int u = neigh[firstNeigh[v] + i];
                    if (u >= v) continue;
                    g.DrawLine(settings.edgePen, xs[v], ys[v], xs[u], ys[u]);
                }
            }
            int vertexRad = settings.vertexRad;
            for (int v = 0; v < nVerts; v++)
            {
                g.FillEllipse(settings.vertexBrush, xs[v] - vertexRad, ys[v] - vertexRad, 2 * vertexRad + 1, 2 * vertexRad + 1);
            }
        }
        #endif
    };

}
