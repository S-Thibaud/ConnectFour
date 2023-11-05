using ConnectFour.AppLogic.Contracts;
using ConnectFour.Common;
using ConnectFour.Domain;
using ConnectFour.Domain.GameDomain.Contracts;

namespace ConnectFour.Infrastructure.Storage;

/// <inheritdoc cref="IGameCandidateRepository"/>
internal class InMemoryGameCandidateRepository : IGameCandidateRepository
{
    private readonly ExpiringDictionary<Guid, IGameCandidate> _candidateDictionary;

    public InMemoryGameCandidateRepository()
    {
        _candidateDictionary = new ExpiringDictionary<Guid, IGameCandidate>(TimeSpan.FromMinutes(10));
    }

    public void AddOrReplace(IGameCandidate candidate)
    {
        _candidateDictionary.AddOrReplace(candidate.User.Id, candidate);
    }

    public void RemoveCandidate(Guid userId)
    {
        if (IGameCandidateRepository.ReferenceEquals(this, _candidateDictionary))
        {

        }
        _candidateDictionary.TryRemove(userId, out IGameCandidate _);
    }

    public IGameCandidate GetCandidate(Guid userId)
    {
        Boolean test = _candidateDictionary.TryGetValue(userId, out IGameCandidate key);
        if (test)
        {
            return key;
        }
        else 
        {
            throw new DataNotFoundException();
        }
    }

    public IList<IGameCandidate> FindCandidatesThatCanBeChallengedBy(Guid userId)
    {
        //TODO: retrieve the candidate with userId as key in the _candidateDictionary (use the TryGetValue method)

        //TODO: loop over all candidates (use the Values property of _candidateDictionary)
        //and check if those candidates can be challenged by the candidate you retrieved in the first step (use the CanChallenge method of IGameCandidate).
        //Put the candidates that can be challenged in a list and return that list.

        _candidateDictionary.TryGetValue(userId, out IGameCandidate key);

        IList<IGameCandidate> toBeChalenged = new List<IGameCandidate>();

        foreach (IGameCandidate candidate in _candidateDictionary.Values)
        {
            //candidate.CanChallenge(key)
            if (key.CanChallenge(candidate))
            {
                toBeChalenged.Add(candidate);
            }
        }
        return toBeChalenged;
    }

    public IList<IGameCandidate> FindChallengesFor(Guid challengedUserId)
    {
        return _candidateDictionary.Values.Where(t => t.ProposedOpponentUserId == challengedUserId).ToList();
    }
}