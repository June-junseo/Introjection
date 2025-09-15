public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData data)
    {
        if (data.projectileCount > 0)
        {
            return new DaggerSkill();
        }

        return null;
    }
}
