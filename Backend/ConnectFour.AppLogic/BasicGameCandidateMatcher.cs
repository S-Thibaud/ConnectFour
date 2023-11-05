using ConnectFour.AppLogic.Contracts;
using ConnectFour.Domain.GameDomain.Contracts;

namespace ConnectFour.AppLogic;

/// <inheritdoc cref="IGameCandidateMatcher"/>
public class BasicGameCandidateMatcher : IGameCandidateMatcher
{
    public IGameCandidate SelectOpponentToChallenge(IList<IGameCandidate> possibleOpponents)
    {
        if (possibleOpponents.Count == 0)
        {
            return null;
        }
        else
        {
            return possibleOpponents[0];
        }
    }
}