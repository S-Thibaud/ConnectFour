using ConnectFour.Domain.GameDomain.Contracts;

namespace ConnectFour.Domain.GameDomain;

/// <inheritdoc cref="IGameCandidateFactory"/>
public class GameCandidateFactory : IGameCandidateFactory
{
    /// <inheritdoc/>
    // aanmaken van een game kandidaat

    GameCandidate _candidate;

    public IGameCandidate CreateNewForUser(User user, GameSettings settings)
    {
        return new GameCandidate(user, settings);
    }
}