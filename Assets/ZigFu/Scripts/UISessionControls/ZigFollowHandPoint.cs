using UnityEngine;
using System;
using OpenNI;

public class ZigFollowHandPoint : MonoBehaviour
{
	public Vector3 Scale = new Vector3(0.02f, 0.02f, -0.02f);
	public Vector3 bias;
	public float damping = 5;
//    public Vector3 bounds = new Vector3(10, 10, 10);
	public Vector3 circlePos = new Vector3(0.01f, 1.35f, 2.89f);
	public GameObject handCircle;
	public GameObject antModel;
	public Vector3 imageViewerBounds;

    Vector3 focusPoint;
	Vector3 desiredPos;


	void Start() {
		desiredPos = transform.localPosition;
		handCircle = GameObject.Find("HandCircle");
		imageViewerBounds = GameObject.Find("Image").GetComponent<Renderer>().bounds.size;
	}
	
	void Update() {
		transform.localPosition = Vector3.Lerp(transform.localPosition,  desiredPos, damping * Time.deltaTime);

		if (transform.localPosition == circlePos) {
			Debug.Log ("Hand positioned at circle");
			Color insideColor = new Color(192, 85, 85, 1);

			MeshRenderer gameObjectRenderer = handCircle.GetComponent<MeshRenderer> ();
			Material insideMaterial = new Material(Shader.Find("Standard"));

			insideMaterial.color = insideColor;
			gameObjectRenderer.material = insideMaterial;
		} else {
			Color outsideColor = new Color(232, 213, 213, 1);

			MeshRenderer gameObjectRenderer = handCircle.GetComponent<MeshRenderer>();
			Material outsideMaterial = new Material(Shader.Find("Standard"));

			outsideMaterial.color = outsideColor;
			gameObjectRenderer.material = outsideMaterial;

		}

	}

	void Session_Start(Vector3 focusPoint) {
        this.focusPoint = focusPoint;
	}
	
	void Session_Update(Vector3 handPoint) {
        Vector3 pos = handPoint - focusPoint;
        desiredPos = ClampVector(Vector3.Scale(pos, Scale) + bias, -0.5f * imageViewerBounds, 0.5f * imageViewerBounds);
	}
	
	void Session_End() {
        desiredPos = Vector3.zero;
	}

    Vector3 ClampVector(Vector3 vec, Vector3 min, Vector3 max) {
//        return new Vector3(Mathf.Clamp(vec.x, min.x, max.x),
//                           Mathf.Clamp(vec.y, min.y, max.y),
//                           Mathf.Clamp(vec.z, min.z, max.z));
		return new Vector3(Mathf.Clamp(vec.x, min.x, max.x),
		                   Mathf.Clamp(vec.y, min.y, max.y),
		                   0.74f);
    }
}