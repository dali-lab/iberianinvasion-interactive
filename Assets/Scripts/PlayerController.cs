using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public Transform goal;
	public Animation anim;
	public NavMeshAgent agent;

	void Start() {
		agent = GetComponent<NavMeshAgent> ();

		anim = GetComponent<Animation> ();
	}

	void Update() {
		agent.destination = goal.position;

		float dist = agent.remainingDistance;
	
		if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0) {
			anim.Play ("idle1");
		} else {
			anim.Play ("walk");
		}
	}
	
}
