using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    public GameObject edgePrefab;
    public Dictionary<int, GameObject> vertices = new Dictionary<int, GameObject>();
    public Dictionary<(int, int), GameObject> edges = new Dictionary<(int, int), GameObject>();
    public List<int> vertexCover = new List<int>();
    private int count = 0;
    public bool VCtoFront = true;
    private readonly List<Color> colors = new List<Color> { 
        Color.cyan,
        Color.green,
        Color.magenta,
        Color.red,
        Color.yellow, 
        Color.blue,
    };
    public int AddVertex(GameObject vertex) {
        vertex.GetComponent<VertexController>().ToFront(VCtoFront);
        count++;
        vertices.Add(count, vertex);
        vertex.GetComponent<VertexController>().vertexName = count;
        vertex.GetComponent<VertexController>().graph = this;
        // Add vertex to graph
        return count;
    }

    public void ToggleEdge(int vertex1, int vertex2) {
        if (vertex1 == vertex2) return;
        if (edges.ContainsKey((vertex1, vertex2))) {
            Destroy(edges[(vertex1, vertex2)]);
            edges.Remove((vertex1, vertex2));
        } else if (edges.ContainsKey((vertex2,vertex1))) {
            Destroy(edges[(vertex2, vertex1)]);
            edges.Remove((vertex2, vertex1));
        } else {
            GameObject edgeInstance = Instantiate(edgePrefab, new Vector3(0, 0, 0), Quaternion.identity);
            LineRenderer edgeLine = edgeInstance.GetComponent<LineRenderer>();
            
            edgeLine.SetPosition(0, vertices[vertex1].transform.position);
            edgeLine.SetPosition(1, vertices[vertex2].transform.position);

            Vector3 direction = vertices[vertex2].transform.position - vertices[vertex1].transform.position;
            Transform ctComponent = edgeInstance.GetComponent<CoverTestingController>().component;
            ctComponent.localRotation = Quaternion.AngleAxis(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg,   Vector3.forward);ctComponent.position = (vertices[vertex1].transform.position + vertices[vertex2].transform.position) / 2;

            edges.Add((vertex1, vertex2), edgeInstance);
            edgeInstance.GetComponent<CoverTestingController>().ToFront(VCtoFront);
            componentCircuit((vertex1,vertex2));
        }
        vertices[vertex1].GetComponent<VertexController>().UpdateAdjacency(vertex2);
        vertices[vertex2].GetComponent<VertexController>().UpdateAdjacency(vertex1);
    }

    public void DeleteVertex(int vertexName) {
        Destroy(vertices[vertexName]);
        vertices.Remove(vertexName);
        List<(int, int)> edgesToDelete = new List<(int, int)>();
        foreach ((int, int) edge in edges.Keys) {
            if (edge.Item1 == vertexName || edge.Item2 == vertexName) {
                edgesToDelete.Add(edge);
            }
        }
        foreach ((int, int) edge in edgesToDelete) {
            Destroy(edges[edge]);
            edges.Remove(edge);
            if (edge.Item1 == vertexName) {
                vertices[edge.Item2].GetComponent<VertexController>().UpdateAdjacency(edge.Item1);
            } else {
                vertices[edge.Item1].GetComponent<VertexController>().UpdateAdjacency(edge.Item2);
            }
        }
    }

    public void ToggleVertexCover(int vertexName) {
        if (vertexCover.Contains(vertexName)) {
            vertexCover.Remove(vertexName);
            vertices[vertexName].GetComponent<VertexController>().ClearColor();
        } else {
            vertexCover.Add(vertexName);
            vertices[vertexName].GetComponent<VertexController>().SetColor(colors[(vertexCover.Count - 1) % colors.Count]);
        }
        foreach ((int, int) edge in edges.Keys) {
            componentCircuit(edge);
        }
    }

    private void componentCircuit((int, int) edge) {
        int indexLeft = vertexCover.IndexOf(edge.Item1);
        int indexRight = vertexCover.IndexOf(edge.Item2);
        CoverTestingController controller = edges[edge].GetComponent<CoverTestingController>();
        if (indexRight != -1 && indexLeft != -1) {
            controller.ChangeConfig(3);
            controller.SetColor(colors[indexLeft % colors.Count], false);
            controller.SetColor(colors[indexRight % colors.Count], true);
        } else if (indexRight != -1) {
            controller.ChangeConfig(2);
            controller.SetColor(colors[indexRight % colors.Count], true);
        } else if (indexLeft != -1) {
            controller.ChangeConfig(1);
            controller.SetColor(colors[indexLeft % colors.Count], false);
        } else {
            controller.ChangeConfig();
        }
    }

    public bool IsVertexCover() {
        foreach ((int, int) edge in edges.Keys) {
            if (!vertexCover.Contains(edge.Item1) && !vertexCover.Contains(edge.Item2))
                return false;
        }
        return true;
    }

    public void ToFront(bool toFront) {
        VCtoFront = toFront;
        foreach (GameObject vertex in vertices.Values) {
            vertex.GetComponent<VertexController>().ToFront(toFront);
        }
        foreach (GameObject edge in edges.Values) {
            edge.GetComponent<CoverTestingController>().ToFront(toFront);
        }
    }
}
