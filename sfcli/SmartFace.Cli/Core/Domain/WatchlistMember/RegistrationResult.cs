using System;
using System.Collections.Generic;

namespace SmartFace.Cli.Core.Domain.WatchlistMember
{
    public class RegistrationResult
    {
        public IReadOnlyCollection<WatchlistMemberRegistrationFailure> Failures => _failureList.ToArray();

        private readonly List<WatchlistMemberRegistrationFailure> _failureList;

        public RegistrationResult()
        {
            _failureList = new List<WatchlistMemberRegistrationFailure>();
        }

        public RegistrationResult Add(WatchlistMemberRegistrationFailure memberRegistrationFailure)
        {
            if (memberRegistrationFailure == null)
            {
                throw new ArgumentNullException(nameof(memberRegistrationFailure));
            }

            _failureList.Add(memberRegistrationFailure);
            return this;
        }
    }

    public class WatchlistMemberRegistrationFailure
    {
        public string[] PhotoPaths { get; }
        public Exception Exception { get; }

        public WatchlistMemberRegistrationFailure(string[] photoPaths, Exception exception)
        {
            PhotoPaths = photoPaths ?? throw new ArgumentNullException(nameof(photoPaths));
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }
}
