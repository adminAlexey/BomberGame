using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PatnFinder : MonoBehaviour
{
    public List<Vector3> PathToTarget;
    List<Node> CheckedNoodes = new List<Node>();
    List<Node> WaitingNoodes = new List<Node>();
    public GameObject Target;
    public LayerMask SolidLayer; 

    // Update is called once per frame
    void Update()
    {
        PathToTarget = GetPath(Target.transform.position);
    }

    public List<Vector3> GetPath(Vector3 target)
    {
        PathToTarget = new List<Vector3>();

        CheckedNoodes = new List<Node>();
        WaitingNoodes = new List<Node>();

        Vector3 StartPosition = new Vector3(Mathf.Round(transform.position.x),2, Mathf.Round(transform.position.z));
        Vector3 TargetPosition = new Vector3(Mathf.Round(Target.transform.position.x),2, Mathf.Round(Target.transform.position.z));

        if (StartPosition == TargetPosition) return PathToTarget;

        Node startNode = new Node(0, StartPosition, TargetPosition, null);
        CheckedNoodes.Add(startNode);
        WaitingNoodes.AddRange(GetNeighbourNodes(startNode));

        while(WaitingNoodes.Count > 0)
        {
            Node nodeToCheck = WaitingNoodes.Where(x => x.F == WaitingNoodes.Min(z => z.F)).FirstOrDefault();

            if(nodeToCheck.Position == TargetPosition)
            {
                return CalculatePathFromNode(nodeToCheck);
            }

            var walkable = Physics2D.OverlapCircle(nodeToCheck.Position, 0.1f, SolidLayer);
            if (!walkable)
            {
                WaitingNoodes.Remove(nodeToCheck);
                CheckedNoodes.Add(nodeToCheck);
            } else if (walkable)
            {
                WaitingNoodes.Remove(nodeToCheck);
                if(!CheckedNoodes.Where(x=> x.Position == nodeToCheck.Position).Any()){
                    CheckedNoodes.Add(nodeToCheck);
                    WaitingNoodes.AddRange(GetNeighbourNodes(nodeToCheck));
                }
            }
        }

        return PathToTarget;
    }

    public List<Vector3> CalculatePathFromNode(Node node)
    {
        var path = new List<Vector3>();
        Node currentNode = node;
        while(currentNode.PreviousNode != null)
        {
            path.Add(new Vector3 (currentNode.Position.x,2,currentNode.Position.z));
            currentNode = currentNode.PreviousNode;
        }

        return null;
    }

    List<Node> GetNeighbourNodes (Node node)
    {
        var Neighbours = new List<Node>();

        Neighbours.Add(new Node(node.G + 1, 
            new Vector3(node.Position.x-1,2, node.Position.z),
            node.TargetPosition, 
            node));
        Neighbours.Add(new Node(node.G + 1,
            new Vector3(node.Position.x+1, 2, node.Position.z),
            node.TargetPosition,
            node));
        Neighbours.Add(new Node(node.G + 1,
            new Vector3(node.Position.x, 2, node.Position.z-1),
            node.TargetPosition,
            node));
        Neighbours.Add(new Node(node.G + 1,
            new Vector3(node.Position.x, 2, node.Position.z+1),
            node.TargetPosition,
            node));
        return Neighbours;
    }
    void OnDrawGizmos()
    {
        foreach(var item in CheckedNoodes)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(new Vector3(item.Position.x, 2, item.Position.z), 0.1f);
        }
        if(PathToTarget != null)
            foreach (var item in PathToTarget)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(new Vector3(item.x, 2, item.z), 0.2f);
            }
    }
}

public class Node
{
    public Vector3 Position;
    public Vector3 TargetPosition;
    public Node PreviousNode;
    public int F; //F=G+H
    public int G; //расстояние от старта до ноды
    public int H; //расстояние от ноды до цели


    public Node(int g, Vector3 nodePosition, Vector3 targetPosition, Node previousNode)
    {
        Position = nodePosition;
        TargetPosition = targetPosition;
        PreviousNode = previousNode;
        G = g;
        H = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.z - Position.z);
        F = G + H;
    }
}
