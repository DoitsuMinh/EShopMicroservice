using Ordering.Application.Configuration.CQRS.Commands;
using Ordering.Domain.Customers;
using Ordering.Domain.SeedWork;

namespace Ordering.Application.Customers.RegisterCustomer;

public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerUniquenessChecker _customerUniquenessChecker;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterCustomerCommandHandler(ICustomerRepository customerRepository, ICustomerUniquenessChecker customerUniquenessChecker, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _customerUniquenessChecker = customerUniquenessChecker;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerDto> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.CreateRegistered(
            request.Email,
            request.Name,
            _customerUniquenessChecker);

        await _customerRepository.AddAsync(customer);

        await _unitOfWork.CommitAsync(cancellationToken);

        return new CustomerDto { Id = customer.Id.Value };
    }
}
