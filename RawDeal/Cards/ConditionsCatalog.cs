using RawDeal.Status;

namespace RawDeal.Cards;

public class ConditionsCatalog
{
    private CardConditions _conditions = new();
    public bool CanThisCardBePlayedAsManeuverOrAction(Player player, Player opponent, Card card, string typeOfPlay)
    {
        byte fortitudeRequired = _conditions.GetFortitudeRequiredToPlayCard(player, card, typeOfPlay);
        if (!card.Types.Contains(typeOfPlay) || fortitudeRequired > player.FortitudeRating())
            return false;
        bool canPlayCard = card.Title switch
        {
            // "Chop" =>
            // "Haymaker" =>
            "Back Body Drop" => _conditions.LastPlayWasAIrishWhip(player),
            // "Shoulder Block" =>
            "Cross Body Block" => _conditions.LastPlayWasAIrishWhip(player),
            // "Ensugiri" =>
            // "Drop Kick" =>
            // "Discus Punch" =>
            // "Superkick" =>
            // "Spear" =>
            // "Clothesline" =>
            // "Arm Bar Takedown" =>
            // "Snap Mare" =>
            // "Fireman's Carry" =>
            // "Belly to Belly Suplex" =>
            // "Atomic Drop" =>
            // "Vertical Suplex" =>
            // "Belly to Back Suplex" =>
            // "Powerbomb" =>
            // "Collar & Elbow Lockup" =>
            // "Full Nelson" =>
            // "Cobra Clutch" =>
            // "Sleeper" =>
            // "Camel Clutch" =>
            // "Abdominal Stretch" =>
            // "Hmmm" =>
            // "Don't Think Too Hard" =>
            // "Whaddya Got?" =>
            // "Not Yet" =>
            // "Jockeying for Position" =>
            // "Irish Whip" =>
            // "Flash in the Pan" =>
            // "View of Villainy" =>
            "Shake It Off" => _conditions.OpponentsFortitudRatingIsHigherThanThePlayers(player, opponent),
            // "Roll Out of the Ring" =>
            // "Distract the Ref" =>
            "Spit At Opponent" => _conditions.HasMinimumNumberOfCardsInHand(player, 2),
            // "Get Crowd Support" =>
            // "Comeback!" =>
            // "Ego Boost" =>
            // "Deluding Yourself" =>
            "Stagger" => _conditions.LastPlayWasOfASpecificType(player, "Maneuver"),
            // "Diversion" =>
            // "Marking Out" =>
            // "Shane O'Mac" =>
            // "Maintain Hold" =>
            // "Pat & Gerry" =>
            "Austin Elbow Smash" => _conditions.LastManeuverHadAMinimalDamage(player, 5),
            // "Stone Cold Stunner" =>
            // "Open Up a Can of Whoop-A%$" =>
            "Undertaker's Flying Clothesline" => _conditions.LastManeuverHadAMinimalDamage(player, 5) && _conditions.LastPlayWasOfASpecificType(player, "Maneuver"),
            // "Power of Darkness" =>
            // "Double Arm DDT" =>
            // "Tree of Woe" =>
            // "Mandible Claw" =>
            // "Mr. Socko" =>
            "Leaping Knee to the Face" => _conditions.LastPlayWasAIrishWhip(player),
            // "I Am the Game" =>
            // "Pedigree" =>
            // "Smackdown Hotel" =>
            // "Rock Bottom" =>
            // "The People's Eyebrow" =>f
            "The People's Elbow" when typeOfPlay == "Maneuver" => _conditions.ThereIsASpecificCardInTheRing(player, "Rock Bottom"),
            "Kane's Flying Clothesline" => _conditions.LastManeuverHadAMinimalDamage(player, 4) && _conditions.LastPlayWasOfASpecificType(player, "Maneuver"),
            // "Kane's Tombstone Piledriver" =>
            // "Hellfire & Brimstone" =>
            "Lionsault" => _conditions.LastManeuverHadAMinimalDamage(player, 4),
            // "Y2J" =>
            // "Walls of Jericho" =>
            // "Ayatollah of Rock 'n' Roll-a" =>
            _ => true,
        };
        return canPlayCard;
    }

