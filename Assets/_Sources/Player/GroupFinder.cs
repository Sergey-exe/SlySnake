using System;
using System.Collections.Generic;
using _Sources.Map;
using UnityEngine;

namespace _Sources.Player
{
    public class GroupFinder
    {
        public static void FindGroups(int[,] array, int playersCount, out List<List<(int, int)>> groups)
        {
            foreach (var VARIABLE in array)
            {
                Debug.Log(VARIABLE);
            }
            
            if (playersCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(playersCount));

            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            
            bool[,] visited = new bool[rows, cols];

            groups = new List<List<(int, int)>>();
            
            for (int i = 0; i < playersCount; i++)
            {
                groups.Add(new List<(int, int)>());
            }

            int foundGroups = 0;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // Начинаем BFS только для ячеек с целью (TailPlayer) и ещё не посещённых
                    if (array[i, j] == (int)MapItemType.TailPlayer && !visited[i, j])
                    {
                        if (foundGroups >= playersCount)
                            throw new Exception(
                                $"Обнаружено больше заявленных групп. Заявлено: {playersCount}, найдено: {foundGroups + 1}");

                        List<(int, int)> currentGroup;
                        BFS(array, visited, i, j, out currentGroup);
                        groups[foundGroups] = currentGroup;
                        foundGroups++;
                        
                        Debug.Log("groups" + groups.Count);
                        Debug.Log("groups[0]" +groups[0].Count);
                    }
                }
            }
        }

        private static void BFS(int[,] array, bool[,] visited, int startX, int startY, out List<(int, int)> group)
        {
            group = new List<(int, int)>();
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);
            int[] dx = { 1, -1, 0, 0 };
            int[] dy = { 0, 0, 1, -1 };
            Queue<(int, int)> queue = new Queue<(int, int)>();
            queue.Enqueue((startX, startY));
            visited[startX, startY] = true;

            while (queue.Count > 0)
            {
                var (x, y) = queue.Dequeue();
                group.Add((x, y));

                for (int dir = 0; dir < 4; dir++)
                {
                    int nx = x + dx[dir];
                    int ny = y + dy[dir];

                    if (nx >= 0 && nx < rows && ny >= 0 && ny < cols)
                    {
                        if (!visited[nx, ny] && array[nx, ny] == (int)MapItemType.TailPlayer)
                        {
                            visited[nx, ny] = true;
                            queue.Enqueue((nx, ny));
                        }
                    }
                }
            }
        }
    }
}