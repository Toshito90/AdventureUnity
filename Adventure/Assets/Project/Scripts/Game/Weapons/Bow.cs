using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
	public GameObject playerModel;
	public GameObject arrowPrefab;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Attack()
	{
		GameObject arrowObject = Instantiate(arrowPrefab);
		arrowObject.transform.position = playerModel.transform.position + playerModel.transform.forward;
		arrowObject.transform.forward = playerModel.transform.forward;
		
	}
}
