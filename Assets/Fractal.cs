using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour
{
		public Mesh mesh;
		public Material material;

		public int maxDepth;
		int depth;
		public float childScale;

		Material[,] materials;

		//unityのバッチングレンダリングを利用するために深さを利用して色分けする
		void InitializeMaterials ()
		{
				materials = new Material[maxDepth + 1, 2];
				for (int i = 0; i <= maxDepth; i++) {
						float t = i / (maxDepth - 1f);
						t *= t;
						materials [i, 0] = new Material (material);
						materials [i, 0].color = Color.Lerp (Color.white, Color.yellow, t);
						materials [i, 1] = new Material (material);
						materials [i, 1].color = Color.Lerp (Color.white, Color.cyan, t);
				}
				//最期の要素は上書きで書き換えている
				materials [maxDepth, 0].color = Color.magenta;
				materials [maxDepth, 1].color = Color.red;
		}

		public Mesh[] meshes;
		public float maxRotationSpeed;
		public float maxTwist;
		float rotationSpeed;

		void Start ()
		{
				rotationSpeed = Random.Range (-maxRotationSpeed, maxRotationSpeed);
				transform.Rotate (Random.Range (-maxTwist, maxTwist), 0, 0);
				if (materials == null) {
						InitializeMaterials ();
				}
				gameObject.AddComponent<MeshFilter> ().mesh = meshes [Random.Range (0, meshes.Length)];
				gameObject.AddComponent<MeshRenderer> ().material = materials [depth, Random.Range (0, 2)];
				if (depth < maxDepth) {	
						StartCoroutine (CreateChildren ());
				}
		}

		void Update ()
		{
				transform.Rotate (0, rotationSpeed * Time.deltaTime, 0f);
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

		public float spawnProbability;

		IEnumerator CreateChildren ()
		{
				for (int i = 0; i < childDirection.Length; i++) {
						if (Random.value < spawnProbability) {
								yield return new WaitForSeconds (0.5f);
								new GameObject ("Fractal Child").AddComponent<Fractal> ().Initialize (this, i);
						}
				}
		}

		void Initialize (Fractal parent, int childIndex)
		{
				meshes = parent.meshes;
				materials = parent.materials;
				maxDepth = parent.maxDepth;
				depth = parent.depth + 1;
				childScale = parent.childScale;
				spawnProbability = parent.spawnProbability;
				maxRotationSpeed = parent.maxRotationSpeed;
				maxTwist = parent.maxTwist;
				transform.parent = parent.transform;
				transform.localScale = Vector3.one * childScale;
				transform.localPosition = childDirection [childIndex] * (0.5f + 0.5f * childScale);
				transform.localRotation = childOrientations [childIndex];
		}
}
