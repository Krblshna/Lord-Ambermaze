namespace LordAmbermaze.Core
{
	public interface IHealthUI
	{
		void UpdateHP(int currentHealth);
		void Init(int maxHealth);
        void Hide();
    }
}