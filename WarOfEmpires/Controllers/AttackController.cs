﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WarOfEmpires.Commands.Attacks;
using WarOfEmpires.Filters;
using WarOfEmpires.Models.Attacks;
using WarOfEmpires.Models.Grids;
using WarOfEmpires.Queries.Attacks;
using WarOfEmpires.Services;

namespace WarOfEmpires.Controllers {
    [Authorize]
    [UserOnline]
    [Route("Attack")]
    public class AttackController : BaseController {
        public AttackController(IAuthenticationService authenticationService, IMessageService messageService, IDataGridViewService dataGridViewService)
            : base(messageService, authenticationService, dataGridViewService) {
        }

        [HttpGet]
        [HttpGet("Index")]
        public ViewResult Index() {
            return View();
        }

        [HttpPost("GetReceivedAttacks")]
        public JsonResult GetReceivedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetReceivedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet("ExecutedIndex")]
        public ViewResult ExecutedIndex() {
            return View();
        }
        
        [HttpPost("GetExecutedAttacks")]
        public JsonResult GetExecutedAttacks(DataGridViewMetaData metaData) {
            return GridJson(new GetExecutedAttacksQuery(_authenticationService.Identity), metaData);
        }

        [HttpGet("Details")]
        public ViewResult Details(int id) {
            var model = _messageService.Dispatch(new GetAttackDetailsQuery(_authenticationService.Identity, id));

            if (!model.IsRead) {
                _messageService.Dispatch(new ReadAttackCommand(_authenticationService.Identity, id));
            }

            // Explicitly name view so it works from other actions
            return View("Details", model);
        }

        [HttpGet("LastExecutedAttackDetails")]
        public ViewResult LastExecutedAttackDetails() {
            var id = _messageService.Dispatch(new GetLastExecutedAttackQuery(_authenticationService.Identity));

            return Details(id);
        }

        [HttpGet("Execute")]
        public ViewResult Execute(int defenderId) {
            return View(_messageService.Dispatch(new GetDefenderQuery(_authenticationService.Identity, defenderId)));
        }

        [HttpPost("Execute")]
        public ViewResult Execute(ExecuteAttackModel model) {
            return BuildViewResultFor(new AttackCommand(model.AttackType, _authenticationService.Identity, model.DefenderId, model.Turns))
                .OnSuccess(LastExecutedAttackDetails)
                .OnFailure("Execute", model);
        }
    }
}