    public bool CanThisCardBePlayedAsReversal(Player player, Play opponentsPlay, Card card, string fromWhere)
    {
        byte fortitudeRequired = _conditions.GetFortitudeRequiredToPlayReversal(opponentsPlay, card);
        bool irreversible = _conditions.OpponentsCardIsIrreversible(opponentsPlay, fromWhere);
        if (player.FortitudeRating() < fortitudeRequired || !card.Types.Contains("Reversal") || irreversible)
            return false;
        bool canPlayReversal = card.Title switch
        {
            "Shoulder Block" => _conditions.OpponentsPlayWasAfterAnIrishWhip(opponentsPlay),
            "Cross Body Block" => _conditions.OpponentsPlayWasAfterAnIrishWhip(opponentsPlay),
            "Ensugiri" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Kick"),
            "Drop Kick" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Drop Kick"),
            "Spear" => _conditions.OpponentsPlayWasAfterAnIrishWhip(opponentsPlay),
            "Belly to Belly Suplex" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Belly to Belly Suplex"),
            "Vertical Suplex" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Vertical Suplex"),
            "Belly to Back Suplex" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Belly to Back Suplex"),
            "Step Aside" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Strike"),
            "Escape Move" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Grapple"),
            "Break the Hold" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Submission"),
            "Rolling Takedown" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Grapple") && _conditions.OpponentsPlayHadAMaximumDamage(opponentsPlay, 7),
            "Knee to the Gut" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Strike") && _conditions.OpponentsPlayHadAMaximumDamage(opponentsPlay, 7),
            "Elbow to the Face" => _conditions.OpponentsPlayHasASpecificType(opponentsPlay, "MANEUVER") && _conditions.OpponentsPlayHadAMaximumDamage(opponentsPlay, 7),
            "Clean Break" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Jockeying for Position"),
            "Manager Interferes" => _conditions.OpponentsPlayHasASpecificType(opponentsPlay, "MANEUVER"),
            // "Disqualification!" =>
            "No Chance in Hell" => _conditions.OpponentsPlayHasASpecificType(opponentsPlay, "ACTION"),
            "Jockeying for Position" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Jockeying for Position"),
            "Irish Whip" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Irish Whip"),
            "Lou Thesz Press" => _conditions.ItIsReversingFromTheHand(fromWhere) && _conditions.OpponentsPlayWasAfterAnIrishWhip(opponentsPlay),
            "Double Digits" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Strike") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Grapple") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Submission"),
            "Undertaker Sits Up!" => _conditions.OpponentsPlayHasASpecificType(opponentsPlay, "MANEUVER"),
            "Have a Nice Day!" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Strike") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Grapple") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Submission"),
            "Double Arm DDT" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Back Body Drop"),
            "Facebuster" => _conditions.ItIsReversingFromTheHand(fromWhere) && _conditions.OpponentsPlayWasAfterAnIrishWhip(opponentsPlay),
            "Pedigree" => _conditions.OpponentsPlayIsASpecificCard(opponentsPlay, "Back Body Drop"),
            "Chyna Interferes" => _conditions.OpponentsPlayHasASpecificType(opponentsPlay, "MANEUVER"),
            "Take That Move, Shine It Up Real Nice, Turn That Sumb*tch Sideways, and Stick It Straight Up Your Roody Poo Candy A%$!" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Strike") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Grapple") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Submission"),
            "Rock Bottom" => _conditions.OpponentsPlayHasASpecificType(opponentsPlay, "MANEUVER") && _conditions.ItIsReversingFromTheHand(fromWhere) && _conditions.PlayerHasAMinumumNumberOfCardsInTheHand(player, 2),
            "Kane's Return!" => _conditions.OpponentsPlayHasASpecificType(opponentsPlay, "MANEUVER"),
            "Don't You Never... EVER!" => _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Strike") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Grapple") || _conditions.OpponentsPlayHasASpecificSubtype(opponentsPlay, "Submission"),
            _ => true,
        };
        return canPlayReversal;
    }
}