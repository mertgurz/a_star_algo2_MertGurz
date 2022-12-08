using UnityEngine;


public class Node : IHeapItem<Node> {

    public bool walkable;
    public Vector3 worldPosition;           
    public int gridX, gridY;
    int heapIndex;

    public int gCost, hCost;
    public Node parent;
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)          // degerleri girebilmek icin constructor  
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;                                     // node kendi gridX ve gridY pozisyonunu tutuyor (neighbour bulmakta faydali)
        gridY = _gridY;
    }
    public int fCost
    {
        get { return gCost + hCost; }
    }
    public int HeapIndex
    {
        get { 
            return heapIndex; 
        }
        set { heapIndex = value; 
        }
    }
    public int CompareTo(Node nodeToCompare)                        // HeapIndex ve CompareTo uygulamak zorundayiz (bu class Heap interfaceden turuyor)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0) {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
