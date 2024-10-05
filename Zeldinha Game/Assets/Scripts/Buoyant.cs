using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Buoyant : MonoBehaviour
{
	[Header("Drag properties")]
	[SerializeField] private float UnderwaterDrag = 3f;
	[SerializeField] private float UnderwaterAngularDrag = 1f;
	[SerializeField] private float AirDrag = 0f;
	[SerializeField] private float AirAngularDrag = 0.05f;

	[Space]
	[SerializeField] private float OceanYPos;
	[SerializeField] private float BuoyancyForce = 10f;

	// Internal fields
	private Rigidbody _rb;
	private bool _hasTouchedWater;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
		// Check if underwater
        float diffY = transform.position.y - OceanYPos;
		bool isUnderwater = diffY < 0;
		if (isUnderwater)
		{
			_hasTouchedWater = true;
		}

		// Ignore if never touched water
		if (!_hasTouchedWater) return;

		Vector3 forceVector = Vector3.up * BuoyancyForce * -diffY;
		_rb.AddForce(forceVector, ForceMode.Acceleration);
		_rb.drag = isUnderwater ? UnderwaterDrag : AirDrag;
		_rb.angularDrag = isUnderwater ? UnderwaterAngularDrag : AirAngularDrag;
    }
}
