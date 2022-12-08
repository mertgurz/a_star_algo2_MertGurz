using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid _grid;
   
    private Heap<Node> openSet; 
    Vector3[] waypoints = new Vector3[0];       

    private void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        _grid = GetComponent<Grid>();
        openSet = new Heap<Node>(_grid.MaxSize);            // pathfinding icin heap optimizasyonlu node listesi
    }
 
    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        //Stopwatch sw = new Stopwatch();       // namespace'e System.Diagnostics eklenip pathfinding hiz testi yapilabilir
        //sw.Start();

        bool pathSuccessful = false;

        Node startNode = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);

        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

            while (openSet.Count > 0)                           // neighbour'a ilerleme costlari ve final path hesaplamasi
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccessful = true;
                    //sw.Stop();
                    break;
                }
                foreach (Node neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))       // neighbour check methodunu kullanirken neighbour node walkable degilse veya closed listteyse diger neighbourlarla devam 
                        continue;

                    int newMovementCostToNeighour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);            // neighbour movement costlarin hesaplamasi
                        neighbour.parent = currentNode;
                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                        else
                            openSet.UpdateItem(neighbour);
                    }
                }
            }
        openSet.Clear();                                            // openSet listesi garbage ile dolmamasi icin (memory leak'i engelliyor)
        yield return null;
      
        if (pathSuccessful)
            waypoints = RetracePath(startNode, targetNode);

        requestManager.FinishedProcessingPath(waypoints, pathSuccessful);
    }
    Vector3[] RetracePath(Node startNode, Node endNode)             // endNode'a ulastiginda array'i ters cevirdigimizde yurunecek esas son path olusuyor 
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        if (dstX>dstY)
            return 14 * dstY + 10 * (dstX - dstY);          //      14 diagonal distance cost 10 direct distance cost
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
