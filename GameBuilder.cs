using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public  class GameBuilder : MonoBehaviour {
	public static float horizontalBounderies = 5.45f;
	public static float handleSpeed = 5f;
	public static float bricksAnimSpeed = 8f;

	public static int destroyedBricks = 0;
//	bool isCompleteted = true;
	private void LevelComplete(){
		Destroy (gameGO);
		Build ();
	}
	public static float speed = 2f;

	public static readonly Color baseColor = Color.green;
	public static readonly Color secondaryColor = Color.blue;

	Vector3 lastBallPos;

	static GameObject gameGO ;
	GameObject ball;

	void Start () {
		Build();
		StartCoroutine("UnStuck");

	}
	
	
	void Update () {
		if (ball.transform.position.y < -1f || destroyedBricks >= 21 ) 
			Restart ();


	}
	void Restart(){
		destroyedBricks = 0;
		Destroy (gameGO);
		Build ();

	}

	void Build(){
		gameGO = new GameObject();
		//Player Assembly
		GameObject player = GameObject.CreatePrimitive (PrimitiveType.Cube);
		player.transform.position = Vector3.zero;
		player.transform.localScale = new Vector3 (4f, 1f, 1f);
		player.GetComponent<Renderer> ().material.color = Color.Lerp (baseColor, secondaryColor, .5f);
		player.GetComponent<Collider> ().isTrigger = true;
		player.AddComponent<PlayerController> ();
		player.name = "Player";
		player.GetComponent<BoxCollider> ().center = new Vector3 (0f, 0.45f);
		player.GetComponent<BoxCollider> ().size = new Vector3 (1f, .1f, 1f);

		//Camera Config
		Camera.main.transform.position = new Vector3(0f, 5f,-10f);
		Camera cam = Camera.main;
		cam.clearFlags = CameraClearFlags.SolidColor;
		cam.backgroundColor = new Color(.2f,.25f,.25f);
		//cam.transform.SetParent (gameGO.transform);

		//Light Creation
		GameObject light = new GameObject ();
		light.name = "light";
		light.AddComponent<Light> ();
		Light lightComponent = light.GetComponent<Light> ();
		lightComponent.type = LightType.Directional;
		lightComponent.color = new Color (.9f, .9f, .8f);
		light.transform.Rotate (45f, 0f, 0f);
		light.transform.SetParent (gameGO.transform);


		//Ball Config
		ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		ball.transform.position = new Vector3 (0f, 1.1f);
		ball.GetComponent<Renderer> ().material.color = Color.white;
		ball.AddComponent<BallBehaviour> ();
		ball.transform.SetParent (player.transform);
		ball.AddComponent<Rigidbody> ().isKinematic = true;

		player.transform.SetParent (gameGO.transform); // player is outplaced here to keep the ball as its parent



		//Brick Builder
		GameObject brick = GameObject.CreatePrimitive(PrimitiveType.Cube);
		brick.transform.localScale = new Vector3 (2f, 1f, 1f);
		brick.GetComponent<Collider> ().isTrigger = true;
		brick.AddComponent<BrickBehaviour> ();
		GameObject bricksParent = new GameObject ();
		bricksParent.name = "Bricks";

		GameObject[] bricks = new GameObject[21];

		int xPos = -4;
		int yPos = 9;
		for (int i = 0; i < bricks.Length; i++) {
			if (xPos < 3)
				xPos++;
			else {
				xPos = -3;
				yPos--;
			}
			Vector3 pos = new Vector3 (2.1f * xPos, 1.1f * yPos, Random.Range(-.1f,.1f) );
			bricks [i] = (GameObject)Instantiate (brick, pos, Quaternion.identity);
			bricks [i].GetComponent<Renderer> ().material.color = Color.Lerp (Color.blue, Color.green, Random.Range(.3f,.7f));
			bricks [i].transform.SetParent (bricksParent.transform);
		}
		bricksParent.transform.SetParent (gameGO.transform);
		Destroy (brick);

		lastBallPos = ball.transform.position;
	}
	
	///<summary>For misterious floathing point mathematical reasons, sometimes the ball gets stucked on the screen 
	///edges,. This coroutine handles it</summary>
	IEnumerator UnStuck(){
		//TODO: use dinamic values for the limits
		while (true) {
			if (BallBehaviour.isMoving) {
				bool isYStuck = (Mathf.Abs (lastBallPos.y - ball.transform.position.y) < 0.1) && 
					(Mathf.Abs(ball.transform.position.y) > 10f );
				bool isXStuck = (Mathf.Abs (lastBallPos.x - ball.transform.position.x) < 0.1) &&
					(ball.transform.position.x > 6.5f);

				if (isYStuck || isXStuck) {
					ball.transform.position = new Vector3 (0, 2f, 0f);
					ball.GetComponent<BallBehaviour> ().SetBallSpeedDirection (Vector2.up);
				}

				lastBallPos = ball.transform.position;

			}
			 yield return new WaitForSeconds(5f);
		}
	}
}
