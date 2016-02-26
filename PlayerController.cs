using UnityEngine;

public class PlayerController : MonoBehaviour {
	//Get values from the singleton to avoid unnesessary static reads;
	float bounderies = GameBuilder.horizontalBounderies;
	float speed = GameBuilder.handleSpeed;

	bool releaseBall = true;

	void FixedUpdate () {
		MoveHandle ();
		ReleaseBall ();

	}

	/// <summary>Moves the players handle left and right using system input </summary>
	void MoveHandle(){
		float x = transform.position.x;
		if (Mathf.Abs (x) <= bounderies)
			transform.Translate (Input.GetAxis ("Horizontal") * (speed / 10), 0f, 0f);
		else {
			int modulo = (x > 0) ? 1 : -1;
			transform.position = Vector3.right * (bounderies * modulo);
		}
	}
	/// <summaryStart the game when spacebar is pressed </summary>
	void ReleaseBall(){
		if (releaseBall) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				releaseBall = false;
				transform.GetChild (0).SetParent (transform.parent);
				BallBehaviour.StartIt ();
			}

		}
	}

}
