using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
	public enum MonsterStates
	{
		Idle,
		Patrol,
		Attack,
	}

	MonsterStates state;

	[SerializeField] float maxDistance;
	[SerializeField] float waitTimeMax;
	[SerializeField] float coneOfVisionAngle;
	[SerializeField] float rangeOfVision;
	[SerializeField] float rangeOfVisionClose;
	[SerializeField] float aggroTimeMax;
	[SerializeField] float patrolSpeed;
	[SerializeField] float chaseSpeed;
	[SerializeField] AudioSource monsterSound;
	[SerializeField] AudioSource monsterAttackSound;
	float waitTime;
	float aggroTime;
	Vector3 loc;

	NavMeshAgent agent;
	Animator anim;

	bool lostSight = true;
	[SerializeField] Player player;

    // Start is called before the first frame update
    void Start()
    {
		state = MonsterStates.Idle;
		agent = GetComponent<NavMeshAgent>();
		waitTime = waitTimeMax;
		aggroTime = aggroTimeMax;
		anim = GetComponent<Animator>();
    }


	// Update is called once per frame
	void Update()
    {
		// make sure the monster can only be in one state at a time
		switch (state)
		{
			case MonsterStates.Idle:
			{
				anim.SetBool("IsMoving", false);
				CheckVision();
				Idle();
				break;
			}
			case MonsterStates.Patrol:
			{
				anim.SetBool("IsMoving", true);
				CheckVision();
				Patrol();
				break;
			}
			case MonsterStates.Attack:
			{
				anim.SetBool("IsMoving", true);
				Attack();
				CheckVision();
				break;
			}
		}
	}

	void Idle()
	{
		if (monsterSound.isPlaying)
		{
			monsterSound.Stop();
		}
		if (monsterAttackSound.isPlaying)
		{
			monsterAttackSound.Stop();
		}
		// pick a random location around the player
		waitTime -= Time.deltaTime;
		// stop all movement once transitioned into the new state
		agent.SetDestination(transform.position);
		if (waitTime <= 0)
		{
			loc = FindRandomLocation();
			waitTime = waitTimeMax;
			agent.SetDestination(loc);
			state = MonsterStates.Patrol;
		}

	}

	void Patrol()
	{
		if (monsterSound.isPlaying == false)
		{
			monsterSound.Play();
		}
		if (monsterAttackSound.isPlaying)
		{
			monsterAttackSound.Stop();
		}
		// check to see if we've reached destination
		float dist = Vector3.Distance(loc, transform.position);
		if (dist <= 10.0f)
		{
			state = MonsterStates.Idle;
			agent.SetDestination(transform.position);
			agent.speed = patrolSpeed;
		}
	}

	void Attack()
	{
		if (monsterSound.isPlaying)
		{
			monsterSound.Stop();
		}
		if (monsterAttackSound.isPlaying == false)
		{
			monsterAttackSound.Play();
		}
		// move to player, also play scary music
		//Debug.Log("Attacking");

		agent.SetDestination(player.gameObject.transform.position);
		agent.speed = chaseSpeed;

		if (lostSight == true)
		{
			aggroTime -= Time.deltaTime;

			if (aggroTime <= 0)
			{
				state = MonsterStates.Idle;
				aggroTime = aggroTimeMax;

			}
		}

	}

	Vector3 FindRandomLocation()
	{
		// check for a point somewhere in a sphere and scale it by the max distance we want our monster to move
		Vector3 rand = Random.insideUnitSphere * maxDistance;
		// add the players position so it picks points around the player
		rand += player.gameObject.transform.position;
		// fire a navmeshresult out and get a point on the nav mesh
		NavMeshHit result;
		if (NavMesh.SamplePosition(rand, out result, maxDistance, 1))
		{
			return result.position;
		}
		else
		{
			// if for some reason we can't find an actual location, find another one
			return FindRandomLocation();
		}
	}

	void CheckVision()
	{
		// check the angle between the player and the monster
		Vector3 dir = player.gameObject.transform.position - transform.position;
		float angle = Vector3.Angle(dir, transform.forward);
		// get the distance from the monster and player
		float dist = Vector3.Distance(player.gameObject.transform.position, transform.position);

		// both these together form a cone so make sure both are true then set the monster to it's aggro state
		if ((angle < coneOfVisionAngle && dist < rangeOfVision) || dist < rangeOfVisionClose)
		{
			// trigger the attack state and set the move direction to the current player

			state = MonsterStates.Attack;
			lostSight = false;
		}
		else
		{
			lostSight = true;
		}
	}

	public void InvestigatePlayer(Vector3 playerLoc_)
	{
		agent.SetDestination(playerLoc_);
	}

}
