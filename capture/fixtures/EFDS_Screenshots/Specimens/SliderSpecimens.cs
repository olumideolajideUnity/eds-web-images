using UnityEngine;
// Specimens for Components > Slider 'how to use': a Camera with only Field of View (slider), and a Transform whose Position
// is drawn as three sliders (the 'don't' example). Shown on a 'Main Camera' object whose real components are hidden.
namespace EFDS {
  public class Camera : MonoBehaviour { public float fieldOfView = 60; void OnEnable() {} }
  public class Transform : MonoBehaviour { public Vector3 position = Vector3.one; }
}
namespace EFDS {
  // Components > Progress bar overview: a 'Depth of field' component whose settings each show a slider and a progress bar.
  public class DepthOfField : MonoBehaviour { public float focusDistance = 10, focalLength = 50; void OnEnable() {} }
}
