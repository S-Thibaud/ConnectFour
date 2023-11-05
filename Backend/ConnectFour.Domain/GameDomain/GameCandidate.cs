using ConnectFour.Domain.GameDomain.Contracts;

namespace ConnectFour.Domain.GameDomain;

/// <inheritdoc cref="IGameCandidate"/>
internal class GameCandidate : IGameCandidate
{
    public User User { get; set; }
    public GameSettings GameSettings { get; }
    public Guid GameId { get; set; }
    public Guid ProposedOpponentUserId { get; set; }

    internal GameCandidate(User user, GameSettings gameSettings)
    {
        User = user;
        GameSettings = gameSettings;
    }

    public bool CanChallenge(IGameCandidate targetCandidate)
    {
        if (targetCandidate.GameId != Guid.Empty)
        {
            return false;
        }
        else if (GameId != Guid.Empty)
        {
            return false;
        }
        else if (GameSettings.GridColumns != targetCandidate.GameSettings.GridColumns || GameSettings.GridRows != targetCandidate.GameSettings.GridRows)
        {
            return false;
        }
        else if (GameSettings.ConnectionSize != targetCandidate.GameSettings.ConnectionSize)
        {
            return false;
        }
        else if (GameSettings.EnablePopOut == false && targetCandidate.GameSettings.EnablePopOut == true)
        {
            return false;
        }
        else if (User.Id == targetCandidate.User.Id)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void Challenge(IGameCandidate targetCandidate)
    {
        if (CanChallenge(targetCandidate))
        {
            ProposedOpponentUserId = targetCandidate.User.Id;
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    public void AcceptChallenge(IGameCandidate challenger)
    {
        if (GameId != Guid.Empty || challenger.GameId != Guid.Empty)
        {
            throw new InvalidOperationException("game");
        }
        else if (challenger.ProposedOpponentUserId != User.Id)
        {
            throw new InvalidOperationException("other candidate");
        }
        else
        {
            ProposedOpponentUserId = challenger.User.Id;
        }
    }

    public void WithdrawChallenge()
    {
        throw new NotImplementedException();
    }
}
