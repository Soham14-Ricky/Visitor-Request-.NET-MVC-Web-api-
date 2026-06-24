using System;
using System.Collections.Generic;
using System.Text;
using VisitorWebAPI.Core.DTOs;
using VisitorWebAPI.Core.Entities;

namespace VisitorWebAPI.Data.Interfaces
{
    public interface IVisitorRequestRepository
    {
        Task<int> CreateRequestAsync(
            VisitorRequestDto requestDto);

        Task<IEnumerable<VisitorRequestEntity>>
            GetMyRequestsAsync(int userId);

        Task<IEnumerable<VisitorRequestEntity>>
            GetPendingRequestsAsync();

        Task<bool> UpdateRequestAsync(
            VisitorRequestDto requestDto);

        Task<int> DeleteRequestAsync(int requestId);

        Task<bool> ApproveRequestAsync(int requestId);

        Task<int> RejectRequestAsync(RejectRequestDto rejectRequestDto);


        Task<VisitorRequestDto> GetRequestByIdAsync(int requestId);



        
    }
}
