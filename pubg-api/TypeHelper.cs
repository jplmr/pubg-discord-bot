namespace DiscordBot {

public enum Perspective
{
    FirstPerson,
    ThirdPesron
}

public enum TeamSize
{
    Solo,
    Duo,
    Squad
}

static class TypeHelper
{
    public static Perspective? GetPerspective(string perspective)
    {
        switch (perspective.Trim().ToLower())
        {
            case "fpp":
            case "first":
            case "firstperson":
                return Perspective.FirstPerson;

            case "tpp":
            case "third":
            case "thirdperson":
                return Perspective.ThirdPesron;

            default:
                return null;
        }
    }

    public static string PerspectiveToString(Perspective? perspective)
    {
        if (!perspective.HasValue)
        {
            return null;
        }

        switch (perspective)
        {
            case Perspective.FirstPerson:
                return "FPP";

            case Perspective.ThirdPesron:
                return "TPP";

            default:
                return null;
        }
    }

    public static TeamSize? GetTeamSize(string teamSize)
    {
        switch (teamSize.Trim().ToLower())
        {
            case "solo":
                return TeamSize.Solo;
            case "duo":
                return TeamSize.Duo;
            case "squad":
                return TeamSize.Squad;
            default:
                return null;
        }
    }

    public static string TeamSizeToString(TeamSize? teamSize)
    {
        if (!teamSize.HasValue)
        {
            return null;
        }

        switch (teamSize.Value)
        {
            case TeamSize.Solo:
                return "solos";

            case TeamSize.Duo:
                return "duos";
            
            case TeamSize.Squad:
                return "squads";

            default:
                return null;
        }
    }

}

}