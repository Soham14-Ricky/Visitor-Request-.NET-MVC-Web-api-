using VisitorMVC.Models.DTOs;

namespace VisitorMVC.Services.Interfaces
{
    public interface IVisitorRequestService
    {
        Task<bool> CreateRequestAsync(VisitorRequestDto dto);

        Task<IEnumerable<VisitorRequestDto>>GetMyRequestsAsync();

        Task<bool> UpdateRequestAsync(VisitorRequestDto dto);

        Task<bool> DeleteRequestAsync(int requestId);

        Task<IEnumerable<VisitorRequestDto>>GetPendingRequestsAsync();

        Task<bool> ApproveRequestAsync(int requestId);

        Task<bool> RejectRequestAsync(RejectRequestDto dto);

        Task<VisitorRequestDto>GetRequestByIdAsync(int requestId);
    }
}