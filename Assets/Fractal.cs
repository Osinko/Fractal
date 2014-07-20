using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{
		public Mesh mesh;
		public Material material;

		public int maxDepth;
		int depth;
		public float childScale;

		void Start ()
		{
				gameObject.AddComponent<MeshFilter> ().mesh = mesh;
				gameObject.AddComponent<MeshRenderer> ().material = material;
				if (depth < maxDepth) {	
						StartCoroutine (CreateChildren ());
				}
		}

		static Vector3[] childDirection = {
		Vector3.up,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back,
		};

		static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler (0, 0, -90),
		Quaternion.Euler (0, 0, 90),
		Quaternion.Euler (90, 0, 0),
		Quaternion.Euler (-90, 0, 0),
		};

		IEnumerator CreateChildren ()
		{
				for (int i = 0; i < childDirection.Length; i++) {
						yield return new WaitForSeconds (0.5f);
						new GameObject ("Fractal Child").AddComponent<Fractal> ().
							Initialize (this, i);
				}
		}

		void Initialize (Fractal parent, int childIndex)
		{
				mesh = parent.mesh;
				material = parent.material;
				maxDepth = parent.maxDepth;
				depth = parent.depth + 1;
				childScale = parent.childScale;
				transform.parent = parent.transform;
				transform.localScale = Vector3.one * childScale;
				transform.localPosition = childDirection [childIndex] * (0.5f + 0.5f * childScale);
				transform.localRotation = childOrientations [childIndex];
		}
}
