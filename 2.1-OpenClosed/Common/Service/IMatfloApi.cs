using System.Threading.Tasks;
using _2._1_OpenClosed.Common.ApiModels;

namespace _2._1_OpenClosed.Common.Service
{
    public interface IMatfloApi
    {
        Task<MatfloResponse> Callback(StockCallbackRequest request);
    }
}
