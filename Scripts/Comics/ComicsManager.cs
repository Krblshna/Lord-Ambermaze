using System.Collections;
using System.Collections.Generic;
using AZ.Core;
using LordAmbermaze.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Playables;

public class ComicsManager : MonoBehaviour
{
	public bool onStart;
	public float onStartDelay = 1;
	IEnumerator process_en;
	bool processActive;
	Animator anim;
	bool wait_for_click, finish_comics, active;
	public float wait_time = 4f;
	[SerializeField] private UnityEvent _event;

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		anim = GetComponent<Animator>();
        CommonBlocker.Block(CommonBlocks.Comics);
		if (onStart)
		{
			yield return new WaitForSeconds(onStartDelay);
			start_comics();
		}
	}

	public void start_comics()
	{
		activate();
		next_scene();
	}

	void activate()
	{
		active = true;
	}

	public void Update()
	{
		if ((InputManagerC.GetButtonDown(CommonButtons.Use) || InputManagerC.GetButtonDown(CommonButtons.Jump)) && active)
		{
			if (wait_for_click)
			{
				next_scene();
			}
			if (finish_comics)
			{
				finish_comics = false;
				Utils.Instance.setTimeOut(() =>
                {
                    CommonBlocker.Unblock(CommonBlocks.Comics);
					_event?.Invoke();
                }, 2);
			}
		}
	}

	void stop_auto_click()
	{
		if (process_en != null) StopCoroutine(process_en);
		process_en = null;
	}

	public void next_scene()
	{
		stop_auto_click();
		var stage = anim.GetInteger("stage");
		anim.SetInteger("stage", stage + 1);
		wait_for_click = false;
	}

	public void stage_finished(int force_click)
	{
		wait_for_click = true;
		switch (force_click)
		{
			case 0:
				{
					process_en = auto_click();
					StartCoroutine(process_en);
					break;
				}
			case 1:
				{
					break;
				}
			case 2:
				{
					finish_comics = true;
					break;
				}
		}
	}

	IEnumerator auto_click()
	{
		yield return new WaitForSeconds(wait_time);
		next_scene();
	}
}
