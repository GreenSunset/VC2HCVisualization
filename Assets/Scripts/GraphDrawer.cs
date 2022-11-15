using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDrawer : MonoBehaviour
{
    private Dictionary<(int, int), int> positions = new Dictionary<(int, int), int>();
    public GameObject selectedVertexPrefab;
    public GameObject hoverVertexPrefab;
    public GameObject vertexPrefab;
    private GameObject hoverVertexInstance;
    private GameObject selectedVertexInstance;
    private Graph graph;
    private (int, int) selectedVertex;
    
    void Start()
    {
        hoverVertexInstance = Instantiate(hoverVertexPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        hoverVertexInstance.SetActive(true);
        selectedVertexInstance = Instantiate(selectedVertexPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        selectedVertexInstance.SetActive(false);
        graph = GetComponent<Graph>();
    }

    void Update() {
        // Mouse screen position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Convert to grid position
        int x = Mathf.RoundToInt(mousePos.x);
        int y = Mathf.RoundToInt(mousePos.y);
        // Update hover vertex position
        hoverVertexInstance.transform.position = new Vector3(x, y, 1);
        // select vertex
        if (Input.GetMouseButtonDown(0)) {
            // Check if vertex exists
            if (positions.ContainsKey((x, y))) {
                // Select vertex
                selectedVertexInstance.transform.position = new Vector3(x, y, 1);
                selectedVertexInstance.SetActive(true);
                selectedVertex = (x, y);
            } else {
                selectedVertexInstance.SetActive(false);
            }
        }
        // edit vertex
        if (Input.GetMouseButtonDown(1)) {
            // Create vertex if it doesn't exist
            if (!positions.ContainsKey((x, y))) {
                GameObject vertexInstance = Instantiate(vertexPrefab, new Vector3(x, y, 1), Quaternion.identity);
                int id = graph.AddVertex(vertexInstance);
                positions.Add((x, y), id);
            }
            // Check if vertex is selected
            if (selectedVertexInstance.activeSelf) {
                graph.ToggleEdge(positions[selectedVertex], positions[(x, y)]);
            }
        }
        // delete selected vertex
        if (Input.GetKeyDown(KeyCode.Delete)) {
            // Check if vertex is selected
            if (selectedVertexInstance.activeSelf) {
                // Delete vertex
                graph.DeleteVertex(positions[selectedVertex]);
                positions.Remove(selectedVertex);
                selectedVertexInstance.SetActive(false);
            }
        }
    }
}
