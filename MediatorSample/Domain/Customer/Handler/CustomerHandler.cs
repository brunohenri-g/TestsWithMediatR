using MediatorPatternExample.Domain.Customer.Command;
using MediatorPatternExample.Domain.Customer.Entity;
using MediatorPatternExample.Infra;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MediatorPatternExample.Domain.Customer.Handler
{
    public class CustomerHandler :
        IRequestHandler<CustomerCreateCommand, string>,
        IRequestHandler<CustomerUpdateCommand, string>,
        IRequestHandler<CustomerDeleteCommand, string>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerHandler> _logger;
        private readonly Guid _uniqueId;

        public CustomerHandler(
            ICustomerRepository customerRepository,
            ILogger<CustomerHandler> logger,
            bool fromAsp = false
        )
        {
            _customerRepository = customerRepository;
            _logger = logger;
            _uniqueId = Guid.NewGuid();

            if (fromAsp)
                logger.LogInformation($"Manual Constructed ({_uniqueId})");
            else
                logger.LogInformation($"Automatic Constructed ({_uniqueId})");
        }

        public Guid GetUniqueId()
        {
            return _uniqueId;
        }

        public async Task<string> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"CustomerCreateCommand called ({_uniqueId})");

            var customer = new CustomerEntity(request.Id, request.FirstName, request.LastName, request.Email, request.Phone);
            await _customerRepository.Save(customer);

            return await Task.FromResult("Cliente registrado com sucesso");
        }

        public async Task<string> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var customer = new CustomerEntity(request.Id, request.FirstName, request.LastName, request.Email, request.Phone);
            await _customerRepository.Update(request.Id, customer);

            return await Task.FromResult("Cliente atualizado com sucesso");
        }

        public async Task<string> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            var client = await _customerRepository.GetById(request.Id);
            await _customerRepository.Delete(request.Id);

            return await Task.FromResult("Cliente excluido com sucesso");
        }
    }
}