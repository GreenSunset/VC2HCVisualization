using UnityEngine;
using UnityEngine.UI;

public class TransparencyController : MonoBehaviour
{
    public GameObject transparencyLayer;
    public Scrollbar scrollbar;
    private Graph graph;
    void Start()
    {
        graph = GetComponent<Graph>();
        transparencyLayer.transform.localScale = new Vector3(Camera.main.orthographicSize * 2 * Camera.main.aspect, Camera.main.orthographicSize * 2, 1);
    }

    public void UpdateTransparency() {
        float value = (scrollbar.value - .5f) * 2;
        Color color = transparencyLayer.GetComponent<SpriteRenderer>().color;
        transparencyLayer.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, Mathf.Abs(value));
        if (graph != null) graph.ToFront(value > 0);
    }
}
