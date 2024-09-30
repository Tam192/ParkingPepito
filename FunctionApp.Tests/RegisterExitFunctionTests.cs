using Application.Dtos;
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

namespace FunctionApp.Tests
{
    internal class RegisterExitFunctionTests
    {
        private RegisterExitFunction _function;
        private Mock<IRegisterExitUseCase> _useCase;
        private Mock<ILogger<RegisterExitFunction>> _logger;
        private Mock<IValidator<RegisterExitUseCaseDto>> _validator;

        [SetUp]
        public void Setup()
        {
            _useCase = new Mock<IRegisterExitUseCase>();
            _logger = new Mock<ILogger<RegisterExitFunction>>();
            _validator = new Mock<IValidator<RegisterExitUseCaseDto>>();
            _function = new RegisterExitFunction(_logger.Object, _useCase.Object, _validator.Object);
        }

        [Test]
        public async Task RunTestAsync()
        {
            _ = _useCase.Setup(x => x.RegisterExitAsync(It.IsAny<RegisterExitUseCaseDto>()))
                .Returns(Task.FromResult(
                    new BaseResponse<decimal>()
                    {
                        Results = [1]
                    }
                ));

            _ = _validator.Setup(x => x.ValidateAsync(It.IsAny<RegisterExitUseCaseDto>(), It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());

            RegisterExitUseCaseDto dto = RegisterExitFunctionMockedObject.GenerateRegisterExitUseCaseDtoMockedObject();
            MemoryStream body = new(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto)));

            Mock<FunctionContext> context = new();
            FakeHttpRequestData requestdata = new(
                context.Object,
                new Uri("http://localhost:7243/RegisterExitFunction"),
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
                Assert.That(objectResult?.Value, Is.InstanceOf<BaseResponse<decimal>>());
            });
        }
    }
}
