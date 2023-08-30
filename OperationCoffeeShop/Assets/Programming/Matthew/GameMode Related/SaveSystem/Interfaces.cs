internal interface ISaveState
{
    void Save(int gameNumber);
    void Load(int gameNumber);
}
internal interface IDamageAndHealth
{
    public void TakeDamage(int damage);
    public void Heal(int health);
}


