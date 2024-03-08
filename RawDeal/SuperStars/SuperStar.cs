using RawDealView.Options;

namespace RawDeal.SuperStars;

public abstract class SuperStar
{
    public string Name;
    public string Logo;
    public byte HandSize;
    public byte SuperstarValue;
    public string SuperstarAbility;
    protected bool _useAbilityInTurn;

    public SuperStar(string name, string logo, byte handSize, byte superstarValue, string superstarAbility, bool useAbilityInTurn)
    {
        Name = name;
        Logo = logo;
        HandSize = handSize;
        SuperstarValue = superstarValue;
        SuperstarAbility = superstarAbility;
        _useAbilityInTurn = useAbilityInTurn;
    }

    public virtual void UseSuperStarAbility(Player player, Player opponent)
    {
        player.View.SayThatPlayerIsGoingToUseHisAbility(Name, SuperstarAbility);
        _useAbilityInTurn = true;   // This is used to know if the player can use his ability in the turn.
    }

    public virtual byte UseAbilityBeforeTakingACard(Player player, Player opponent)
    {
        _useAbilityInTurn = false;  // at the start of the turn, the player has not used his ability yet.
        return 1;
    }

    public virtual byte UseSuperStarAbilityReceivingDamage(byte damage)
        => damage;
    
    public virtual NextPlay ShowOptionsOfASpecificSuperStar(Player player)
        => player.View.AskUserWhatToDoWhenHeCannotUseHisAbility();
}