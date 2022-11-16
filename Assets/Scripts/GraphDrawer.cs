using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphDrawer : MonoBehaviour
{
    private Dictionary<(float, float), int> positions = new Dictionary<(float, float), int>();
    public GameObject selectedVertexPrefab;
    public GameObject hoverVertexPrefab;
    public GameObject vertexPrefab;
    public float gridSize = 2;
    public Text text;
    private GameObject hoverVertexInstance;
    private GameObject selectedVertexInstance;
    private Graph graph;
    private (float, float) selectedVertex;
    
    void Start()
    {
        hoverVertexInstance = Instantiate(hoverVertexPrefab, new Vector3(0, 0, -1), Quaternion.identity);
        hoverVertexInstance.SetActive(true);
        selectedVertexInstance = Instantiate(selectedVertexPrefab, new Vector3(0, 0, -1), Quaternion.identity);
        selectedVertexInstance.SetActive(false);
        graph = GetComponent<Graph>();
    }

    void Update() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float x = Mathf.Round(mousePos.x / gridSize) * gridSize;
        float y = Mathf.Round(mousePos.y / gridSize) * gridSize;
        hoverVertexInstance.transform.position = new Vector3(x, y, -1);
        // select vertex
        if (Input.GetMouseButtonDown(0)) {
            if (positions.ContainsKey((x, y))) {
                if (selectedVertexInstance.activeSelf && selectedVertex == (x, y)) {
                    selectedVertexInstance.SetActive(false);
                } else {
                    selectedVertexInstance.transform.position = new Vector3(x, y, -1);
                    selectedVertexInstance.SetActive(true);
                    selectedVertex = (x, y);
                }
            } else {
                selectedVertexInstance.SetActive(false);
            }
        }
        // edit vertex
        if (Input.GetMouseButtonDown(1)) {
            if (!positions.ContainsKey((x, y))) {
                GameObject vertexInstance = Instantiate(vertexPrefab, new Vector3(x, y, -0.5f), Quaternion.identity);
                int id = graph.AddVertex(vertexInstance);
                positions.Add((x, y), id);
            }
            if (selectedVertexInstance.activeSelf) {
                graph.ToggleEdge(positions[selectedVertex], positions[(x, y)]);
            }
            selectedVertexInstance.transform.position = new Vector3(x, y, -1);
            selectedVertexInstance.SetActive(true);
            selectedVertex = (x, y);
        }
        // delete selected vertex
        if (Input.GetKeyDown(KeyCode.D)) {
            if (selectedVertexInstance.activeSelf) {
                if(graph.vertexCover.Contains(positions[selectedVertex])) {
                    graph.ToggleVertexCover(positions[selectedVertex]);
                }
                graph.DeleteVertex(positions[selectedVertex]);
                positions.Remove(selectedVertex);
                selectedVertexInstance.SetActive(false);
            }
        }
        // add vertex to cover
        if (Input.GetKeyDown(KeyCode.A)) {
            if (selectedVertexInstance.activeSelf) {
                graph.ToggleVertexCover(positions[selectedVertex]);
                
            }
        }
        text.text = "VC Size / Selectors: " + graph.vertexCover.Count + "\nVertex Cover / Hamiltonian Circuit: " + (graph.IsVertexCover()? "Yes" : "No");
    }

    public void Quit() {
        Application.Quit();
    }
}
