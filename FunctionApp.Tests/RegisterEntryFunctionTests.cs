using Application.Dtos;
using Application.Dtos.Entities;
using Application.Dtos.UseCases;
using Application.UseCases;
using FluentValidation;
using FluentValidation.Results;
using FunctionApp.Functions;
using FunctionApp.Tests.Utils.MockedObjects;
using FunctionApp.Tests.Utils.MockedObjects.Function;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Text;

namespace FunctionApp.Tests;

internal class RegisterEntryFunctionTests
{
    private RegisterEntryFunction _function;
    private Mock<IRegisterEntryUseCase> _useCase;
    private Mock<ILogger<RegisterEntryFunction>> _logger;
    private Mock<IValidator<RegisterEntryUseCaseDto>> _validator;

    [SetUp]
    public void Setup()
    {
        _useCase = new Mock<IRegisterEntryUseCase>();
        _logger = new Mock<ILogger<RegisterEntryFunction>>();
        _validator = new Mock<IValidator<RegisterEntryUseCaseDto>>();
        _function = new RegisterEntryFunction(_logger.Object, _useCase.Object, _validator.Object);
    }

    [Test]
    public async Task RunTestAsync()
    {
        _ = _useCase.Setup(x => x.RegisterEntryAsync(It.IsAny<RegisterEntryUseCaseDto>()))
            .Returns(Task.FromResult(
                new BaseResponse<StayDto>()
                {
                    Results = [RegisterEntryFunctionMockedObjects.GenerateStayDtoMockedObject()]
                }
            ));

        _ = _validator.Setup(x => x.ValidateAsync(It.IsAny<RegisterEntryUseCaseDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

        RegisterEntryUseCaseDto dto = RegisterEntryFunctionMockedObjects.GenerateRegisterEntryUseCaseDtoMockedObject();
        MemoryStream body = new(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto)));

        Mock<FunctionContext> context = new();
        FakeHttpRequestData requestdata = new(
            context.Object,
            new Uri("http://localhost:7243/RegisterEntryFunction"),
            body
        );

        IActionResult result = await _function.RunAsync(requestdata);
        Assert.That(result, Is.Not.Null);

        ObjectResult? objectResult = result as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(objectResult, Is.Not.Null);
            Assert.That(objectResult?.StatusCode, Is.EqualTo(200));
            Assert.That(objectResult is OkObjectResult, Is.True);
            Assert.That(objectResult?.Value, Is.Not.Null);
            Assert.That(objectResult?.Value, Is.InstanceOf<BaseResponse<StayDto>>());
        });
    }
}