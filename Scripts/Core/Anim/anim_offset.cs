using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anim_offset : MonoBehaviour {
	private Animator animator;
	public float offset;
    public float offset_alt;
    public float speed = 1f;
	public bool random;
	public bool random_speed;

	void Awake () {
		animator = this.GetComponent<Animator>();
		if (random)
		{
			offset = Random.Range(0f, 1f);
			if (random_speed)
			{
				speed = Random.Range(0f, 1f);
			}
		}
		if (hasParameter("offset"))
		{
			animator.SetFloat("offset", offset);
			var hash = animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
			animator.Play(hash, 0, offset);
		}
		if (hasParameter("offset_alt")) animator.SetFloat("offset_alt", offset_alt);
        if (hasParameter("speed")) animator.SetFloat("speed", speed);
		//animator.Rebind();
	}

	bool hasParameter(string paramName)
	{
		foreach (AnimatorControllerParameter param in animator.parameters)
		{
			if (param.name == paramName) return true;
		}
		return false;
	}
}
