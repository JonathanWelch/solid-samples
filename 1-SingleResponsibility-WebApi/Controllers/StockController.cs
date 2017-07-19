using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using log4net;
using _1_SingleResponsibility_WebApi.Common.ApiModels;
using _1_SingleResponsibility_WebApi.Common.DataAccess;
using _1_SingleResponsibility_WebApi.Common.DataAccess.Commands;
using _1_SingleResponsibility_WebApi.Common.DataAccess.Queries;

namespace _1_SingleResponsibility_WebApi.Controllers
{
    public class StockController : ApiController
    {
        #region Fields

        private readonly ICommandAndQueryExecutor _commandAndQueryExecutor;
        private readonly ILog _logger;

        #endregion

        #region Constructors

        public StockController() : this(new CommandAndQueryExecutor(), LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType))
        {
        }

        public StockController(ICommandAndQueryExecutor commandAndQueryExecutor, ILog logger)
        {
            _commandAndQueryExecutor = commandAndQueryExecutor;
            _logger = logger;
        }

        #endregion


        [HttpPost]
        public IHttpActionResult Allocate([FromBody] StockRequestModel stockRequest)
        {
            LogRequest(stockRequest);

            stockRequest.StockRequestType = StockRequestType.Allocate;

            return ProcessRequest(stockRequest);
        }

        [HttpPost]
        public IHttpActionResult Deallocate([FromBody] StockRequestModel stockRequest)
        {
            LogRequest(stockRequest);
            stockRequest.StockRequestType = StockRequestType.Deallocate;

            return ProcessRequest(stockRequest);
        }

        [HttpPost]
        public IHttpActionResult CancelAllocate([FromBody] StockRequestModel stockRequest)
        {
            LogRequest(stockRequest);
            stockRequest.StockRequestType = StockRequestType.CancelAllocate;

            return ProcessRequest(stockRequest);
        }

        [HttpGet]
        [Route("heartbeat/{messageNo}")]
        public IHttpActionResult Heartbeat2(int messageNo)
        {
            return Ok(
                new StockRequestResponse
                {
                    Operation = "heartbeat",
                    MessageNo = messageNo
                });
        }

        [HttpPost]
        [Route("heartbeat")]
        public IHttpActionResult Heartbeat([FromBody] dynamic input)
        {
            return Ok(
                new StockRequestResponse
                {
                    Operation = "heartbeat",
                    MessageNo = input.MessageNo
                });
        }

        #region Help Methods

        private void LogRequest(StockRequestModel stockRequestModel)
        {
            Task.Run(() =>
            {
                var logMessage = new StringBuilder();

                logMessage.AppendFormat("Request: {0} {1} {2} ", stockRequestModel.IwtAdviceId, stockRequestModel.MessageNo, Enum.GetName(typeof(StockRequestType), stockRequestModel.StockRequestType));

                if (stockRequestModel.Requests != null && stockRequestModel.Requests.Any())
                {
                    foreach (var stockRequest in stockRequestModel.Requests)
                    {
                        logMessage.AppendFormat("[ Sku {0} ", stockRequest.Sku);
                        logMessage.AppendFormat("Quantity {0} ", stockRequest.Quantity);
                        logMessage.AppendFormat("TmId {0} ]", stockRequest.TmId);
                    }
                }
                else
                {
                    logMessage.AppendFormat("No stock request info.");
                }

                _logger.Debug(logMessage.ToString(), null);
            });
        }

        private void LogResponse(StockRequestResponse stockResponse)
        {
            Task.Run(() =>
            {
                var logMessage = new StringBuilder();

                logMessage.AppendFormat(
                    "Response: {0} {1} {2} {3} {4} {5} {6}",
                    stockResponse.IwtAdviceId,
                    stockResponse.MessageNo,
                    stockResponse.Operation,
                    stockResponse.RequestProcessed,
                    stockResponse.TotalQuantity,
                    stockResponse.TotalSkus,
                    stockResponse.Error);

                _logger.Debug(logMessage.ToString(), null);
            });
        }

        private IHttpActionResult ProcessRequest(StockRequestModel stockRequest)
        {
            if (stockRequest == null)
            {
                return InternalServerError();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var operationName = Enum.GetName(typeof(StockRequestType), stockRequest.StockRequestType).ToLowerInvariant();
            var response = new StockRequestResponse(stockRequest.IwtAdviceId, stockRequest.MessageNo, operationName);
            string errorMessage;

            response.RequestProcessed = false;
            if (stockRequest.StockRequestType != StockRequestType.CancelAllocate)
            {
                response.TotalSkus = 0;
                response.TotalQuantity = 0;
            }

            if (stockRequest.IsValid(DoesMessageExists, DoesAdviceExists, out errorMessage))
            {
                _commandAndQueryExecutor.Execute(new PersistIwtStockRequest(stockRequest));
                response.RequestProcessed = true;

                if (stockRequest.Requests != null)
                {
                    response.TotalSkus = stockRequest.Requests.GroupBy(x => x.Sku).Count();
                    response.TotalQuantity = stockRequest.Requests.Sum(x => x.Quantity);
                }
            }

            response.Error = errorMessage;
            LogResponse(response);

            return Ok(response);
        }

        private bool DoesMessageExists(int messageNo)
        {
            var retrieveStockRequestMessage =
                _commandAndQueryExecutor.Query(new RetrieveStockRequest(messageNo));

            return retrieveStockRequestMessage != null;
        }

        private bool DoesAdviceExists(string iwtAdvice)
        {
            var retrieveIwtByAdviceId =
                _commandAndQueryExecutor.Query(new RetrieveIwtByAdviceId(iwtAdvice));

            return retrieveIwtByAdviceId != null;
        }

        #endregion

    }
}