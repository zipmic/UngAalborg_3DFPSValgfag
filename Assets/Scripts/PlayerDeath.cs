using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{

    public Behaviour[] BehavioursToDisableOnDeath;
    private Rigidbody rb;

	private void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void KillPlayer()
    {
        foreach (Behaviour b in BehavioursToDisableOnDeath)
        {
            b.enabled = false;
        }
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(new Vector3(Random.Range(-1,1), 0, Random.Range(-1, 1))*5);


	}

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.Q))
        {
            KillPlayer();
        }
	}
}
