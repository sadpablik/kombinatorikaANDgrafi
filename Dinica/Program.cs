using System;
using System.Collections.Generic;

public class Dinic
{
    private int n; // Количество вершин
    private List<int>[] adj; // Список смежности
    private int[,] capacity; // Матрица пропускных способностей
    private int[] level; // Уровни вершин
    private int[] ptr; // Указатели для обхода

    public Dinic(int n)
    {
        this.n = n;
        adj = new List<int>[n];
        for (int i = 0; i < n; i++)
            adj[i] = new List<int>();
        capacity = new int[n, n];
        level = new int[n];
        ptr = new int[n];
    }

    public void AddEdge(int u, int v, int cap)
    {
        adj[u].Add(v);
        adj[v].Add(u);
        capacity[u, v] += cap; // Прямое ребро
        capacity[v, u] += 0;   // Обратное ребро
    }


    private bool BFS(int s, int t)
    {
        for (int i = 0; i < n; i++)
            level[i] = -1;
        level[s] = 0;
        Queue<int> q = new Queue<int>();
        q.Enqueue(s);
        while (q.Count > 0)
        {
            int u = q.Dequeue();
            foreach (int v in adj[u])
            {
                if (level[v] < 0 && capacity[u, v] > 0)
                {
                    level[v] = level[u] + 1;
                    q.Enqueue(v);
                }
            }
        }
        return level[t] >= 0;
    }

    private int DFS(int u, int t, int flow)
    {
        if (u == t)
            return flow;
        for (; ptr[u] < adj[u].Count; ptr[u]++)
        {
            int v = adj[u][ptr[u]];
            if (level[v] == level[u] + 1 && capacity[u, v] > 0)
            {
                int df = DFS(v, t, Math.Min(flow, capacity[u, v]));
                if (df > 0)
                {
                    capacity[u, v] -= df;
                    capacity[v, u] += df;
                    return df;
                }
            }
        }
        return 0;
    }

    public int MaxFlow(int s, int t)
    {
        int flow = 0;
        while (BFS(s, t))
        {
            for (int i = 0; i < n; i++)
                ptr[i] = 0;
            int df;
            while ((df = DFS(s, t, int.MaxValue)) > 0)
            {
                flow += df;
            }
        }
        return flow;
    }
}

public class Program
{
    public static void Main()
    {
        Dinic dinic = new Dinic(6);

        // Добавление рёбер: (u, v, capacity)
        dinic.AddEdge(0, 1, 10);
        dinic.AddEdge(0, 2, 10);
        dinic.AddEdge(1, 3, 4);
        dinic.AddEdge(1, 4, 8);
        dinic.AddEdge(2, 4, 9);
        dinic.AddEdge(3, 5, 10);
        dinic.AddEdge(4, 5, 10);

        // Нахождение максимального потока из 0 в 5
        Console.WriteLine("Maximum Flow: " + dinic.MaxFlow(0, 5)); // Output: Maximum Flow: 19
    }
}
