using RawDeal.Cards;

namespace RawDeal.SuperStars;

public class Kane : SuperStar
{
    private Effects _effects = new();
    public Kane() : base(
        "KANE",
        "Kane",
        7,
        2,
        "At the start of your turn, before your draw segment, opponent must take the top card from his Arsenal and place it into his Ringside pile.",
        false){}
    
    public override void UseSuperStarAbility(Player player, Player opponent)
    {
        base.UseSuperStarAbility(player, opponent);
        _effects.DamageOpponent(opponent, 1);
    }

    public override byte UseAbilityBeforeTakingACard(Player player, Player opponent)
    {
        UseSuperStarAbility(player, opponent);
        return 1;
    }
}