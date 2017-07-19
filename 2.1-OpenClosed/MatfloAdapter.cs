using Castle.Core.Internal;
using System;
using System.Configuration;
using System.Threading.Tasks;
using _2._1_OpenClosed.Common.ApiModels;
using _2._1_OpenClosed.Common.DataAccess.DTO;
using _2._1_OpenClosed.Common.Infrastructure;
using _2._1_OpenClosed.Common.Service;
using _2._1_OpenClosed.Domain.StockBuid;

namespace _2._1_OpenClosed
{
    public class MatfloAdapter : IMatfloSender
    {
        private readonly IMatfloApi _matfloApi;
        private readonly Clock _clock;
        private readonly int _interval;
        private DateTime _lastSent;

        public MatfloAdapter(int heartbeatIntervalInSeconds) : this(new MatfloApiClient(ConfigurationManager.AppSettings["Iwt.StockBuild.Matflo.Api.Url"]), heartbeatIntervalInSeconds, new Clock())
        {
        }

        public MatfloAdapter(IMatfloApi matfloApi, int interval, Clock clock)
        {
            _matfloApi = matfloApi;
            _interval = interval;
            _clock = clock;
            _lastSent = DateTime.MinValue;
        }

        public virtual async Task<MatfloResponse> SendHeartbeat()
        {
            if (!ShouldSendHeartbeat())
            {
                return null;
            }

            _lastSent = _clock.Now();

            var request = new StockCallbackRequest
            {
                Operation = "heartbeat"
            };

            return await _matfloApi.Callback(request).ConfigureAwait(false);
        }

        public virtual async Task<MatfloResponse> SendProcessConfirmation(StockRequest stockRequest)
        {
            var callbackResponse = CreateCallbackResponse(stockRequest);

            return await _matfloApi.Callback(callbackResponse);
        }

        private bool ShouldSendHeartbeat()
        {
            return NotBeenSent() || IntervalElapsed();
        }

        private bool NotBeenSent()
        {
            return _lastSent == DateTime.MinValue;
        }

        private bool IntervalElapsed()
        {
            return (_clock.Now() - _lastSent).TotalSeconds >= _interval;
        }

        private StockCallbackRequest CreateCallbackResponse(StockRequest stockRequest)
        {
            var callbackRequest = new StockCallbackRequest
            {
                IwtAdviceId = stockRequest.IwtAdviceId,
                MessageNo = stockRequest.MessageNo,
                Operation = GetOperationName(stockRequest.StockRequestType)
            };

            if (stockRequest.StockRequestType == StockRequestType.CancelAllocate)
            {
                stockRequest.Requests = null;
            }
            else
            {
                stockRequest.Requests.ForEach(x =>
                {
                    callbackRequest.Requests.Add(new StockConfirmation
                    {
                        Sku = x.Sku,
                        QuantityRequested = x.QtyRequested,
                        QuantityConfirmed = x.QtyConfirmed ?? 0,
                        TmId = stockRequest.StockRequestType == StockRequestType.Deallocate ? x.TmId : null
                    });
                });
            }

            return callbackRequest;
        }

        private static string GetOperationName(StockRequestType stockRequestType)
        {
            switch (stockRequestType)
            {
                case StockRequestType.Allocate:
                    return "allocatecallback";
                case StockRequestType.Deallocate:
                    return "deallocatecallback";
                case StockRequestType.CancelAllocate:
                    return "cancelallocatecallback";
                default:
                    throw new NotImplementedException("StockRequestType not yet implemented.");
            }
        }
    }
}
