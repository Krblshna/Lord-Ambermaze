using UnityEngine;

namespace LordAmbermaze.Projectiles
{
	public class ProjectileGraphic : MonoBehaviour, IProjectileGraphic
	{
		public void SetDirection(Vector2 direction)
		{
			float angle = Vector2.Angle(direction, Vector2.left);
            if (direction.y > 0)
            {
                angle *= -1;
            }
			float init_angle = 180;
			transform.Rotate(new Vector3(0,0, angle + init_angle));
		}
	}
}