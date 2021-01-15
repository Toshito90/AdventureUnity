using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControlAnimations : MonoBehaviour
{
	private int animation_id;
	private int total_animations;
	private float animation_current_duration;
	private float animation_final_duration;
	private float animation_current_percentage;

    // Start is called before the first frame update
    void Start()
    {
		animation_id = 0;
		animation_current_duration = 0.0f;
		animation_final_duration = 0.0f ;
		animation_current_percentage = 0.0f;
		total_animations = 1;
	}

    // Update is called once per frame
    void Update()
    {
		OnProcess();
	}

	public virtual void Play_Animation( float _speed )
	{

		if (_speed <= 0.0f)
		{
			animation_final_duration = 1.0f;
		}
		else
		{
			animation_final_duration = _speed;
		}
		
		animation_current_duration = 0.0f;
		animation_current_percentage = 0.0f;

		animation_id = 1;
	}

	public virtual void Set_Animations_Total_Ids( int new_total_ids)
	{
		if( new_total_ids <= 0 )
		{
			total_animations = 1;
		}
		else
		{
			total_animations = new_total_ids;
		}
	}

	public virtual bool Is_Animating()
	{
		if( animation_id > 0)
		{
			return true;
		}

		return false;
	}

	protected virtual void OnProcess()
	{
		if (animation_id > 0)
		{
			animation_current_duration += Time.deltaTime;
			if (animation_current_duration >= animation_final_duration)
			{
				animation_current_duration = 0.0f;
				animation_current_percentage = 1.0F;
			}
			else
			{
				animation_current_percentage = animation_current_duration / animation_final_duration;
			}

			if (animation_current_percentage >= 1.0f)
			{
				animation_current_percentage = 0.0f;
				if( animation_id <= total_animations)
				{
					animation_id++;
				}
				else
				{
					animation_id = 0;
				}
			}

		}
	}
}
