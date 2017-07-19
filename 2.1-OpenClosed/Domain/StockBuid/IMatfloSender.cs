using System.Threading.Tasks;
using _2._1_OpenClosed.Common.ApiModels;
using _2._1_OpenClosed.Common.DataAccess.DTO;

namespace _2._1_OpenClosed.Domain.StockBuid
{
    public interface IMatfloSender
    {
        Task<MatfloResponse> SendHeartbeat();

        Task<MatfloResponse> SendProcessConfirmation(StockRequest stockRequest);
    }
}
