using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject edgePrefab;
    private Dictionary<int, GameObject> vertices = new Dictionary<int, GameObject>();
    private Dictionary<(int, int), GameObject> edges = new Dictionary<(int, int), GameObject>();
    public int AddVertex(GameObject vertex) {
        int id = vertices.Count + 1;
        vertices.Add(id, vertex);
        // Add vertex to graph
        return id;
    }
    public void ToggleEdge(int vertex1, int vertex2) {
        // if edge already exists, delete it
        if (edges.ContainsKey((vertex1, vertex2))) {
            Destroy(edges[(vertex1, vertex2)]);
            edges.Remove((vertex1, vertex2));
        } else {
            // Add edge to graph
            GameObject edgeInstance = Instantiate(edgePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            
            LineRenderer edgeLine = edgeInstance.GetComponent<LineRenderer>();
            // Add endpoints to line
            edgeLine.SetPosition(0, vertices[vertex1].transform.position);
            edgeLine.SetPosition(1, vertices[vertex2].transform.position);
            // Get rotation
            Vector3 direction = vertices[vertex2].transform.position - vertices[vertex1].transform.position;
            Transform ctComponent = edgeInstance.GetComponent<CoverTestingController>().component;
            ctComponent.localRotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,   Vector3.forward);ctComponent.position = (vertices[vertex1].transform.position + vertices[vertex2].transform.position) / 2;
            // Add edge to dictionary
            edges.Add((vertex1, vertex2), edgeInstance);
        }
    }

    public void DeleteVertex(int vertexName) {
        // Delete vertex from graph
        Destroy(vertices[vertexName]);
        vertices.Remove(vertexName);
        // Delete edges connected to vertex
        List<(int, int)> edgesToDelete = new List<(int, int)>();
        foreach ((int, int) edge in edges.Keys) {
            if (edge.Item1 == vertexName || edge.Item2 == vertexName) {
                edgesToDelete.Add(edge);
            }
        }
        foreach ((int, int) edge in edgesToDelete) {
            Destroy(edges[edge]);
            edges.Remove(edge);
        }
    }
}
