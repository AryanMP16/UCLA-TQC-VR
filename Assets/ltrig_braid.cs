using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

//NOTE: cone has radius 1

public class ltrig_braid : MonoBehaviour
{
    public const int _1_ON_TOP = 0;
    public const int _2_ON_TOP = 1;

    public GameObject braid_parent;

    public InputActionReference triggerValueAction;
    public const float debounceDelay = 0.5f;
    public const float duration = 0.5f;
    private volatile float lastTriggerTime = -Mathf.Infinity;
    public LineRenderer strand_1;
    public LineRenderer strand_2;
    private List<Vector3> s1_points = new List<Vector3>();
    private List<Vector3> s2_points = new List<Vector3>();

    private Color c1 = new Color(0.3f, 0.039f, 0.47f, 1.0f);
    private Color c2 = new Color(0.75f, 0.26f, 0.64f, 1.0f);

    void Start() {
        strand_1.SetColors(c1, c1);
        strand_2.SetColors(c2, c2);

        strand_1.positionCount = 0;
        strand_2.positionCount = 0;
    }

    void OnEnable() {
        if (triggerValueAction != null)
            triggerValueAction.action.Enable();
    }

    void OnDisable() {
        if (triggerValueAction != null)
            triggerValueAction.action.Disable();
	}

    int braid_index = 0;
    void Update()
    {
        if (triggerValueAction != null) {
            float value = triggerValueAction.action.ReadValue<float>();
            if (value > 0.1f && Time.time - lastTriggerTime > debounceDelay) {
                MakeNewBraid(braid_index, _1_ON_TOP);
                lastTriggerTime = Time.time;
                braid_index++;
            }
        }
    }

    void MakeNewBraid(int idx, int ordering){
        if (s1_points.Count > 0 && s2_points.Count > 0)
        {
            GameObject oldA = Instantiate(strand_1.gameObject, braid_parent.transform);
            GameObject oldB = Instantiate(strand_2.gameObject, braid_parent.transform);
            LineRenderer lrA = oldA.GetComponent<LineRenderer>();
            LineRenderer lrB = oldB.GetComponent<LineRenderer>();

            lrA.positionCount = s1_points.Count;
            lrA.SetPositions(s1_points.ToArray());
            lrB.positionCount = s2_points.Count;
            lrB.SetPositions(s2_points.ToArray());
        }

        s1_points.Clear();
        s2_points.Clear();
        strand_1.positionCount = 0;
        strand_2.positionCount = 0;

        if (idx % 2 == 0) {
            strand_1.SetColors(c1, c1);
            strand_2.SetColors(c2, c2);
        }
        else {
            strand_2.SetColors(c1, c1);
            strand_1.SetColors(c2, c2);
        }

        StartCoroutine(braid(idx, ordering));
    }

    
    IEnumerator braid(int index, int ordering) {
        float elapsed = 0f;
        int ind = 0;
        while (elapsed < duration) {
            //Debug.Log(ind);
            if (ind != 0) { //this is unbelievably stupid but it works
	            elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                Vector3 newPos1 = new Vector3(0.5f * Mathf.Cos(Mathf.PI * t), t + braid_index, Mathf.Pow(-1.0f, index) * 0.5f * Mathf.Sin(Mathf.PI * t));
	            Vector3 newPos2 = new Vector3(-0.5f * Mathf.Cos(Mathf.PI * t), t + braid_index, Mathf.Pow(-1.0f, index) * 0.33f * Mathf.Sin(Mathf.PI * t));

                s1_points.Add(newPos1);
	            s2_points.Add(newPos2);
                strand_1.positionCount = s1_points.Count;
	            strand_2.positionCount = s2_points.Count;
                strand_1.SetPositions(s1_points.ToArray());
	            strand_2.SetPositions(s2_points.ToArray());
            }
            ind++;
            yield return null;
        }
    }
}
