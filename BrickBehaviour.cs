using UnityEngine;

public class BrickBehaviour : MonoBehaviour {
	int mod;
	float speed = GameBuilder.bricksAnimSpeed / 1000;

	void Start () {
		mod = (Random.value > .5) ? 1 : -1;
		speed *= Random.Range (.7f, 1f);
	}

	void Update () {
		Ocilate ();
	}

	void OnTriggerEnter(Collider col){
	
		Destroy (gameObject);
		GameBuilder.destroyedBricks++;
	}

	void Ocilate(){
		if (mod > 0 && transform.position.z < .1f)
			transform.Translate (0f, 0f, speed * mod);
		else if(mod > 0){
			transform.position = new Vector3 (transform.position.x, transform.position.y, .1f);
			mod *= -1;
		}	

		if (mod < 0 && transform.position.z > -0.1f)
			transform.Translate (0f, 0f, speed * mod);
		else if(mod < 0){
			transform.position = new Vector3 (transform.position.x, transform.position.y, -0.1f);
			mod *= -1;
		}	
	}

}
