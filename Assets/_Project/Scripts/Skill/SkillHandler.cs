public class SkillHandler : Singleton<SkillHandler>
{
    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GiveRandomSkill()
    {
        
    }
}

