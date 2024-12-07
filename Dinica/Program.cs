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
        // Пример 1: Простой граф
        Dinic dinic1 = new Dinic(4);
        dinic1.AddEdge(0, 1, 10);
        dinic1.AddEdge(0, 2, 5);
        dinic1.AddEdge(1, 3, 5);
        dinic1.AddEdge(2, 3, 10);
        Console.WriteLine("Maximum Flow (Simple Graph): " + dinic1.MaxFlow(0, 3)); // Expected: 10

        // Пример 2: Сложный граф
        Dinic dinic2 = new Dinic(6);
        dinic2.AddEdge(0, 1, 10);
        dinic2.AddEdge(0, 2, 10);
        dinic2.AddEdge(1, 3, 4);
        dinic2.AddEdge(1, 4, 8);
        dinic2.AddEdge(2, 4, 9);
        dinic2.AddEdge(3, 5, 10);
        dinic2.AddEdge(4, 5, 10);
        Console.WriteLine("Maximum Flow (Complex Graph): " + dinic2.MaxFlow(0, 5)); // Expected: 14

        // Пример 3: Циклический граф
        Dinic dinic3 = new Dinic(5);
        dinic3.AddEdge(0, 1, 10);
        dinic3.AddEdge(1, 2, 5);
        dinic3.AddEdge(2, 3, 5);
        dinic3.AddEdge(3, 1, 5);
        dinic3.AddEdge(3, 4, 5);
        Console.WriteLine("Maximum Flow (Cyclic Graph): " + dinic3.MaxFlow(0, 4)); // Expected: 10
    }
}
