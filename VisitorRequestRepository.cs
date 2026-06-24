using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VisitorWebAPI.Core.DTOs;
using VisitorWebAPI.Core.Entities;
using VisitorWebAPI.Data.Context;
using VisitorWebAPI.Data.Interfaces;

namespace VisitorWebAPI.Data.Repositories
{
    public class VisitorRequestRepository: IVisitorRequestRepository
    {
        private readonly DapperContext _context;
        private readonly ILogger<VisitorRequestDto> _logger;

        public VisitorRequestRepository(DapperContext context, ILogger<VisitorRequestDto> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<int> CreateRequestAsync(VisitorRequestDto requestDto)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@VisitorName", requestDto.VisitorName);

                parameters.Add("@MobileNumber", requestDto.MobileNumber);

                parameters.Add("@CompanyName", requestDto.CompanyName);

                parameters.Add("@PersonToMeet", requestDto.PersonToMeet);

                parameters.Add("@PurposeOfVisit", requestDto.PurposeOfVisit);

                parameters.Add("@VisitDate", requestDto.VisitDate);

                parameters.Add("@CreatedBy", requestDto.CreatedBy);

                return await connection.ExecuteAsync(
                    "sp_CreateVisitorRequest",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "error while creating repository");
                throw;
            }
            }

        public async Task<IEnumerable<VisitorRequestEntity>> GetMyRequestsAsync(int userId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                return await connection.QueryAsync<VisitorRequestEntity>(
                    "sp_GetMyRequests",
                    new { CreatedBy = userId },
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while getting request");
                throw;
            }
        }

        public async Task<IEnumerable<VisitorRequestEntity>> GetPendingRequestsAsync()
        {
            try
            {
                using var connection =_context.CreateConnection();

                return await connection.QueryAsync<VisitorRequestEntity>(
                    "sp_GetPendingRequests",
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while pending request");
                throw;
            } 
        }





        public async Task<bool> UpdateRequestAsync(VisitorRequestDto requestDto)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@RequestId", requestDto.RequestId);

                parameters.Add("@VisitorName", requestDto.VisitorName);

                parameters.Add("@MobileNumber", requestDto.MobileNumber);

                parameters.Add("@CompanyName", requestDto.CompanyName);

                parameters.Add("@PersonToMeet", requestDto.PersonToMeet);

                parameters.Add("@PurposeOfVisit", requestDto.PurposeOfVisit);

                parameters.Add("@VisitDate", requestDto.VisitDate);

                return await connection.QueryFirstOrDefaultAsync<bool>(
                    "sp_UpdateVisitorRequest",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while update");
                throw;

            }
            }






        public async Task<bool> ApproveRequestAsync(int requestId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add(
                    "@RequestId",
                    requestId);

                return await connection.QueryFirstOrDefaultAsync<bool>(
                    "sp_ApproveVisitorRequest",
                    parameters,
                    commandType:
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while approve");
                throw;
            } }

        public async Task<int> RejectRequestAsync(RejectRequestDto rejectRequestDto)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var parameters = new DynamicParameters();

                parameters.Add("@RequestId", rejectRequestDto.RequestId);

                parameters.Add("@Remarks", rejectRequestDto.Remarks);

                return await connection.ExecuteAsync(
                    "sp_RejectRequest",
                    parameters,
                    commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while reject");
                throw;
            } 
        }


        public async Task<VisitorRequestDto> GetRequestByIdAsync(int requestId)
        {
            try
            {
                using var connection =
                    _context.CreateConnection();

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@RequestId",
                    requestId);

                return await connection
                    .QueryFirstOrDefaultAsync
                    <VisitorRequestDto>(
                        "sp_GetVisitorRequestById",
                        parameters,
                        commandType:
                        CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while getting req by id");
                throw;
            } 
        }



        public async Task<int> DeleteRequestAsync(int requestId)
        {
            try
            {
                using var connection =
                    _context.CreateConnection();

                var parameters =
                    new DynamicParameters();

                parameters.Add(
                    "@RequestId",
                    requestId);

                return await connection.ExecuteAsync(
                    "sp_DeleteVisitorRequest",
                    parameters,
                    commandType:
                    CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error while delete req");
                throw;
            } }
    }
}
