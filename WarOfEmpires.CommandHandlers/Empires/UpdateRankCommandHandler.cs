using System;
using System.Linq;
using VDT.Core.DependencyInjection;
using WarOfEmpires.CommandHandlers.Decorators;
using WarOfEmpires.Commands.Empires;
using WarOfEmpires.Domain.Game;
using WarOfEmpires.Domain.Players;
using WarOfEmpires.Repositories.Game;
using WarOfEmpires.Repositories.Players;

namespace WarOfEmpires.CommandHandlers.Empires {
    [TransientServiceImplementation(typeof(ICommandHandler<UpdateRankCommand>))]
    public sealed class UpdateRankCommandHandler : ICommandHandler<UpdateRankCommand> {
        private readonly IPlayerRepository _repository;
        private readonly IGameStatusRepository _gameStatusRepository;
        private readonly IRankService _rankService;

        public UpdateRankCommandHandler(IPlayerRepository repository, IGameStatusRepository gameStatusRepository, IRankService rankService) {
            _repository = repository;
            _gameStatusRepository = gameStatusRepository;
            _rankService = rankService;
        }

        [Audit]
        public CommandResult<UpdateRankCommand> Execute(UpdateRankCommand command) {
            var result = new CommandResult<UpdateRankCommand>();
            var players = _repository.GetAll();
            var gameStatus = _gameStatusRepository.Get();

            _rankService.Update(players);

            if (gameStatus.Phase != GamePhase.Finished) {
                gameStatus.CurrentGrandOverlord = players.SingleOrDefault(p => p.Title == TitleType.GrandOverlord);

                if (gameStatus.CurrentGrandOverlord?.GrandOverlordTime >= TimeSpan.FromHours(GameStatus.GrandOverlordHoursToWin)) {
                    gameStatus.Phase = GamePhase.Finished;
                }
            }

            _repository.SaveChanges();

            return result;
        }
    }
}