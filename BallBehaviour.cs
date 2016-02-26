using UnityEngine;
using System.Collections;

public class BallBehaviour : MonoBehaviour {
	public float linearSpeed = GameBuilder.speed / 10;
	private Vector2 speed = Vector2.up;
	public static bool isMoving = false;
	float bounderies = GameBuilder.horizontalBounderies;
	Renderer rendererRef;

	void Start () {
		speed *= linearSpeed;
		rendererRef = GetComponent<Renderer> ();
	}

	void Update(){
		ChangeBallColor ();
//		print (speed.magnitude);
	}

	void FixedUpdate () {
		Reflect ();
		if (isMoving)
			Move ();
	}

	void OnTriggerEnter(Collider col){
		Vector3 relPos =  transform.position - col.transform.position;
		if (col.name == "Player") {
			float outOfCenter = Mathf.Clamp(relPos.x, -2f,2f) /2;
			speed.x = Mathf.Clamp (outOfCenter, -0.7f, 0.7f);
			speed.y = 1 - speed.x;
			speed = linearSpeed * speed.normalized;
		}
			
		else{
			speed *= -1;
			speed.x *= Random.Range (.7f, 1);
			speed = speed.normalized * linearSpeed;
		}
	}

	void Move(){
		transform.Translate (speed.x, speed.y, 0f);
	}

	public static void StartIt(){
		isMoving = true;
	}

	void Reflect(){
		if (Mathf.Abs(transform.position.x) -1.5f > bounderies)
			speed.x *= -1;
		if (transform.position.y > 10) {
			speed.y *= -1;
		}
	}

	void ChangeBallColor(){
		float a = Mathf.Clamp (transform.position.y / 10, .3f, .7f);
		rendererRef.material.color = Color.Lerp (GameBuilder.baseColor, GameBuilder.secondaryColor, a);
	}

	public void SetBallSpeedDirection(Vector2 dir){
		speed = dir.normalized;
	}

	void OnDestroy(){
		isMoving = false;
	}


}
