using UnityEngine;
using UnityEngine.UI;

public class NPCsController : MonoBehaviour
{
    public Text stateLabel;
    public NpcController selectedNpc;
    private Camera _cam;

    private void Start()
    {
        _cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // Unselect the current NPC:
        if (selectedNpc != null)
        {
            selectedNpc.stateLabel = null;
            selectedNpc.GetComponent<Renderer>().material.color = new Color(0, 255, 0);
            selectedNpc = null;
            stateLabel.text = "Null";
        }

        // Check if clicked on a NPC:
        var hit = Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out var hitInfo);
        if (!hit || !hitInfo.transform.gameObject.CompareTag("NPC")) return;

        // Select the clicked NPC:
        selectedNpc = hitInfo.transform.gameObject.GetComponent<NpcController>();
        selectedNpc.stateLabel = stateLabel;
        selectedNpc.GetComponent<Renderer>().material.color = new Color(0, 0, 255);
    }
}