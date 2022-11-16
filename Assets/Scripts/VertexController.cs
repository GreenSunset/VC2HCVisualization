using System.Collections.Generic;
using UnityEngine;

public class VertexController : MonoBehaviour
{
    // adjacency order
    private Color currentColor = Color.white;
    private List<int> adjacency = new List<int>();
    private List<LineRenderer> connections = new List<LineRenderer>();
    public int vertexName = 0;
    public GameObject connectionPrefab;
    public Graph graph;
    public void UpdateAdjacency(int changedEdge) {
        int index = adjacency.IndexOf(changedEdge);
        if (index == -1) {
            Vector3 direction = graph.vertices[changedEdge].transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            int i = 0;
            for (; i < adjacency.Count; i++) {
                Vector3 direction2 = graph.vertices[adjacency[i]].transform.position - transform.position;
                float angle2 = Mathf.Atan2(direction2.y, direction2.x) * Mathf.Rad2Deg;
                if (angle < angle2) {
                    break;
                }
            }
            if(adjacency.Count > connections.Count) {
                GameObject newConnection = Instantiate(connectionPrefab, transform);
                connections.Add(newConnection.GetComponent<LineRenderer>());
            }
            adjacency.Insert(i, changedEdge);
        } else {
            adjacency.RemoveAt(index);
            if (connections.Count > 0) {
                Destroy(connections[connections.Count - 1].gameObject);
                connections.RemoveAt(connections.Count - 1);
            }
        }
        UpdateConnections();
    }
    private void UpdateConnections() {
        for (int i = 1; i < adjacency.Count; i++) {
            Vector3 start, end;
            if (graph.edges.ContainsKey((vertexName, adjacency[i]))) {
                end = graph.edges[(vertexName, adjacency[i])].GetComponent<CoverTestingController>().exit06.position;
            } else {
                end = graph.edges[(adjacency[i], vertexName)].GetComponent<CoverTestingController>().entry07.position;
            }
            if (graph.edges.ContainsKey((vertexName, adjacency[i-1]))) {
                start = graph.edges[(vertexName, adjacency[i-1])].GetComponent<CoverTestingController>().entry01.position;
            } else {
                start = graph.edges[(adjacency[i-1], vertexName)].GetComponent<CoverTestingController>().exit12.position;
            }
            connections[i-1].SetPosition(0, start);
            connections[i-1].SetPosition(1, end);
            connections[i-1].startColor = currentColor;
            connections[i-1].endColor = currentColor;
        }
    }

    public void SetColor(Color color) {
        currentColor = color;
        GetComponent<SpriteRenderer>().color = color;
        foreach (LineRenderer connection in connections) {
            connection.startColor = color;
            connection.endColor = color;
        }
    }
    
    public void ClearColor() {
        GetComponent<SpriteRenderer>().color = Color.white;
        foreach (LineRenderer connection in connections) {
            connection.startColor = Color.white;
            connection.endColor = Color.white;
        }
    }

    public void ToFront(bool vc) {
        if (vc) {
            foreach (LineRenderer connection in connections) {
                Vector3 pos = connection.GetPosition(0);
                pos.z = 0.5f;
                connection.SetPosition(0, pos);
                pos = connection.GetPosition(1);
                pos.z = 0.5f;
                connection.SetPosition(1, pos);
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.5f);
        } else {
            foreach (LineRenderer connection in connections) {
                Vector3 pos = connection.GetPosition(0);
                pos.z = -0.5f;
                connection.SetPosition(0, pos);
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.5f);
        }
    }
}
