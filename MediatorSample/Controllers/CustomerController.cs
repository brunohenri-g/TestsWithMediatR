using MediatorPatternExample.Domain.Customer.Command;
using MediatorPatternExample.Domain.Customer.Handler;
using MediatorPatternExample.Infra;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPatternExample.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerController> _logger;
        private readonly IRequestHandler<CustomerCreateCommand, string> _manualHandler;

        public CustomerController(
            IMediator mediator,
            ICustomerRepository customerRepository,
            ILogger<CustomerController> logger,
            IRequestHandler<CustomerCreateCommand, string> manualHandler
        )
        {
            _mediator = mediator;
            _customerRepository = customerRepository;
            _logger = logger;
            _manualHandler = manualHandler;

            var manualId = ((CustomerHandler)_manualHandler).GetUniqueId();
            _logger.LogInformation("IRequestHandler Injected Id: " + manualId);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CustomerCreateCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(CustomerUpdateCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = new CustomerDeleteCommand { Id = id };
            var result = await _mediator.Send(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            CustomerCreateCommand cmd = new CustomerCreateCommand() {
                FirstName = "Bruno"
            };            

            await _manualHandler.Handle(cmd, ct);
            var response = await _mediator.Send(cmd);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _customerRepository.GetById(id);
            return Ok(result);
        }
    }
}