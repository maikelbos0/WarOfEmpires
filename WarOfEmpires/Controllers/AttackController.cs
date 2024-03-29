﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [Route(Route)]
    public class AttackController : BaseController {
        public const string Route = "Attack";

        public AttackController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet("", Order = -1)]
        [HttpGet(nameof(Index))]
        public ViewResult Index() {
            return View();
        }

        [HttpPost(nameof(GetReceivedAttacks))]
        public JsonResult GetReceivedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet(nameof(ExecutedIndex))]
        public ViewResult ExecutedIndex() {
            return View();
        }
        
        [HttpPost(nameof(GetExecutedAttacks))]
        public JsonResult GetExecutedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetExecutedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet(nameof(Details))]
        public ViewResult Details(int id) {
            var model = _messageService.Dispatch(new GetAttackDetailsQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadAttackCommand(_authenticationService.Identity, id));
            }

            return View(model);
        }

        [HttpGet(nameof(Execute))]
        public ViewResult Execute(int defenderId) {
            return View(_messageService.Dispatch(new GetDefenderQuery(_authenticationService.Identity, defenderId)));
        }

        [HttpPost(nameof(Execute))]
        public ActionResult Execute(ExecuteAttackModel model) {
            return BuildViewResultFor(new AttackCommand(model.AttackType, _authenticationService.Identity, model.DefenderId, model.Turns))
                .OnSuccess(nameof(Details), () => new { id = _messageService.Dispatch(new GetLastExecutedAttackQuery(_authenticationService.Identity)) })
                .OnFailure(model);
        }
    }
}
