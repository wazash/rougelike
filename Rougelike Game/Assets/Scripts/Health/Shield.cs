namespace Healths
{
    public class Shield
    {
        private int currentShield;

        public int CurrentShield { get => currentShield; }

        public Shield()
        {
            currentShield = 0;
        }

        public void AddShield(int shieldAmount) => currentShield += shieldAmount;

        public void RemoveShield(int shieldAmount)
        {
            currentShield -= shieldAmount;

            if (currentShield <= 0)
            {
                currentShield = 0;
            }
        }
    }
}
