using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGizmos;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;                           // grid'i temsil eden 2 boyutlu array

    float nodeDiameter;
    int gridSizeX, gridSizeY;
    private void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);         // gridworldsize.x'e kac node sigabileceginin hesaplamasi
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);       // RoundToInt cunku yarim node olamaz
        CreateGrid();
    }
    private void Update()
    {
        if (StructureBuilder.mapHasChanged)
        {
            CreateGrid();
            StructureBuilder.mapHasChanged = false;
        }
    }
    public int MaxSize { 
        get { return gridSizeX * gridSizeY; }   // Heap icin gerekli total grid size hesaplamasi (pathfinding openSet initializationda kullaniliyor)
    }
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];             
        Vector3 worldBottomLeft = transform.position - Vector3.right * (gridWorldSize.x / 2) - Vector3.forward * (gridWorldSize.y / 2);
        
        for (int x = 0; x < gridSizeX; x++){                                // nodelarin olacagi her pozisyonda loop(collision kontrol icin) 
            for (int y = 0; y < gridSizeY; y++) {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);     //sol alttan baslayarak her node'un pozisyonu bulmak
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));  
                grid[x, y] = new Node(walkable, worldPoint, x, y);          //constructor'a degerleri aktariyoruz 
            }
        }
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neigbours = new List<Node>();
        for (int x = -1; x <= 1; x++)                   // node'un etrafinda 3x3 check 
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)           //  x & y 0 pozisyonu node'un kendisi olacagi icin skip
                    continue;

                int checkX = node.gridX + x;     // loop -1den basladigi icin node'un x ekseninde saga sola check yapiyor
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neigbours.Add(grid[checkX, checkY]);
           
            }
        }
        return neigbours;
    }
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = worldPosition.x / gridWorldSize.x + 0.5f;     // percent olarak: 0 en sol 0.5 orta 1 en sag
        float percentY = worldPosition.z / gridWorldSize.y + 0.5f;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

   // public List<Node> visualRepresentationOfPath;                     // bunu kullanarak asagidaki method'da path icin de Gizmo cizilebilir
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid != null && displayGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;                      // ternary ile node walkable mi check
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));       
            }
        }
    }
}
