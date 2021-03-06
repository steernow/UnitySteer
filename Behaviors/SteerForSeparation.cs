using UnityEngine;

/// <summary>
/// Steers a vehicle to keep separate from neighbors
/// </summary>
[AddComponentMenu("UnitySteer/Steer/... for Separation")]
[RequireComponent(typeof(SteerForNeighborGroup))]
public class SteerForSeparation : SteerForNeighbors
{

	/// <summary>
	/// The comfort distance. Any neighbors closer than this will be hit with an
	/// extra penalty.
	/// </summary>
	[SerializeField]
	float _comfortDistance = 1;

	/// <summary>
	/// How much of a multiplier is applied to neighbors that are inside our
	/// comfort distance.  Defaults to 1 so that we don't change the behavior
	/// of already-created boids.
	/// </summary>
	[SerializeField]
	float _multiplierInsideComfortDistance = 1;

	float _comfortDistanceSquared;


	public float ComfortDistance
	{
		get { return _comfortDistance; }
		set 
		{ 
			_comfortDistance = value;
			_comfortDistanceSquared = _comfortDistance * _comfortDistance;
		}
	}

	protected override void Start()
	{
		_comfortDistanceSquared = _comfortDistance * _comfortDistance;
	}


	#region Methods
	public override Vector3 CalculateNeighborContribution(Vehicle other)
	{
		// add in steering contribution
		// (opposite of the offset direction, divided once by distance
		// to normalize, divided another time to get 1/d falloff)
		Vector3 offset = other.Position - Vehicle.Position;

		var offsetSqrMag = offset.sqrMagnitude;
		Vector3 steering = (offset / -offsetSqrMag);	
		if (_multiplierInsideComfortDistance != 1 && offsetSqrMag < _comfortDistanceSquared)
		{
			steering *= _multiplierInsideComfortDistance;
		}
		return steering;
	}
	#endregion
}

