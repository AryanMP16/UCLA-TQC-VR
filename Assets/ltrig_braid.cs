using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

//NOTE: cone has radius 1

public class ltrig_braid : MonoBehaviour
{
    public const int _1_ON_TOP = 0;
    public const int _2_ON_TOP = 1;

    public InputActionReference triggerValueAction;
    public const float debounceDelay = 0.5f;
    public const float duration = 0.5f;
    private volatile float lastTriggerTime = -Mathf.Infinity;
    public LineRenderer strand_1;
    public LineRenderer strand_2;
    private List<Vector3> s1_points = new List<Vector3>();
    private List<Vector3> s2_points = new List<Vector3>();

    void Start() {
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
                StartCoroutine(braid(braid_index, _1_ON_TOP));
                lastTriggerTime = Time.time;
                braid_index++;
            }
        }
    }
    
    IEnumerator braid(int index, int ordering) {
        float elapsed = 0f;
        while (elapsed < duration)
        {
	    elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Vector3 newPos1 = new Vector3(0.5f * Mathf.Cos(Mathf.PI * t), t + braid_index - 1, Mathf.Pow(-1.0f, index) * 0.5f * Mathf.Sin(Mathf.PI * t));
	    Vector3 newPos2 = new Vector3(-0.5f * Mathf.Cos(Mathf.PI * t), t + braid_index - 1, Mathf.Pow(-1.0f, index) * 0.4f * Mathf.Sin(Mathf.PI * t));

	    Debug.Log("Ordering: " + ordering);

            s1_points.Add(newPos1);
	    s2_points.Add(newPos2);
            strand_1.positionCount = s1_points.Count;
	    strand_2.positionCount = s2_points.Count;
            strand_1.SetPositions(s1_points.ToArray());
	    strand_2.SetPositions(s2_points.ToArray());

            yield return null;
        }
    }
}
