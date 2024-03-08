using RawDeal.Status;

namespace RawDeal.Cards;

public class EffectsCatalog
{
    private Effects _effects = new();
    public void ApplyCardEffect(Player player, Player opponent, Play play)
    {
        _effects.CkeckOpponentsDoubleEdgedCard(opponent, play);
        _effects.MarkLastCardPlayed(player, play);
        switch (play.Card.Title)
        {
            case "Chop":    // hybrid
                if (play.PlayedAs == "ACTION")
                {
                    _effects.DiscardThisCard(player, play.IdInHand);
                    _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                }
                break;
            case "Head Butt":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
                break;
            case "Haymaker":
                player.State.Turn.Add(PlayerStatus.StrikesInTheTurnArePlus1D);
                play.PlayDamage += 1;
                break;
            case "Back Body Drop":
                _effects.DrawCardsOrForceOpponentToDiscardCards(player, opponent, play, 2);
                break;
            // case "Shoulder Block":    // hybrid
            //     break;
            case "Kick":
                _effects.CollateralDamage(player, 1);
                player.State.LastCardPlayed = PlayerStatus.KickWasPlayed;
                break;
            // case "Cross Body Block":    // hybrid
            //     break;
            // case "Ensugiri":    // hybridcase "Ensugiri":
            //     break;
            case "Running Elbow Smash":
                _effects.CollateralDamage(player, 1);
                break;
            // case "Drop Kick":    // hybridcase "Drop Kick":
            //     break;
            case "Discus Punch":
                _effects.DoubleEdgedCard(player, 2);
                break;
            case "Superkick":
                _effects.AddDamageAfterPlayingAMinimumDamageManeuver(play, 5);
                break;
            case "Spinning Heel Kick":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Spear":    // hybridcase "Spear":
                break;
            case "Clothesline":
                player.State.NextPlayDamage = PlayerStatus.NextManeuverIsPlus2D;
                break;
            case "Arm Bar Takedown":    // hybrid
                if (play.PlayedAs == "ACTION")
                {
                    _effects.DiscardThisCard(player, play.IdInHand);
                    _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                }
                break;
            case "Arm Drag":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
                break;
            case "Snap Mare":
                player.State.NextPlayDamage = PlayerStatus.NextStrikeIsPlus2D;
                break;
            case "Double Leg Takedown":
                _effects.MayDrawSpecifiedNumberOfCards(player, 1);
                break;
            // case "Fireman's Carry":
            //     break;
            case "Headlock Takedown":
                _effects.MustDrawSpecifiedNumberOfCards(opponent, 1);
                break;
            // case "Belly to Belly Suplex":    // hybrid
            //     break;
            case "Atomic Drop":
                player.State.NextPlayDamage = PlayerStatus.NextManeuverIsPlus2D;
                break;
            // case "Vertical Suplex":    // hybrid
            //     break;
            // case "Belly to Back Suplex":    // hybrid
            //     break;
            case "Pump Handle Slam":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 2);
                break;
            case "Reverse DDT":
                _effects.MayDrawSpecifiedNumberOfCards(player, 1);
                break;
            case "Samoan Drop":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Bulldog":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
                _effects.PlayerDiscardSpecifiedNumberOfOpponentsCards(player, opponent, 1);
                break;
            case "Fisherman's Suplex":
                _effects.CollateralDamage(player, 1);
                _effects.MayDrawSpecifiedNumberOfCards(player, 1);
                break;
            case "DDT":
                _effects.CollateralDamage(player, 1);
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 2);
                break;
            case "Power Slam":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Powerbomb":
                _effects.MayDrawSpecifiedNumberOfCards(player, 1);
                _effects.IncreaseDamageByOneForEverySlamCardInTheRingArea(player, play);
                break;
            case "Press Slam":
                _effects.CollateralDamage(player, 1);
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 2);
                break;
            case "Collar & Elbow Lockup":    // hybrid
                if (play.PlayedAs == "ACTION")
                {
                    _effects.DiscardThisCard(player, play.IdInHand);
                    _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                }
                break;
            case "Arm Bar":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
                break;
            case "Bear Hug":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            // case "Full Nelson":
            //     break;
            case "Choke Hold":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Ankle Lock":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Standing Side Headlock":
                _effects.MustDrawSpecifiedNumberOfCards(opponent, 1);
                break;
            // case "Cobra Clutch":
            //     break;
            case "Chicken Wing":
                _effects.RecoverDamage(player, 2);
                break;
            // case "Sleeper":
            //     break;
            // case "Camel Clutch":
            //     break;
            case "Boston Crab":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Guillotine Stretch":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                _effects.MayDrawSpecifiedNumberOfCards(player, 1);
                break;
            // case "Abdominal Stretch":
            //     break;
            case "Torture Rack":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Figure Four Leg Lock":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            // case "Step Aside":
            //     break;
            // case "Escape Move":
            //     break;
            // case "Break the Hold":
            //     break;
            // case "Rolling Takedown":
            //     break;
            // case "Knee to the Gut":
            //     break;
            // case "Elbow to the Face":
            //     break;
            case "Clean Break":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 4);
                _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                break;
            case "Manager Interferes":
                _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                break;
            // case "Disqualification!":
            //     break;
            // case "No Chance in Hell":
            //     break;
            // case "Hmmm":
            //     break;
            // case "Don't Think Too Hard":
            //     break;
            // case "Whaddya Got?":
            //     break;
            // case "Not Yet":
            //     break;
            case "Jockeying for Position":    // hybrid
                if (play.PlayedAs == "ACTION")
                    _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.JockeyingForPositionEffect(player);
                break;
            case "Irish Whip":    // hybrid
                if (play.PlayedAs == "ACTION")
                    _effects.MoveThisCardToRingArea(player, play.IdInHand);
                player.State.LastCardPlayed = PlayerStatus.IrishWhipWasPlayed;
                player.State.NextPlayDamage = PlayerStatus.NextStrikeIsPlus5D;
                break;
            //     break;
            // case "Flash in the Pan":
            //     break;
            // case "View of Villainy":
            //     break;
            case "Shake It Off":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.MoveCardFromOpponentsRingAreaToHisRingSide(player, opponent);
                opponent.State.CheckMrSockoInRingArea(opponent);
                break;
            case "Offer Handshake":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.MayDrawSpecifiedNumberOfCards(player, 3);
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
                break;
            case "Roll Out of the Ring":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.DiscardCardsToObtainCardsFromTheRingside(player, 2);
                break;
            // case "Distract the Ref":
            //     break;
            case "Recovery":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.RecoverDamage(player, 2);
                _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                break;
            case "Spit At Opponent":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 4);
                break;
            case "Get Crowd Support":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                player.State.NextPlayDamage = PlayerStatus.NextManeuverIsPlus4D;
                player.State.NextPlayFortitude = PlayerStatus.NextManeuverReversalIsPlus12F;
                break;
            // case "Comeback!":
            //     break;
            // case "Ego Boost":
            //     break;
            // case "Deluding Yourself":
            //     break;
            case "Stagger":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                player.State.NextPlayFortitude = PlayerStatus.StaggerWasPlayed;
                break;
            case "Diversion":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                player.State.NextPlayFortitude = PlayerStatus.DiversionWasPlayed;
                break;
            // case "Marking Out":
            //     break;
            case "Puppies! Puppies!":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.RecoverDamage(player, 5);
                _effects.MustDrawSpecifiedNumberOfCards(player, 2);
                break;
            // case "Shane O'Mac":
            //     break;
            // case "Maintain Hold":
            //     break;
            // case "Pat & Gerry":
            //     break;
            // case "Austin Elbow Smash":
            //     break;
            case "Lou Thesz Press":
                _effects.MayDrawSpecifiedNumberOfCards(player, 1);
                break;
            case "Double Digits":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 2);
                _effects.DamageOpponent(opponent, 2);
                break;
            // case "Stone Cold Stunner":
            //     break;
            case "Open Up a Can of Whoop-A%$":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                player.State.NextPlayDamage = PlayerStatus.NextManeuverIsPlus6D;
                player.State.NextPlayFortitude = PlayerStatus.NextManeuverReversalIsPlus20F;
                break;
            case "Undertaker's Flying Clothesline":
                _effects.DoubleEdgedCard(player, 6);
                break;
            case "Undertaker Sits Up!":
                _effects.CollateralDamage(player, 4);
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                player.State.Turn.Add(PlayerStatus.ManeuversInTheTurnArePlus2D);
                player.State.Turn.Add(PlayerStatus.ReversalsInTheTurnArePlus25F);
                break;
            case "Undertaker's Tombstone Piledriver":    // hybrid
                if (play.PlayedAs == "ACTION")
                {
                    _effects.DiscardThisCard(player, play.IdInHand);
                    _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                }
                break;
            case "Power of Darkness":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                player.State.Turn.Add(PlayerStatus.ManeuversInTheTurnArePlus5D);
                player.State.Turn.Add(PlayerStatus.ReversalsInTheTurnArePlus20F);
                break;
            case "Have a Nice Day!":
                _effects.DiscardAllThaHand(opponent);
                break;
            // case "Double Arm DDT":    // hybrid
            //     break;
            case "Tree of Woe":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 2);
                break;
            // case "Mandible Claw":
            //     break;
            case "Mr. Socko":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.MoveCardFromArsenalOrRingSideToTheHand(player);
                player.State.Game.Add(PlayerStatus.MrSockoInRingArea);
                break;
            case "Leaping Knee to the Face":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Facebuster":
                _effects.MayDrawSpecifiedNumberOfCards(player, 2);
                break;
            case "I Am the Game":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                player.State.Turn.Add(PlayerStatus.ManeuversInTheTurnArePlus3D);
                _effects.DrawCardsOrForceOpponentToDiscardCards(player, opponent, play, 2);
                break;
            case "Pedigree":    // hybrid
                _effects.RecieveADamageBonusAfterPlayingAStrike(play);
                break;
            case "Chyna Interferes":
                _effects.MustDrawSpecifiedNumberOfCards(player, 2);
                break;
            case "Smackdown Hotel":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.MustDrawSpecifiedNumberOfCards(player, 1);
                _effects.SeeOpponentsHand(player, opponent);
                player.State.NextPlayDamage = PlayerStatus.NextManeuverIsPlus6D;
                break;
            case "Take That Move, Shine It Up Real Nice, Turn That Sumb*tch Sideways, and Stick It Straight Up Your Roody Poo Candy A%$!":
                _effects.RecoverDamage(player, 5);
                break;
            case "Rock Bottom":    // hybrid
                if (play.PlayedAs == "REVERSAL")
                    _effects.PlayerDiscardSpecifiedNumberOfHisCards(player, 1);
                _effects.RecoverCardFromRingSideOrArsenal(player, "The People's Elbow");
                break;
            case "The People's Eyebrow":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.RecoverCardsToTheHand(player, 2);
                _effects.RecoverDamage(player, 2);
                break;
            case "The People's Elbow":    // hybrid
                if (play.PlayedAs == "ACTION")
                {
                    _effects.PutCardInTheBottomOfPlayersArsenal(player, play);
                    _effects.MustDrawSpecifiedNumberOfCards(player, 2);
                }
                break;
            case "Kane's Flying Clothesline":
                _effects.DoubleEdgedCard(player, 6);
                break;
            case "Kane's Return!":
                _effects.CollateralDamage(player, 4);
                player.State.Turn.Add(PlayerStatus.ManeuversInTheTurnArePlus2D);
                player.State.Turn.Add(PlayerStatus.ReversalsInTheTurnArePlus15F);
                break;
            // case "Kane's Tombstone Piledriver":
            //     break;
            case "Kane's Chokeslam":
                player.State.LastCardPlayed = PlayerStatus.KanesChokeSlamWasPlayed;
                break;
            case "Hellfire & Brimstone":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.DiscardAllThaHand(player);
                _effects.DiscardAllThaHand(opponent);
                _effects.DamageOpponent(opponent, 5);
                break;
            case "Lionsault":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 1);
                break;
            case "Y2J":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.DrawCardsOrForceOpponentToDiscardCards(player, opponent, play, 5);
                break;
            case "Don't You Never... EVER!":
                _effects.PlayerDiscardSpecifiedNumberOfHisCards(opponent, 2);
                player.State.Turn.Add(PlayerStatus.ManeuversInTheTurnArePlus2D);
                break;
            // case "Walls of Jericho":
            //     break;
            case "Ayatollah of Rock 'n' Roll-a":
                _effects.MoveThisCardToRingArea(player, play.IdInHand);
                _effects.SeeOpponentsHand(player, opponent);
                player.State.Turn.Add(PlayerStatus.AyatollahWasPlayed);
                break;
        }
    }
}