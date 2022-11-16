using UnityEngine;

public class CoverTestingController : MonoBehaviour
{
    public Transform component;
    public Transform entry01;
    public Transform entry07;
    public Transform exit06;
    public Transform exit12;
    public GameObject configL;
    public GameObject configR;
    public GameObject configC;
    public LineRenderer lineCR;
    public LineRenderer lineCL;

    public void ChangeConfig(int config = 0) {
        switch (config) {
            case 1:
                configL.SetActive(true);
                configR.SetActive(false);
                configC.SetActive(false);
                break;
            case 2:
                configL.SetActive(false);
                configR.SetActive(true);
                configC.SetActive(false);
                break;
            case 3:
                configL.SetActive(false);
                configR.SetActive(false);
                configC.SetActive(true);
                break;
            default:
                configL.SetActive(false);
                configR.SetActive(false);
                configC.SetActive(false);
                break;
        }
    }

    public void SetColor(Color color, bool right) {
        if (right) {
            lineCR.startColor = color;
            lineCR.endColor = color;
            configR.GetComponent<LineRenderer>().startColor = color;
            configR.GetComponent<LineRenderer>().endColor = color;
        } else {
            lineCL.startColor = color;
            lineCL.endColor = color;
            configL.GetComponent<LineRenderer>().startColor = color;
            configL.GetComponent<LineRenderer>().endColor = color;
        }
    }

    public void ToFront(bool vc) {
        LineRenderer line = GetComponent<LineRenderer>();
        if (vc) {
            component.localPosition = new Vector3(component.localPosition.x, component.localPosition.y, 0.5f);
            Vector3 pos = line.GetPosition(0);
            pos.z = -0.5f;
            line.SetPosition(0, pos);
            pos = line.GetPosition(1);
            pos.z = -0.5f;
            line.SetPosition(1, pos);
        } else {
            component.localPosition = new Vector3(component.localPosition.x, component.localPosition.y, -0.5f);
            Vector3 pos = line.GetPosition(0);
            pos.z = 0.5f;
            line.SetPosition(0, pos);
            pos = line.GetPosition(1);
            pos.z = 0.5f;
            line.SetPosition(1, pos);
        }
    }
}
